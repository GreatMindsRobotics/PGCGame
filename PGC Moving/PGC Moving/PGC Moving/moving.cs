using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace PGC_Moving
{
    class moving
    {
        
        
    
        protected Keys KeyRight;
        public Keys KeyRight;
        
            get
            {
                return _keyRight;
            }

            set
            {
                _keRight = value;
            }
         
       protected Keys KeyDown;
        public Keys KeyDown;
        {
            get
            {
                return _keyDown;
            }

            set
            {
                _keyDown = value;
            }
        }
        protected  Keys keyUp;
        public Keys KeyUp;
        {
            get
            {
                return _keyFire;
            }

            set
            {
                _keyFire = value;
            }
        }
        protected Keys.keyLeft
        
        
      protected Vector2 _speed;

        public Vector2 Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }


        _speed = new Vector2(5);

         
   
        if (keyboard.IsKeyDown(keyUp) && _vector.Y > 0)
        {
            _vector.Y -= _speed.Y;
        }
    public void Update(GameTime gametime, KeyboardState keyboard, Viewport viewport, GameState gamestate);
        
            base.Update(gametime, keyboard);
        if (keyboard.IsKeyDown(_keyDown) && _vector.Y + _texture.Height < viewport.Height)
        {
            _vector.Y += _speed.Y;
        }
        if (keyboard.IsKeyDown(_keyRight) && _vector.X + _texture.Width < viewport.Width)
        {
            _vector.X += _speed.X;
        }
        if (keyboard.IsKeyDown(_keyLeft) && _vector.X > 0)
        {
            _vector.X -= _speed.X;
        }
    if (keyboard.IsKeyDown(_keyUp) && _vector.Y < viewport.Height)
        {
            _vector.Y -= _speed.Y;
        }
        
            _keyDown = Keys.S;
                _keyUp = Keys.W;
                _keyLeft = Keys.A;
                _keyRight = Keys.D;
        
       
    }
}
