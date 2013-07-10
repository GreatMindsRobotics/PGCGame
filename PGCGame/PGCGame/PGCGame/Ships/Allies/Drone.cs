using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;

using PGCGame.CoreTypes;
using PGCGame.Ships.Allies;

namespace PGCGame
{
    public class Drone : BaseAllyShip
    {
        #region Private Fields

        //TODO: Make this a selectable key in options 
        private Keys _deployKey = Keys.LeftShift;

        #endregion Private Fields

        #region Public Fields
        #endregion Public Fields

        #region Private Methods
        #endregion Private Methods

        #region Private Events

        #endregion Private Events

        #region Public Properties

        public override string FriendlyName
        {
            get { return "Drone"; }
        }

        public FighterCarrier ParentShip { get; set; }
        public DroneState DroneState { get; set; }

        #endregion Public Properties

        #region Public Ctors

        public Drone(Texture2D texture, Vector2 location, SpriteBatch spriteBatch, FighterCarrier parent)
            : base(texture, location, spriteBatch)
        {
            ParentShip = parent;

            UseCenterAsOrigin = true;
            Position = ParentShip.Position;
            WorldCoords = ParentShip.WorldCoords;
            Scale = new Vector2(.75f);
            RotateTowardsMouse = false;
            _initHealth = 10;
            BulletTexture = GameContent.GameAssets.Images.Ships.Bullets[ShipType.Drone, ShipTier.Tier1];
            DamagePerShot = 1;
        }

        #endregion Public Ctors

        #region Public Methods

        TimeSpan _elapsedRotationDelay = TimeSpan.Zero;
        TimeSpan _rotationDelay = new TimeSpan(0, 0, 0, 0, 5);

        TimeSpan _MinFireRate = new TimeSpan(0, 0, 0, 0, 15);
        TimeSpan _MaxFireRate = new TimeSpan(0, 0, 0, 0, 150);
        TimeSpan? _DelayTillNextFire = null;

        Ship closestEnemyShip = null;
        Vector2? closestEnemyShipDistance = null;
        Vector2? shipDistance = null;


        public override void Shoot()
        {
            base.Shoot();
        }

        private bool isEnemyDetected()
        {
            //finds the closes ship 
            foreach (Ship allyShip in StateManager.ActiveShips)
            {
                if (allyShip.PlayerType == CoreTypes.PlayerType.Enemy)
                {
                    if (!shipDistance.HasValue && !closestEnemyShipDistance.HasValue)
                    {
                        shipDistance = allyShip.WorldCoords - this.WorldCoords;
                        closestEnemyShipDistance = shipDistance;
                        closestEnemyShip = allyShip;
                    }
                    else
                    {
                        shipDistance = allyShip.WorldCoords - this.WorldCoords;
                        if (shipDistance.Value.LengthSquared() < closestEnemyShipDistance.Value.LengthSquared())
                        {
                            closestEnemyShipDistance = shipDistance;
                            closestEnemyShip = allyShip;
                        }
                    }
                }
            }

            //TODO: Cleanup magic numbers
            return (closestEnemyShipDistance.HasValue && closestEnemyShip != null && closestEnemyShipDistance.Value.LengthSquared() < Math.Pow(400, 2) && closestEnemyShip.CurrentHealth > 0);          
        }


        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            _elapsedRotationDelay += gameTime.ElapsedGameTime;

