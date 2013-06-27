using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PGCGame
{
    public class ShrinkRay : SecondaryWeapon
    {
        public ShrinkRay(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {

        }

        public ShrinkBullet ShotBullet { get; set; }
    }
}
