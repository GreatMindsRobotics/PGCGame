using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PGCGame
{
    public class EMP : SecondaryWeapon
    {

        public EMP(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
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

        public Ship LaunchingShip { get; set; }

        public override void Update(GameTime currentGameTime)
        {
            throw new NotImplementedException();
        }
    }
}
