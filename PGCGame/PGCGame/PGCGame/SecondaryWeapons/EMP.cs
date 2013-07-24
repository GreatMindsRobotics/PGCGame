using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Input;
using Glib;

namespace PGCGame
{
    public class EMP : SecondaryWeapon
    {

        public EMP(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            UseCenterAsOrigin = true;
            Scale = Vector2.Zero;
            Cost = 1000;
            Name = "EMP";
            
        }

        public int Radius { get; set; }
        public int Diameter
        {
            get
            {
                return Radius * 2;
            }
            set
            {
                Radius = value / 2;
            }
        }

        TimeSpan updateDelay = new TimeSpan(0, 0, 0, 0, 1);
        TimeSpan elapsedUpdateDelay = TimeSpan.Zero;


        private Vector2 _pointOfOrigin;
        private Boolean firstShouldDraw = true;

        private EMPState _EMPState;
        public EMPState PublicEMPState
        {
            get { return _EMPState; }
            set
            {
                _EMPState = value;
                if (_EMPState == CoreTypes.EMPState.RIP)
                {
                    FireKilledEvent();
                }
            }
        }

        TimeSpan maxTime = new TimeSpan(0, 0, 5);
        TimeSpan elapsedMaxTime = TimeSpan.Zero;

        private bool _hasDeployInited = false;

        public override void Update(GameTime currentGameTime)
        {
            foreach (Ship ship in StateManager.ActiveShips)
            {
                if (ship != ParentShip && ship.PlayerType == PlayerType.Enemy && ship.Intersects(this))
                {
                    ship.Cast<Ships.Enemies.BaseEnemyShip>().isEMPed = true;
                }
            }
            switch (_EMPState)
            {
                default:
                case CoreTypes.EMPState.Stowed:

                    break;

                case CoreTypes.EMPState.Deployed:

                    if (!_hasDeployInited)
                    {
                        _pointOfOrigin = ParentShip.WorldCoords;
                        _hasDeployInited = true;
                    }
                    
                    ShouldDraw = true;

                    elapsedUpdateDelay += currentGameTime.ElapsedGameTime;
                    if (elapsedUpdateDelay > updateDelay)
                    {
                        elapsedUpdateDelay = TimeSpan.Zero;
                        this.Scale += new Vector2(.015f, .015f);
                    }

                    elapsedMaxTime += currentGameTime.ElapsedGameTime;
                    if (elapsedMaxTime > maxTime)
                    {
                        _EMPState = EMPState.RIP;
                    }

                    break;

                case CoreTypes.EMPState.RIP:
                    FireKilledEvent();
                    break;


            }
            if (ShouldDraw && firstShouldDraw)
            {
                Position = ParentShip.WorldCoords;
                firstShouldDraw = false;
            }
        }
    }
}
