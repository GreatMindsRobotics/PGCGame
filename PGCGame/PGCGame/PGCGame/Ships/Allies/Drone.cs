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
using Glib.XNA.InputLib;

namespace PGCGame
{
    public class Drone : BaseAllyShip
    {
        #region Private Fields
        
        //TODO: Make this a selectable key in options 
#if WINDOWS
        public static Keys DeployKey = Keys.LeftShift;


      
#elif XBOX
        private bool _xboxDroneDeployTriggered = false;
#endif

        #endregion Private Fields

        #region Public Fields
        #endregion Public Fields

        #region Private Methods

        private bool OtherDroneDetects()
        {
            return ParentShip.Drones.First<Drone>(d => d.ShipID != this.ShipID).isEnemyDetected();
        }

        #endregion Private Methods

        #region Private Events
#if XBOX
        void Buttons_XButtonPressed(object sender, EventArgs e)
        {
            _xboxDroneDeployTriggered = true;
        }
#endif
        #endregion Private Events

        #region Public Properties

        public FighterCarrier ParentShip { get; set; }
        public DroneState DroneState { get; set; }

        #endregion Public Properties

        #region Public Ctors

        public Drone(Texture2D texture, Vector2 location, SpriteBatch spriteBatch, FighterCarrier parent)
            : base(texture, location, spriteBatch, true)
        {
            ParentShip = parent;

            _hasHealthBar = false;

            UseCenterAsOrigin = true;
            Position = ParentShip.Position;
            WorldCoords = ParentShip.WorldCoords;
            Scale = new Vector2(.75f);
            RotateTowardsMouse = false;
            _initHealth = 10;
            //BulletTexture = GameContent.Assets.Images.Ships.Bullets[ShipType.Drone, ShipTier.Tier1];
            DamagePerShot = 1;
#if XBOX
            GamePadManager.One.Buttons.XButtonPressed += new EventHandler(Buttons_XButtonPressed);
#endif
        }

        static Drone()
        {
            _cost[ShipTier.Tier1] = 5000;
            _cost[ShipTier.Tier2] = 10000;
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
            if (closestEnemyShipDistance.HasValue)
            {
                //Glen's experiment
                DroneShoot = GameContent.Assets.Sound[SoundEffectType.DronesShoot];
                Bullet bullet = StateManager.BulletPool.GetBullet();
                bullet.InitializePooledBullet(WorldCoords, this);
                bullet.SpriteBatch = WorldSb;
                
                Vector2 normalizedDistance = this.closestEnemyShipDistance.Value;
                bullet.Rotation = new SpriteRotation(normalizedDistance.ToAngle(), AngleType.Radians);
                normalizedDistance.Normalize();
                normalizedDistance *= 3f;
                bullet.Speed = normalizedDistance;
                //bullet.Rotation = Rotation;
                bullet.Damage = DamagePerShot;
                if(StateManager.Options.SFXEnabled)
                {
                   DroneShoot.Play();
                }
                StateManager.AllyBullets.Legit.Add(bullet);
                FireBulletEvent(bullet);
            }
        }

        private bool isEnemyDetected()
        {
            //finds the closes ship 
            foreach (Ship enemyShip in StateManager.EnemyShips)
            {
                if (!shipDistance.HasValue && !closestEnemyShipDistance.HasValue)
                {
                    shipDistance = enemyShip.WorldCoords - this.WorldCoords;
                    closestEnemyShipDistance = shipDistance;
                    closestEnemyShip = enemyShip;
                }
                else
                {
                    shipDistance = enemyShip.WorldCoords - this.WorldCoords;
                    if (shipDistance.Value.LengthSquared() < closestEnemyShipDistance.Value.LengthSquared())
                    {
                        closestEnemyShipDistance = shipDistance;
                        closestEnemyShip = enemyShip;
                    }
                }
            }

            //TODO: Cleanup magic numbers
            return (closestEnemyShipDistance.HasValue && closestEnemyShip != null && closestEnemyShipDistance.Value.LengthSquared() < Math.Pow(400, 2) && closestEnemyShip.CurrentHealth > 0);          
        }

