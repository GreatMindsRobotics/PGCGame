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
    public abstract class Ship : Sprite, ITimerSprite
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

        public Texture2D BulletTexture { get; set; }

        public override void Update()
        {
            base.Update();
            MouseState ms = Mouse.GetState();
            Vector2 mousePos = new Vector2(ms.X, ms.Y);
            Vector2 targetPos = mousePos - Position;
            //Rotate towards mouse
            Rotation.Radians = Math.Atan2(targetPos.X, -targetPos.Y).ToFloat();
        }

        private TimeSpan _elapsedShotTime = new TimeSpan();

        public void Update(GameTime gt)
        {
            Update();
            //Shoot
            KeyboardState ks = Keyboard.GetState();
            _elapsedShotTime += gt.ElapsedGameTime;
            //Shoot w/ space key
            if (CanShoot && ks.IsKeyDown(Keys.Space))
            {
                Shoot();
                _elapsedShotTime = new TimeSpan();
            }
        }

        public bool CanShoot
        {
            get
            {
                return _elapsedShotTime >= DelayBetweenShots;
            }
        }

        public override void DrawNonAuto()
        {
            base.DrawNonAuto();
            foreach (Bullet b in FlyingBullets)
            {
                b.DrawNonAuto();
            }
            //TODO: Draw Bullets
        }

        public int DamagePerShot { get; set; }
        public int Cost { get; set; }

        public TimeSpan DelayBetweenShots { get; set; }

        public Vector2 Speed { get; set; }

        public int CurrentHealth { get; set; }

        public int InitialHealth { get; set; }

        public int Shield { get; set; }

        public int Armor { get; set; }

        private List<Bullet> _flyingBullets = new List<Bullet>();

        public List<Bullet> FlyingBullets
        {
            get { return _flyingBullets; }
            set { _flyingBullets = value; }
        }
        
    }
}
