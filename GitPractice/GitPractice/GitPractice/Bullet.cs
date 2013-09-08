using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GitPractice
{
    public class Bullet : MovingSprite
    {
        private bool _isDead;

        public bool IsDead
        {
            get { return _isDead; }
            set { _isDead = value; }
        }

        private Rectangle _rect = new Rectangle(0, 0, 4, 17);

        public Rectangle Rect
        {            
            get { return _rect; }
        }

        public override void Update(KeyboardState keyboard, GameTime gameTime, GameState gameState, Viewport viewport)
        {
            base.Update(keyboard, gameTime, gameState, viewport);

            _rect.X = (int)_location.X;
            _rect.Y = (int)_location.Y;
        }
    }
}
