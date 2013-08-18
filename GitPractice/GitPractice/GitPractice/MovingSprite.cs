using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace GitPractice
{
    public class MovingSprite : BaseSprite
    {
        /* TODO: JOEY:
         * If i hit "R" it should place any moving sprite to the center of the screen
         * The default moving keys should be the arrow keys                                 
         */

        public Keys KeyUp { get { return _KeyUp; } set { _KeyUp = value; } }

        public Keys KeyDown { get { return _KeyDown; } set { _KeyDown = value; } }

        public Keys KeyRight { get { return _KeyRight; } set { _KeyRight = value; } }

        public Keys KeyLeft { get { return _KeyLeft; } set { _KeyLeft = value; } }

        protected Keys _KeyUp;
        protected Keys _KeyDown;
        protected Keys _KeyRight;
        protected Keys _KeyLeft;
        protected Keys _KeyReset;

        protected Vector2 _speed;
        public Vector2 Speed { get { return _speed; } set { _speed = value; } }

        public void LoadContent(ContentManager content, string assetName, Vector2 speed)
        {
            _KeyLeft = Keys.Left;
            _KeyDown = Keys.Down;
            _KeyRight = Keys.Right;
            _KeyUp = Keys.Up;
            _KeyReset = Keys.R;

            _speed = speed;

            base.LoadContent(content, assetName);
        }

        public new void LoadContent(ContentManager content, string assetName)
        {
            LoadContent(content, assetName, new Vector2());
        }

        public virtual void Update(KeyboardState keyboard, GameTime gameTime, GameState gameState, Viewport viewport)
        {
            if (keyboard.IsKeyDown(_KeyUp) && Location.Y - Speed.Y > 0)
            {
                _location.Y  -= _speed.Y;
            }
            if (keyboard.IsKeyDown(_KeyDown) && Location.Y + Texture.Height + Speed.Y < viewport.Height)
            {
                _location.Y += _speed.Y;
            }
            if (keyboard.IsKeyDown(_KeyRight) && Location.X + Texture.Width + Speed.X < viewport.Width)
            {
                _location.X += _speed.X;
            }
            if (keyboard.IsKeyDown(_KeyLeft) && Location.X - Speed.X > 0)
            {
                _location.X -= _speed.X;
            }
            if (keyboard.IsKeyDown(_KeyReset))
            {
                Location = new Vector2((viewport.Width - _texture.Width) / 2, (viewport.Height - _texture.Height ) / 2);
            }




        }
    }
}
