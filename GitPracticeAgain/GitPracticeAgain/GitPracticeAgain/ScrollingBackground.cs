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
    class ScrollingBackground: BaseSprite
    {
        public void Update()
        {

            if (_position.X + _texture.Width == 0)
            {
                _position.X = 0;
            }
            else
            {
                _position.X += _speed * -1;
            }
        }
        private int _speed;



        public void LoadContent(Vector2 position, Texture2D texture, Color color, int speed)
        {
            _position = position;
            _texture = texture;
            _tintColor = color;
            _speed = speed;
        }
    
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_texture, _position, _tintColor);
            batch.Draw(_texture, new Vector2(_position.X + _texture.Width, _position.Y), _tintColor);
        }
    }
}
