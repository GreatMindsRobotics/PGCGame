using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GitPractice
{
    public class MovingSprite
    {
        /* TODO: MATTHEW:
         * 
         * Inherit from BaseSprite
         * CANNOT move offscreen
         * 
         * Protected fields (aka variables): _keyUp, _keyDown, _keyLeft, _keyRight
         *                                   _speed
         *                                   
         */

        public Keys KeyUp { get; set; }
        public Keys KeyDown { get; set; }
        public Keys KeyRight { get; set; }
        public Keys KeyLeft { get; set; }

        public Vector2 Speed { get; set; }

        public void Update(KeyboardState keyboard, GameTime gameTime, GameState gameState, Viewport viewport)
        {
        
        }
        
    }
}
