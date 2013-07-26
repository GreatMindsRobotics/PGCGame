using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GitPracticeAgain
{
    class BaseSprite
    {
        protected Vector2 _position;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        protected Texture2D _texture;

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        protected Color _tintColor;

        public Color Color
        {
            get { return _tintColor; }
            set { _tintColor = value; }
        }

        public void LoadContent(Vector2 position, Texture2D texture, Color color)
        {
            _position = position;
            _texture = texture;
            _tintColor = color;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_texture, _position, _tintColor);
        }
        

    }
}
