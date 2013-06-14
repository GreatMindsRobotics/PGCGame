using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GitPractice
{
    public sealed class ScrollingBackground : BaseSprite
    {
        private Vector2 _scrollingSpeed;
        public Vector2 ScrollingSpeed { get; set; }

        public void LoadContent(ContentManager content, string assetName, Vector2 scrollingSpeed)
        {
            _scrollingSpeed = scrollingSpeed;

            base.LoadContent(content, assetName);      
        }

        public void Update(GameTime gameTime, GameState gameState, MoveDirection scrollDirection)
        {

        }            

        public override void Draw(SpriteBatch spriteBatch)
        { 
            
        }
    }
}
