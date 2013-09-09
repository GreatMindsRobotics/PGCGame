using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Glib.XNA.SpriteLib;

namespace blah
{
    public class Bullet : Sprite
    {
        private int _damage;
        private int _speed;
        private Player _owner;

        public Bullet(Texture2D texture, Vector2 pos, SpriteBatch sb, int damage, int speed)
            : base(texture, pos, sb)
        {
            this._damage = damage;
            this._speed = speed;
        }

        public override void Update()
        {
            base.Update();
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

        public void setOwner(Player owner)
        {
            this._owner = owner;
        }

        public Player getPlayer()
        {
            return this._owner;
        }
    }
}
