using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using blah;

namespace blah
{
<<<<<<< HEAD
    public class Player : Sprite
=======
    public class Player : Sprite //Had to make this public?
>>>>>>> Added player to the game, player functions, etc.......................................................
    {
        private int _damage;
        private int _shoot;
        private int _lastShot;
        private Vector2 _bSpeed;

        private Texture2D _texture;
        private Texture2D _bulletTex;
        private List<Bullet> _bullets = new List<Bullet>();

<<<<<<< HEAD
        public Player(Texture2D texture, Texture2D bTex, Vector2 pos, SpriteBatch sb, int damage, int speed, int shoot, int bSpeed)
=======
        public Player(Texture2D texture, Vector2 pos, SpriteBatch sb, int damage, Vector2 speed, int shoot, Vector2 bSpeed)
>>>>>>> Added player to the game, player functions, etc.......................................................
            : base(texture, pos, sb)
        {
            this._texture = texture;
            this._bulletTex = bTex;
            this._damage = damage;
            this.Speed = speed;
            this._shoot = shoot;
            this._bSpeed = bSpeed;
            this._lastShot = 0;
        }

<<<<<<< HEAD
        public void Update(KeyboardState kState, GameTime time)
=======
        public void updatePlayer(KeyboardState kState, GameTime time) //OVERRIDE?
>>>>>>> Added player to the game, player functions, etc.......................................................
        {

            foreach (Bullet bullet in _bullets)
             {
                bullet.Update();
            }

            if (kState.IsKeyDown(Keys.Space))
            {
                this._lastShot++;

                if (this._shoot <= this._lastShot)
                {
                    this._lastShot = 0;

                    //this._bullets.Add(new Bullet(this._bulletTex, this.Position, this.SpriteBatch, this._damage, this._bSpeed));
                }
            }
        }

        public void setDamage(int damage)
        {
            this._damage = damage;
        }

        public void setBulletTexture(Texture2D bTexture)
        {
            this._bulletTex = bTexture;
        }

        public int getDamage()
        {
            return _damage;
        }

        public void setSpeed(Vector2 speed)
        {
            this.Speed = speed;
        }

        public Vector2 getSpeed()
        {
            return Speed;
        }

        public void setBSpeed(Vector2 bSpeed)
        {
            this._bSpeed = bSpeed;
        }

        public Vector2 getBSpeed()
        {
            return _bSpeed;
        }
    }
}
