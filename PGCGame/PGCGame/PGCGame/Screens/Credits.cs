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

using PGCGame.CoreTypes;
using PGCGame.Xml.XmlTypes;

namespace PGCGame.Screens
{
    public class Credits : Screen
    {
        private List<TextSprite> credits = new List<TextSprite>();
        private Song _creditsSong;
        private Vector2 _scrollingSpeed;

        private XmlCredits _xmlCredits = XmlBaseLoader.Create<XmlCredits>(XmlDataFile.Credits);
      
        private string[] creditLines = new string[]
        {
            "<b>\n\n\n\nWeek # - Topic 1, Topic 2\n",
            "Student 1.\n",
            "Student 2.\n",
            "Student 3.\n\n\n",
            "<b>Week # - Topic 1, Topic 2, Topic 3\n",
            "Student 1.\n",
            "Student 2.\n",
            "Student 3.\n\n\n",
            "<b>Underlying Sprite/Screen Management Library",
            "Glen Husman (glen3b)",
            "Glib is available on github!\n\n\n\n\n\n\n",
            "<b>Credits Music:",
            "Failing Defense - Kevin MacLeod\n\n",
            "<b>All music obtained from Incompetech.com"
        };
        
        private TimeSpan _timeUntilCreditsFinish = TimeSpan.FromSeconds(38);
        private TimeSpan _elapsedTime;


        public Credits(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            _xmlCredits.LoadData();
        }

        Sprite imgSprite;

        public void LoadContent(ContentManager content)
        {
            BackgroundSprite = new Sprite(content.Load<Texture2D>("Images\\Background\\1920by1080SkyStar"), Vector2.Zero, Sprites.SpriteBatch);
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
            SpriteFont boldCreditsFont = content.Load<SpriteFont>("Fonts\\SegoeUIMonoBold");
            _creditsSong = content.Load<Song>("Songs\\Failing Defense");

            foreach (KeyValuePair<XmlCredits.Week, XmlCredits.Student> weekStudent in _xmlCredits.Students)
            {
                StringBuilder title = new StringBuilder();

                int weekID = weekStudent.Key.ID;
                title.Append(String.Format("Week {0} - ", weekID));

                foreach (KeyValuePair<int, string> topic in weekStudent.Key.Topics)
                {
                    title.Append(String.Format("{0}, ", topic.Value));
                }

                TextSprite credit = new TextSprite(Sprites.SpriteBatch, boldCreditsFont, title.ToString());
                credit.X = credit.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X;
                credit.Color = Color.White;

                if (credits.Count == 0)
                {
                    credit.Y = imgSprite.Y + imgSprite.Height;
                }
                else
                {
                    credit.Y = credits[credits.Count - 1].Y + credits[credits.Count - 1].Height + (creditsFont.LineSpacing - creditsFont.MeasureString("A").Y);
                }
                credits.Add(credit);
            }

/*
            for (int i = 0; i < creditLines.Length; i++)
            {
                TextSprite credit = new TextSprite(Sprites.SpriteBatch, creditLines[i].Length > 3 && creditLines[i].Substring(0,3) == "<b>" ? boldCreditsFont : creditsFont, creditLines[i].Replace("<b>", ""));
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
*/
            AdditionalSprites.AddRange(credits);
            Sprites.Add(imgSprite);

            _elapsedTime = new TimeSpan();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState keyboard = Keyboard.GetState();

            _elapsedTime += gameTime.ElapsedGameTime;

            if (MediaPlayer.State != MediaState.Playing && StateManager.Options.MusicEnabled )
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
                //credits.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .255f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height);

                float offset = Sprites.SpriteBatch.GraphicsDevice.Viewport.Height - Sprites[0].Position.Y;
                Sprites[0].Y += offset;

                foreach (TextSprite credit in credits)
                {
                    credit.Y += offset;
                }

            }
            
        }
    }
}
