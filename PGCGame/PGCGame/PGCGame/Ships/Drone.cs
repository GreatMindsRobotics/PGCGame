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

namespace PGCGame
{
    public class Drone : Ship
    {

        public Drone(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
        }
        public int ShotsFromMain { get; set; }
        //must add event from fighter carrier

        public override void Shoot()
        {
            //Every 4th shot of main, shoot
            if (ShotsFromMain % 4 == 0)
            {
                //Ben magic
                Bullet bullet = new Bullet(BulletTexture, Position, SpriteBatch);
                MouseState ms = Mouse.GetState();
                bullet.Damage = DamagePerShot;
                Vector2 mousePos = new Vector2(ms.X, ms.Y);
                Vector2 slope = mousePos - Position;
                slope.Normalize();
                bullet.Speed = slope;
                FlyingBullets.Add(bullet);
            }
            //throw new NotImplementedException();
        }
    }
}
