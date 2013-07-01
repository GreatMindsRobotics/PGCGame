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

using Glib.XNA;
using Glib;
using Glib.XNA.SpriteLib;


namespace PGCGame
{
    public class Bullet : Sprite
    {
        public Bullet(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
        }
        public int Damage { get; set; }

        private bool _isDead = false;


        public bool IsDead
        {
            get { return _isDead; }
            set { _isDead = value; }
        }
        
         
    }
}
