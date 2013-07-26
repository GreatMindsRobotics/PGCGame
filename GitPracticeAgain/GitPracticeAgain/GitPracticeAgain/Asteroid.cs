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

namespace GitPracticeAgain
{
    class Asteroid: BaseSprite
    {
        Random r = new Random();
        private int _speed;

        public Asteroid(Viewport viewport)
        {
            _speed = r.Next(1, 11);
            _position = new Vector2();
            _position.Y = 0;//viewport.Height - 100;
            _position.X = r.Next(0, viewport.Width - 100);

        }
        public void LoadContent(Texture2D texture, Color color)
        {

            _texture = texture;
            _tintColor = color;
        }
        public void Update()
        {
            _position.X += _speed;
            _position.Y += 5;

        }

    }
}
