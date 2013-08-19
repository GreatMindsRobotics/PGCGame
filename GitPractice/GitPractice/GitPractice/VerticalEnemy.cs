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
        Vector2 newSpeed = new Vector2(3, 12);

        public override void Update(GameTime gameTime, GameState gameState, MoveDirection moveDirection, Viewport viewport)
        {
            _tintColor = Color.Green;
            Location = Location + newSpeed;

            if (Location.Y < 0 || Location.Y + Texture.Height > viewport.Height)
            {
                newSpeed.Y *= -1;
            }
            if (Location.X < 0 || Location.X + Texture.Width > viewport.Width)
            {
                newSpeed.X *= -1;
            }

            base.Update(gameTime, gameState, moveDirection, viewport);
        }

    }
} 
