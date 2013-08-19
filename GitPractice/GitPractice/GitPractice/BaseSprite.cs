using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GitPractice
{
    public abstract class BaseSprite
    {


        protected Vector2 _location;
        
        public Vector2 Location
        {
            get
            {
                return _location;
            }

            set
            {
                _location = value;
            }
        }
        
        protected Texture2D _texture;
        
        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
            }
        }
       
        protected Color _tintColor;
        
        public Color TintColor
        {
            get
            {
                return _tintColor;
            }

            set
            {
                _tintColor = value;
            }

        }

        public virtual void LoadContent(ContentManager content, string assetName)
        { 
           _location = new Vector2 (0, 0);
           _texture = content.Load<Texture2D>(assetName);
           _tintColor = Color.White;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, _tintColor);      
        }
    }
}