        public static readonly DroneState[] BadStates = { DroneState.Attacking, DroneState.AcceptingFate,
                    DroneState.EvadingFire, DroneState.MovingToTarget, DroneState.TargetAcquired, DroneState.RIP};

        public override void Update(GameTime gameTime)
        {

            KeyboardState keyboard = Keyboard.GetState();

            DroneDeploy = GameContent.Assets.Sound[SoundEffectType.DronesDeploy];

            if (CurrentHealth <= 0)
            {
                this.shipState = CoreTypes.ShipState.Exploding;
                DroneState = CoreTypes.DroneState.RIP;
            }

            _elapsedRotationDelay += gameTime.ElapsedGameTime;

            if (_elapsedRotationDelay > _rotationDelay)
            {
#region Drone AI FSM
                switch (DroneState)
                {
                    default:
                    case CoreTypes.DroneState.Stowed:
                        //Drone is stowed on the Figher Carrier; wait for deploy command. This is the default state
#if WINDOWS
                        if (StateManager.InputManager.IsKeyDownOnFrame(DeployKey))
                        {
                            if(StateManager.Options.SFXEnabled)
                            {
                               DroneDeploy.Play();
                            }

                            DroneState = DroneState.Deploying;
                        }
#elif XBOX
                        if(_xboxDroneDeployTriggered)
                        {
                            DroneState = DroneState.Deploying;
                            _xboxDroneDeployTriggered = false;
                        }
#endif
                        break;

                    case CoreTypes.DroneState.Deploying:
                        //Deploy command was given; allow "Stow" command (same key as deploy); else, continue deploying

#if WINDOWS
                        if (StateManager.InputManager.IsKeyDownOnFrame(DeployKey) && !OtherDroneDetects())
                        {
                            DroneState = DroneState.Stowing;
                            return;
                        }
#elif XBOX
                        if(_xboxDroneDeployTriggered && !OtherDroneDetects())
                        {
                            DroneState = DroneState.Stowing;
                            _xboxDroneDeployTriggered = false;
                            return;
                        }
#endif

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
#if WINDOWS
                        if (StateManager.InputManager.IsKeyDownOnFrame(DeployKey) && !OtherDroneDetects())
                        {
                            if (StateManager.Options.SFXEnabled)
                            {
                                DroneDeploy.Play();
                            }

                            DroneState = DroneState.Stowing;
                            return;
                        }
#elif XBOX
                        if(_xboxDroneDeployTriggered && !OtherDroneDetects())
                        {
                            DroneState = DroneState.Stowing;
                            _xboxDroneDeployTriggered = false;

                            return;
                        }
#endif
                        //TODO: DEBUG: Show monitoring radius
                        //OtherDroneShooting() or !OtherDroneShooting() ?
                        if (isEnemyDetected() || OtherDroneDetects())
                        {
                            DroneState = DroneState.TargetAcquired;
                        }
                        
                        break;

                    case CoreTypes.DroneState.TargetAcquired:

                        //OtherDroneShooting() or !OtherDroneShooting() ?
                        if (!isEnemyDetected() && !OtherDroneDetects())
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

#if WINDOWS
                        if (StateManager.InputManager.IsKeyDownOnFrame(DeployKey))
                        {

                            DroneState = DroneState.Deploying;
                            return;
                        }
#elif XBOX
                        if(_xboxDroneDeployTriggered)
                        {
                            DroneState = DroneState.Deploying;
                            _xboxDroneDeployTriggered = false;
                            return;
                        }
#endif

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
#endregion



                if (DroneState == CoreTypes.DroneState.Stowed)
                {
                    return;
                }
                else
                {
                }

                if (!
                    BadStates.Contains(DroneState))
                {
                    Position = ParentShip.Position; //+ new Vector2(ParentShip.Width / 2, ParentShip.Height / 2);
                    Rotation += .5f;
                }

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

        //No draw override needed

        public override ShipType ShipType
        {
            get { return ShipType.Drone; }
        }

        #endregion Public Methods
    }
}
