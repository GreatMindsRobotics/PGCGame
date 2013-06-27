using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Glib.XNA;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;

namespace PGCGame
{
    public class SpaceMine : SecondaryWeapon
    {
        public int ExplosionRadius { get; set; }
        public int ExplosionDiameter { get; set; }

        public TimeSpan RemainingArmTime = TimeSpan.FromMilliseconds(2000);

        public SpaceMineState SpaceMineState { get; set; }
        public Ship ParentShip { get; set; }

        public SpaceMine(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {

        }

        public void Update(GameTime gameTime)
        {
            switch (SpaceMineState)
            {
                default:
                case CoreTypes.SpaceMineState.Stowed:
                    //Mine is stowed on board the ship, ready to be deployed

                    break;

                case CoreTypes.SpaceMineState.Deploying:
                    //Mine is deploying - set position, rotation, etc

                    Position = ParentShip.WorldCoords;
                    UseCenterAsOrigin = true;

                    SpaceMineState = CoreTypes.SpaceMineState.Deployed;

                    break;

                case CoreTypes.SpaceMineState.Deployed:
                    //Mine is deployed; start countdown to arm the mine

                    RemainingArmTime -= gameTime.ElapsedGameTime;
                    if (RemainingArmTime.Milliseconds <= 0)
                    {
                        SpaceMineState = CoreTypes.SpaceMineState.Armed;
                    }

                    break;

                case CoreTypes.SpaceMineState.Armed:
                    //Mine is armed; TODO: Wait for enemy
                    Rotation.Radians += .2f;

                    break;

                case CoreTypes.SpaceMineState.RIP:
                    //Mine is detonated; //TODO: remove from list
                    break;

            }

            base.Update();
        }
    }
}
