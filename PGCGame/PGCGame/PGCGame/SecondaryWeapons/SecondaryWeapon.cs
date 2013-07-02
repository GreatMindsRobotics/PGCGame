using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PGCGame
{
    public abstract class SecondaryWeapon : Sprite
    {
        public int Cost { get; set; }
        public int Damage { get; set; }
        public bool SingleUse { get; set; }
       
        public SecondaryWeapon(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {

        }
    }
}
