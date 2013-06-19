using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitPractice
{
    public class HorizontalEnemy : BaseEnemy
    {
        /*TODO: Alexa
         * 
         * Can only move left or right; MUST NOT accept any other movemenet
         * 
         */

        public override void Update(GameTime gameTime, GameState gameState, MoveDirection moveDirection, Viewport viewport)
        {

            if (gameState == GameState.Playing)
            {

                if (Location.X < 0 || Location.X + Texture.Width > viewport.Width)
                {
                    moveDirection = moveDirection == GitPractice.MoveDirection.Left ? MoveDirection.Right : MoveDirection.Left;
                    Speed *= -1;
                }


                Location = Location + Speed;

                
            }
        }

    }
}
