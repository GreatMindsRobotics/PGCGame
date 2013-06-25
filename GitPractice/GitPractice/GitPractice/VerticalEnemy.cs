using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GitPractice
{
    public class VerticalEnemy : BaseEnemy
    {
        /*TODO: Jeremiah
         * 
         * CANNOT move offscreen
         * 
         */

        public override void Update(GameTime gameTime, GameState gameState, MoveDirection moveDirection, Viewport viewport)
        {
            Location = Location + Speed;

            if (Location.Y < 0 || Location.Y > viewport.Height)
            {

            }
        }

    }
} 
