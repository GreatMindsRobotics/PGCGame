using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace blah
{
    class Player : Sprite
    {
        private int _damage;
        private int _speed;
        private int _shoot;
        private int _lastShot;
        private int _bSpeed;

        private Texture2D _bulletTex;
        private List<Bullet> _bullets = new List<Bullet>();

        public Player(Texture2D texture, Vector2 pos, SpriteBatch sb, int damage, int speed, int shoot, int bSpeed)
            : base(texture, pos, sb)
        {
            this._damage = damage;
            this._speed = speed;
            this._shoot = shoot;
            this._bSpeed = bSpeed;
            this._lastShot = 0;
        }

        public override void Update(KeyboardState kState, GameTime time)
        {
            base.Update();

            if (kState.IsKeyDown(Keys.Space))
            {
                this._lastShot++;

                if (this._shoot <= this._lastShot)
                {
                    this._lastShot = 0;

                    this._bullets.Add(new Bullet(this._bulletTex, this.Position, this.SpriteBatch, this._damage, this._bSpeed));
                }
            }
        }

        public void setDamage(int damage)
        {
            this._damage = damage;
        }

        public int getDamage()
        {
            return _damage;
        }

        public void setSpeed(int speed)
        {
            this._speed = speed;
        }

        public int getSpeed()
        {
            return _speed;
        }

        public void setBSpeed(int bSpeed)
        {
            this._bSpeed = bSpeed;
        }

        public int getBSpeed()
        {
            return _bSpeed;
        }
    }
}
