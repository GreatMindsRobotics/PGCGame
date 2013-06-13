using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GitPractice
{
    public abstract class BaseEnemy : BaseSprite
    {
        //TODO: Jeremiah

        public MoveDirection MoveDirection { get; set; }
        public Vector2 Speed { get; set; }

        public abstract void Update(GameTime gameTime, GameState gameState, MoveDirection moveDirection, Viewport viewport);            
    }
}
