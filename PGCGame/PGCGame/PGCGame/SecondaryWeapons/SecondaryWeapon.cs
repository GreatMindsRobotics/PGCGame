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
        public string Name { get; set; }
        public Ship ParentShip { get; set; }

        public Boolean fired = false;

        private bool _shouldShow;

        public event EventHandler Killed;

        protected void FireKilledEvent()
        {
            Killed(this, EventArgs.Empty);
        }

        public virtual bool ShouldDraw
        {
            get { return _shouldShow; }
            set { _shouldShow = value; }
        }


        public bool IsDead = false;

        public abstract void Update(GameTime currentGameTime);

        public SecondaryWeapon(Texture2D texture, Vector2 location, SpriteBatch spriteBatch)
            : base(texture, location, spriteBatch)
        {
            this.Killed += new EventHandler(SecondaryWeapon_Killed);
        }

        private void SecondaryWeapon_Killed(object sender, EventArgs e)
        {
            IsDead = true;
        }
    }
}
