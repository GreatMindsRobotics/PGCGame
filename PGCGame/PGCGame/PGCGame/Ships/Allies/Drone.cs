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
using Glib.XNA.SpriteLib;

using PGCGame.CoreTypes;

namespace PGCGame
{
    public class Drone : Ship
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
            _rotateTowardsMouse = false;

            BulletTexture = Ship.DroneBullet;
        }

        #endregion Public Ctors

        #region Public Methods

        public override void Shoot()
        {
            //TODO: One shot per "range" - 400px from shot origin
        }

        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();

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

                    //TODO: Monitor for enemies within 400x400px square
                    //TODO: DEBUG: Show monitoring radius                    

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

            Position = ParentShip.Position; //+ new Vector2(ParentShip.Width / 2, ParentShip.Height / 2);
            Rotation += .5f;

            _lastKs = keyboard;

            base.Update();
        }

        public override void DrawNonAuto()
        {
            if (DroneState == CoreTypes.DroneState.Stowed)
            {
                return;
            }

            base.DrawNonAuto();
        }

        public override void Draw()
        {
            if (DroneState == CoreTypes.DroneState.Stowed)
            {
                return;
            }

            base.Draw();
        }

        public override string TextureFolder
        {
            get { throw new NotImplementedException(); }
        }

        #endregion Public Methods
    }
}
