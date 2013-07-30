using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GitPractice
{
    public class coin3 : BaseEnemy
    {
        Vector2 newSpeed = new Vector2(8, 6);

        public override void Update(GameTime gameTime, GameState gameState, MoveDirection moveDirection, Viewport viewport)
        {
            if (gameState == GameState.Playing)
            {
                _tintColor = Color.Fuchsia;

                if (Location.X < 0 || Location.X + Texture.Width > viewport.Width)
                {
                    moveDirection = moveDirection == MoveDirection.Left ? MoveDirection.Right : MoveDirection.Left;
                    newSpeed.X *= -1;
                }

                if (Location.Y + Texture.Height > viewport.Height || Location.Y < 0)
                {
                    newSpeed.Y *= -1;
                }

                Location = Location + newSpeed;


            }
        }
    }
}
