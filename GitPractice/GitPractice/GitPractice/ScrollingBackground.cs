using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GitPractice
{
    public sealed class ScrollingBackground : BaseSprite
    {
        /* TODO: Michael
         *
         * Private Fields (aka variables): _scrollingSpeed
         *
         */

        public Vector2 ScrollingSpeed { get; set; }

        public void Update(GameTime gameTime, GameState gameState, MoveDirection scrollDirection)
        { 
            //TODO: Make this work
        }

        public override void Draw(SpriteBatch spriteBatch)
        { 
            //TODO: Make this work, too!
        }
    }
}