            if (_elapsedRotationDelay > _rotationDelay)
            {
                //Drone AI FSM
                switch (DroneState)
                {
                    default:
                    case CoreTypes.DroneState.Stowed:
                        //Drone is stowed on the Figher Carrier; wait for deploy command. This is the default state

                        if (keyboard.IsKeyDown(_deployKey) && _lastKs != null && !_lastKs.IsKeyDown(Keys.LeftShift))
                        {
                            DroneState = DroneState.Deploying;
                        }

                        break;

                    case CoreTypes.DroneState.Deploying:
                        //Deploy command was given; allow "Stow" command (same key as deploy); else, continue deploying

                        if (keyboard.IsKeyDown(_deployKey) && _lastKs != null && !_lastKs.IsKeyDown(Keys.LeftShift))
                        {
                            DroneState = DroneState.Stowing;

                            _lastKs = keyboard;
                            return;
                        }

                        //Push the drones out by changing origin slowly
                        Vector2 distanceToParentOrigin = ParentShip.Origin - Origin;

                        if (distanceToParentOrigin.LengthSquared() < 1.0f)
                        {
                            DroneState = CoreTypes.DroneState.Deployed;
                            return;
                        }
                        else
                        {
                            if (Scale.X < .75f)
                            {
                                Scale.X += .03f;
                                Scale.Y += .03f;
                            }
                            else
                            {
                                distanceToParentOrigin.Normalize();
                                Origin += distanceToParentOrigin;
                            }
                        }

                        break;

                    case CoreTypes.DroneState.Deployed:
                        //Drone is deployed; monitor for enemies, and listen for "Stow" command (same key as deploy)s
                        if (keyboard.IsKeyDown(_deployKey) && _lastKs != null && !_lastKs.IsKeyDown(Keys.LeftShift))
                        {
                            DroneState = DroneState.Stowing;

                            _lastKs = keyboard;
                            return;
                        }

                        //TODO: DEBUG: Show monitoring radius                    
                        if (isEnemyDetected())
                        {
                            DroneState = DroneState.TargetAcquired;
                        }

                        break;

                    case CoreTypes.DroneState.TargetAcquired:

                        if (!isEnemyDetected())
                        {
                            DroneState = DroneState.Deployed;
                        }

                        if (!_DelayTillNextFire.HasValue)
                        {
                            _DelayTillNextFire = StateManager.RandomGenerator.NextTimeSpan(_MinFireRate, _MaxFireRate);
                        }
                        else if (_DelayTillNextFire.Value.Milliseconds <= 0)
                        {
                            Shoot();
                            _DelayTillNextFire = null;
                        }
                        else
                        {
                            _DelayTillNextFire -= gameTime.ElapsedGameTime;
                        }
                        break;



                    case CoreTypes.DroneState.Stowing:
                        //Drone received a stow command (either while it began deployment, or was fully deployed)
                        //Monitor for re-deployment command (same key as stow)

                        if (keyboard.IsKeyDown(_deployKey) && _lastKs != null && !_lastKs.IsKeyDown(Keys.LeftShift))
                        {

                            DroneState = DroneState.Deploying;

                            _lastKs = keyboard;
                            return;
                        }

                        //Pull the drones back by changing origin slowly
                        Vector2 distanceToParentCenter = Origin - new Vector2(Width / 2, Height / 2);

                        if (distanceToParentCenter.LengthSquared() < 1.0f)
                        {
                            //Shrink into mother-ship
                            if (Scale.X > .3f)
                            {
                                Scale.X -= .05f;
                                Scale.Y -= .05f;
                            }
                            else
                            {
                                DroneState = CoreTypes.DroneState.Stowed;
                                return;
                            }
                        }
                        else
                        {
                            distanceToParentCenter.Normalize();
                            Origin -= distanceToParentCenter;
                        }
                        break;

                }



                if (DroneState == CoreTypes.DroneState.Stowed)
                {
                    return;
                }

                if (!new DroneState[] { DroneState.Attacking, DroneState.AcceptingFate,
                    DroneState.EvadingFire, DroneState.MovingToTarget, DroneState.TargetAcquired, DroneState.RIP}
                    .Contains(DroneState))
                {
                    Position = ParentShip.Position; //+ new Vector2(ParentShip.Width / 2, ParentShip.Height / 2);
                    Rotation += .5f;
                }

                _lastKs = keyboard;

                base.Update(gameTime);
            }
        }

        public override void DrawNonAuto()
        {
            if (DroneState == CoreTypes.DroneState.Stowed)
            {
                return;
            }

            //TODO: Needs better handling for drawing drones while still having them recognize world coords they are at
            //For the draw, set the world coordinates to ParentShip.Position
            Vector2 worldCoords = WorldCoords;
            WorldCoords = ParentShip.Position;

            base.DrawNonAuto();

            //After the draw, reset world coordinates to actual world coordinates
            WorldCoords = worldCoords;
        }

        public override void Draw()
        {
            if (DroneState == CoreTypes.DroneState.Stowed)
            {
                return;
            }

            //TODO: Needs better handling for drawing drones while still having them recognize world coords they are at
            //For the draw, set the world coordinates to ParentShip.Position
            Vector2 worldCoords = WorldCoords;
            WorldCoords = ParentShip.Position;
            
            base.Draw();

            //After the draw, reset world coordinates to actual world coordinates
            WorldCoords = worldCoords;
        }

        public override ShipType ShipType
        {
            get { return ShipType.Drone; }
        }

        #endregion Public Methods
    }
}
