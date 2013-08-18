using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        private Rectangle _rect;

        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }
        
    }
}
