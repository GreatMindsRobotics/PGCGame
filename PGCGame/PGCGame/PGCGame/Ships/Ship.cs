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
    public abstract class Ship : Sprite
    {
        //TODO: ALEX


        public Ship(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
        }

        public abstract void Shoot();

        //override: 
        //Update
        //DrawNonAuto

        public override void DrawNonAuto()
        {
            base.DrawNonAuto();
            throw new NotImplementedException();
            //TODO: Draw Bullets
        }

        public int DamagePerShot { get; set; }
        public int Cost { get; set; }

        public Vector2 Speed { get; set; }

        public int CurrentHealth { get; set; }

        public int InitialHealth { get; set; }

        public int Shield { get; set; }

        public int Armor { get; set; }

        public List<Bullet> FlyingBullets { get; set; }
    }
}
