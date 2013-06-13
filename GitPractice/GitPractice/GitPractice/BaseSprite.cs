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
        /*
         * TODO: KAI:
         * 
         * Protected fields (aka variables): _texture, _location, _tintColor
         */

        public Texture2D Texture { get; set; }
        public Vector2 Location { get; set; }
        public Color TintColor { get; set; }

        public void LoadContent(ContentManager content, string assetName)
        { 
            //TODO: Make this work        
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //TODO: Make this work        
        }
    }
}
