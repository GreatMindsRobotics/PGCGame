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

        Vector2 newSpeed = new Vector2(4, 3);

        public override void Update(GameTime gameTime, GameState gameState, MoveDirection moveDirection, Viewport viewport)
        {
            if (gameState == GameState.Playing)
            {
                _tintColor = Color.Maroon;

                if (Location.X < 0 || Location.X + Texture.Width > viewport.Width)
                {
                    moveDirection = moveDirection == MoveDirection.Left ? MoveDirection.Right : MoveDirection.Left;
                    newSpeed.X *= -1;
                }

                if(Location.Y + Texture.Height > viewport.Height || Location.Y < 0)
                {
                    newSpeed.Y *= -1;
                }

                Location = Location + newSpeed;

                
            }
        }

    }
}
