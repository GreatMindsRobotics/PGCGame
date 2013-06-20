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
        private List<TextSprite> credits = new List<TextSprite>();
        private Song _creditsSong;
        private Vector2 _scrollingSpeed;
        private string[] creditLines = new string[]{
            "\n\n\n\nWeek 1 - Functional Spec, GameState Management\n",
            "Kai F.\n",
            "Michael K.\n",
            "Alexa L.\n",
            "Andrea L.\n",
            "Alexander L.\n",
            "Matthew P.\n",
            "Jeremiah T.\n\n\n",
            "Week 2 - Technical Spec, Class Design, Functionality\n",
            "Glen H.\n",
            "Michael K.\n",
            "Alex L.\n",
            "Matthew P.\n\n\n",
            "Week 3 - AI Programming\n",
            "...\n",
            "...\n\n\n",
            "Week 4 - Xbox Converson\n",
            "...\n",
            "...\n\n\n",
            "Underlying Sprite/Screen Management Library",
            "Glen Husman (glen3b)",
            "Glib is available on github!\n\n\n\n\n\n\n",
            "Credits Music:",
            "Failing Defense - Kevin MacLeod\n\n",
            "All music obtained from Incompetech.com"
        };
        
        private TimeSpan _timeUntilCreditsFinish = TimeSpan.FromSeconds(38);
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
            //credits = new TextSprite(Sprites.SpriteBatch, , "\n\n\n\nWeek 1 - Functional Spec, GameState Management\n
            //Week 3 - AI's\n\n\n\n\n\n\n\n\n\n\n\nWeek 4 - Xbox Converson\n\n\n\n\nUnderlying Library written by:\nGlen Husman (glen3b)\nGlib is available on github! \n\n\n\n\n\n\n                Music:\n\nFailing Defense - Kevin MacLeod\n\nAll music obtained from Incompetech.com", Color.White);
            //credits = new TextSprite(Sprites.SpriteBatch, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "   Plequarius: Galactic Commanders\n\n\n\n\n\nAll Developement:\nGlen Husman\n\nMinor Assistance:\nAbe", Color.White);

            //credits.Position = new Vector2(credits.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height+imgSprite.Height);

            SpriteFont creditsFont = content.Load<SpriteFont>("Fonts\\SegoeUIMono");
            _creditsSong = content.Load<Song>("Songs\\Failing Defense");

            for (int i = 0; i < creditLines.Length; i++)
            {
                TextSprite credit = new TextSprite(Sprites.SpriteBatch, creditsFont, creditLines[i]);
                credit.X = credit.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X;
                credit.Color = Color.White;
                if (i == 0)
                {
                    credit.Y = imgSprite.Y + imgSprite.Height;
                }
                else
                {
                    credit.Y = credits[i - 1].Y + credits[i-1].Height+(creditsFont.LineSpacing-creditsFont.MeasureString("A").Y);
                }
                credits.Add(credit);
            }

            AdditionalSprites.AddRange(credits);
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
            foreach (TextSprite credit in credits)
            {
                credit.Position += _scrollingSpeed;
            }

            if (_elapsedTime >= _timeUntilCreditsFinish || keyboard.IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Stop();
                _elapsedTime = new TimeSpan();
                StateManager.ScreenState = ScreenState.Title;
            }
        }
    }
}
