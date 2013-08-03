using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Glib.XNA;
using Glib;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;

namespace PGCGame
{
    public class SpaceMine : SecondaryWeapon
    {
        public int ExplosionRadius { get; set; }
        public int ExplosionDiameter { get; set; }

        public TimeSpan RemainingArmTime = TimeSpan.FromMilliseconds(4000);

        private SpaceMineState _spaceMineState;

        public SpaceMineState SpaceMineState
        {
            get { return _spaceMineState; }
            set
            {
                _spaceMineState = value;
                if (_spaceMineState == CoreTypes.SpaceMineState.RIP)
                {
                    FireKilledEvent();
                }
            }
        }



        public override bool ShouldDraw
        {
            get
            {
                return _spaceMineState != CoreTypes.SpaceMineState.Stowed && _spaceMineState != CoreTypes.SpaceMineState.RIP;
            }
            set
            {
                base.ShouldDraw = value;
            }
        }

        public SpaceMine(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Cost = 500;
            Damage = 50;
            Name = "SpaceMine";
        }

        public override void Update(GameTime gameTime)
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
                    //Mine is armed; Wait for enemy
                    Rotation.Radians += .2f;
                    foreach (Ship ship in StateManager.EnemyShips)
                    {
                        checkIfShipHitMine(ship);
                    }

                    //Ally and player ships can also be hurt by the mine
                    foreach (Ship ship in StateManager.AllyShips)
                    {
                        checkIfShipHitMine(ship);
                    }

                    break;

                case CoreTypes.SpaceMineState.RIP:

                    FireKilledEvent();
                    break;

            }

            base.Update();
        }

        private void checkIfShipHitMine(Ship ship)
        {
            if (ship is Drone)
            {
                if (ship.Cast<Drone>().DroneState == DroneState.Stowed || ship.Cast<Drone>().DroneState == DroneState.RIP)
                {
                    return;
                }
            }

            if (Intersects(ship.WCrectangle))
            {
                ship.CurrentHealth -= this.Damage;
                SpaceMineState = CoreTypes.SpaceMineState.RIP;
            }
        }
    }
}
