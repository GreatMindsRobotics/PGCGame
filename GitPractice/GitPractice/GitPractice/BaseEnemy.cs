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
        public MoveDirection MoveDirection { get; set; }
        public Vector2 Speed { get; set; }

        public virtual void Update(GameTime gameTime, GameState gameState, MoveDirection moveDirection, Viewport viewport)
        {
            _rect.X = (int)_location.X;
            _rect.Y = (int)_location.Y;
        }

        protected Rectangle _rect;

        public Rectangle Rect
        {
            get { return _rect; }           
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content, string assetName)
        {
            base.LoadContent(content, assetName);

            _rect = new Rectangle((int)_location.X, (int)_location.Y, _texture.Width, _texture.Height);
        }

    }
}
