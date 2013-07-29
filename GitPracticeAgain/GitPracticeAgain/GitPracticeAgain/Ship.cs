using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GitPracticeAgain
{
    class Ship : BaseSprite
    {
         Vector2 Speed = new Vector2(5,5);
        
        
        public void Update(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                _position.Y -= Speed.Y;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _position.X -= Speed.X;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                _position.X += Speed.X;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                _position.Y += Speed.Y;
            }

            
        }

    }
}
