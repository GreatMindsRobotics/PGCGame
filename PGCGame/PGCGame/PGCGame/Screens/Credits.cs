using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;

namespace PGCGame.Screens
{
    public class Credits : Screen
    {
        //TODO FOR MATTHEW:
        //MAKE THE CREDITS END AND GO BACK TO THE MAINMENU
        private TextSprite credits;
        private Song _creditsSong;
        private Vector2 _scrollingSpeed;
        
        private TimeSpan _timeUntilCreditsFinish = new TimeSpan (0, 0, 0, 45, 0);
        private TimeSpan _elapsedTime;


        public Credits(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            
        }

        Sprite imgSprite;

        public void LoadContent(ContentManager content)
        {
            BackgroundSprite = new Sprite(content.Load<Texture2D>("Images\\Background\\1920by1080SkyStar"), Vector2.Zero, Sprites.SpriteBatch);
            Texture2D buttonImage = content.Load<Texture2D>("Images\\Controls\\Button");
            SpriteFont SegoeUIMono = content.Load<SpriteFont>("Fonts\\SegoeUIMono");
            _scrollingSpeed = new Vector2(0, -1);
            
            Texture2D logo = content.Load<Texture2D>("Images\\Controls\\Gametitle");
            imgSprite = new Sprite(logo, new Vector2(0, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height), Sprites.SpriteBatch);
            imgSprite.X = imgSprite.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X;
            credits = new TextSprite(Sprites.SpriteBatch, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "\n\n\n\nWeek 1 - Functional Spec, GameState Management\n\n\n\n- Kai F.\n\n\n- Michael K.\n\n\n- Alexa L.\n\n\n- Andrea L.\n\n\n- Alexander L.\n\n\n- Matthew P.\n\n\n- Jeremiah T.\n\n\n\n\nWeek 2 - Technical Spec, Class Design, Functionality\n\n\n\n- Glen H.\n\n\n- Michael K.\n\n\n- Alex L.\n\n\n- Matthew P.\n\n\n\n\n\n\n Week 3 - AI's\n\n\n\n\n\n\n\n\n\n\n\nWeek 4 - Xbox Converson\n\n\n\n\nUnderlying Library written by:\nGlen Husman (glen3b)\nGlib is available on github! \n\n\n\n\n\n\n                Music:\n\nFailing Defense - Kevin MacLeod\n\nAll music obtained from Incompetech.com", Color.White);
            //credits = new TextSprite(Sprites.SpriteBatch, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "   Plequarius: Galactic Commanders\n\n\n\n\n\nAll Developement:\nGlen Husman\n\nMinor Assistance:\nAbe", Color.White);

            credits.Position = new Vector2(credits.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height+imgSprite.Height);

            _creditsSong = content.Load<Song>("Songs\\Failing Defense");

            AdditionalSprites.Add(credits);
            Sprites.Add(imgSprite);

            _elapsedTime = new TimeSpan();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState keyboard = Keyboard.GetState();

            _elapsedTime += gameTime.ElapsedGameTime;

            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(_creditsSong);
            }
            imgSprite.Position += _scrollingSpeed;
            credits.Position += _scrollingSpeed;

            if (_elapsedTime >= _timeUntilCreditsFinish || keyboard.IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Stop();
                _elapsedTime = new TimeSpan();
                StateManager.ScreenState = ScreenState.Title;
            }
        }
    }
}
