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
    public class Credits : BaseScreen
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
        private EventHandler<EventArgs> musicHandler;

        private void music_StateChange(object src, EventArgs data)
        {
            if (Visible && StateManager.Options.MusicEnabled && MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(_creditsSong);
            }
        }

        public Credits(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            _xmlCredits.LoadData();
            musicHandler = new EventHandler<EventArgs>(music_StateChange);
        }

        Sprite imgSprite;

        internal void PlayMusic()
        {
            MediaPlayer.Play(_creditsSong);
        }

        public override void InitScreen(ScreenType screenType)
        {
            base.InitScreen(screenType);
            MediaPlayer.MediaStateChanged -= musicHandler;
            MediaPlayer.MediaStateChanged += musicHandler;
            BackgroundSprite = new Sprite(GameContent.GameAssets.Images.Backgrounds.Screens[ScreenBackgrounds.Credits], Vector2.Zero, Sprites.SpriteBatch);
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;
            _scrollingSpeed = new Vector2(0, -1);

            Texture2D logo = GameContent.GameAssets.Images.Controls.Title;
            imgSprite = new Sprite(logo, new Vector2(0, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height), Sprites.SpriteBatch);
            imgSprite.X = imgSprite.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X;
            //credits = new TextSprite(Sprites.SpriteBatch, , "\n\n\n\nWeek 1 - Functional Spec, GameState Management\n
            //Week 3 - AI's\n\n\n\n\n\n\n\n\n\n\n\nWeek 4 - Xbox Converson\n\n\n\n\nUnderlying Library written by:\nGlen Husman (glen3b)\nGlib is available on github! \n\n\n\n\n\n\n                Music:\n\nFailing Defense - Kevin MacLeod\n\nAll music obtained from Incompetech.com", Color.White);
            //credits = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "   Plequarius: Galactic Commanders\n\n\n\n\n\nAll Developement:\nGlen Husman\n\nMinor Assistance:\nAbe", Color.White);

            //credits.Position = new Vector2(credits.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height+imgSprite.Height);

            SpriteFont creditsFont = GameContent.GameAssets.Fonts.NormalText;
            SpriteFont boldCreditsFont = GameContent.GameAssets.Fonts.BoldText;
            _creditsSong = GameContent.GameAssets.Music[ScreenMusic.Credits];

            int lastWeekID = 0;
            foreach (KeyValuePair<XmlCredits.Week, XmlCredits.Student> weekStudent in _xmlCredits.Students)
            {
                int weekID = weekStudent.Key.ID;

                if (lastWeekID != weekID)
                {
                    StringBuilder title = new StringBuilder();

                    title.AppendFormat("\n\n\n\nWeek {0} - ", weekID);
                    
                    for (int topicCounter = 0; topicCounter < weekStudent.Key.Topics.Count; topicCounter++)
                    {
                        KeyValuePair<int, string> topic = weekStudent.Key.Topics[topicCounter];
                        title.AppendFormat("{0}{1}", topic.Value, topicCounter == weekStudent.Key.Topics.Count - 1 ? "\n" : ", ");
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

                    lastWeekID = weekID;
                }

                TextSprite student = new TextSprite(Sprites.SpriteBatch, creditsFont, String.Format("{0} {1}\n", weekStudent.Value.FirstName, weekStudent.Value.LastName));
                student.X = student.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X;
                student.Color = Color.White;
                student.Y = credits[credits.Count - 1].Y + credits[credits.Count - 1].Height + (creditsFont.LineSpacing - creditsFont.MeasureString("A").Y);
                credits.Add(student);
            }

            AdditionalSprites.AddRange(credits);
            Sprites.Add(imgSprite);

            _elapsedTime = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState keyboard = Keyboard.GetState();

            _elapsedTime += gameTime.ElapsedGameTime;

            imgSprite.Position += _scrollingSpeed;

            foreach (TextSprite credit in credits)
            {
                credit.Position += _scrollingSpeed;
            }

            if (_elapsedTime >= _timeUntilCreditsFinish || keyboard.IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Stop();
                _elapsedTime = TimeSpan.Zero;
                StateManager.GoBack();
                
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
