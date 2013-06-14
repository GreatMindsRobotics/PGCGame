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
                if (moveDirection == MoveDirection.Left)
                {
                    if (Speed.X > 0)
                    {
                        Speed = Speed * -1;
                    }
                }
                else if (moveDirection == MoveDirection.Right)
                {
                    if (Speed.X < 0)
                    {
                        Speed = Speed * -1;
                    }
                }
                else
                {
                    Speed = new Vector2(0, 0);
                }

                Location = Location + Speed;

                if (Location.X < 0 || Location.X + Texture.Width > viewport.Width)
                {
                    if (moveDirection == GitPractice.MoveDirection.Left)
                    {
                        moveDirection = GitPractice.MoveDirection.Right;
                    }
                    else
                    {
                        moveDirection = GitPractice.MoveDirection.Left;
                    }
                }
            }
        }

    }
}
