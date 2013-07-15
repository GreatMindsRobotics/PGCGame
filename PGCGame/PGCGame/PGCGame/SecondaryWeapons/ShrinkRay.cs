using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib;

namespace PGCGame
{
    public class ShrinkRay : SecondaryWeapon
    {
        public ShrinkRay(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            Cost = 2000;
            Name = "ShrinkRay";
        }
        
        public ShrinkBullet ShotBullet { get; set; }

        public override void Update(GameTime currentGameTime)
        {
            foreach (Ship ship in StateManager.ActiveShips)
            {
                if(Intersects(ship))
                {
                    ship.CurrentHealth = (ship.CurrentHealth * .66f).Round();
                    FireKilledEvent();
                }
            }
        }
    }
}
