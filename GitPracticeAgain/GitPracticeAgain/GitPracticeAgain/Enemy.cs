using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GitPracticeAgain
{
    class Enemy: BaseSprite
    {
        private Vector2 _speed;

        public Vector2 Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public void move()
        { 
            _position.X -= _speed.X;
        }
        

    }
}
