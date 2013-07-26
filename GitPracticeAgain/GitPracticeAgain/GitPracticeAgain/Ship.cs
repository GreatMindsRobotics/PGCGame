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
        private Vector2 Speed = new Vector2(0,0);
        
        
        public void Update(KeyboardState keyboardState)
        {

        }


        
        private void move(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                Speed.X = 3;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {

            }
        }
    }
}
