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
    class SizableCfg : ISizable
    {

        private float _height;

        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }

        private float _width;

        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }
        
    }

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

        private SpriteFont _creditsFont = GameContent.GameAssets.Fonts.NormalText;
        private SpriteFont _boldCreditsFont = GameContent.GameAssets.Fonts.BoldText;


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
            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            //Get the new viewport from EventArgs
            Viewport viewport = e.Cast<ViewportEventArgs>().Viewport;

            //Re-position title based on new viewport
            gameTitle.X = gameTitle.GetCenterPosition(viewport).X;
            gameTitle.Y = viewport.Height;

            //Re-position all credits based on new viewport
            for (int i = 0; i < AdditionalSprites.Count; i++)
            {
                TextSprite credit = AdditionalSprites[i].Cast<TextSprite>();

                credit.X = credit.GetCenterPosition(viewport).X;

                if (i == 0)
                {
                    credit.Y = gameTitle.Y + gameTitle.Height;
                }
                else
                {
                    TextSprite prevCredit = AdditionalSprites[i - 1].Cast<TextSprite>();
                    credit.Y = prevCredit.Y + prevCredit.Height + (_creditsFont.LineSpacing - _creditsFont.MeasureString("A").Y);
                }
            }
        }

        Sprite gameTitle;

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
            gameTitle = new Sprite(logo, new Vector2(0, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height), Sprites.SpriteBatch);
            gameTitle.X = gameTitle.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X;
            //credits = new TextSprite(Sprites.SpriteBatch, , "\n\n\n\nWeek 1 - Functional Spec, GameState Management\n
            //Week 3 - AI's\n\n\n\n\n\n\n\n\n\n\n\nWeek 4 - Xbox Converson\n\n\n\n\nUnderlying Library written by:\nGlen Husman (glen3b)\nGlib is available on github! \n\n\n\n\n\n\n                Music:\n\nFailing Defense - Kevin MacLeod\n\nAll music obtained from Incompetech.com", Color.White);
            //credits = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "   Plequarius: Galactic Commanders\n\n\n\n\n\nAll Developement:\nGlen Husman\n\nMinor Assistance:\nAbe", Color.White);

            //credits.Position = new Vector2(credits.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height+imgSprite.Height);

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

                    TextSprite credit = new TextSprite(Sprites.SpriteBatch, _boldCreditsFont, title.ToString());
                    credit.X = credit.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X;
                    credit.Color = Color.White;

                    if (credits.Count == 0)
                    {
                        credit.Y = gameTitle.Y + gameTitle.Height;
                    }
                    else
                    {
                        credit.Y = credits[credits.Count - 1].Y + credits[credits.Count - 1].Height + (_creditsFont.LineSpacing - _creditsFont.MeasureString("A").Y);
                    }
                    credits.Add(credit);

                    lastWeekID = weekID;
                }

                TextSprite student = new TextSprite(Sprites.SpriteBatch, _creditsFont, String.Format("{0} {1}\n", weekStudent.Value.FirstName, weekStudent.Value.LastName));
                student.X = student.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X;
                student.Color = Color.White;
                student.Y = credits[credits.Count - 1].Y + credits[credits.Count - 1].Height + (_creditsFont.LineSpacing - _creditsFont.MeasureString("A").Y);
                credits.Add(student);
            }



            TextSprite helperLabel = new TextSprite(Sprites.SpriteBatch, _boldCreditsFont, "\n\n\n\nHelpers\n", Color.White);
            helperLabel.X = helperLabel.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X;
            helperLabel.Y = credits[credits.Count - 1].Y + credits[credits.Count - 1].Height + (_creditsFont.LineSpacing - _creditsFont.MeasureString("A").Y);
            credits.Add(helperLabel);

            foreach (PGCGame.Xml.XmlTypes.XmlCredits.Helper assistant in _xmlCredits.AllHelpers)
            {
                float targetY = credits[credits.Count - 1].Y + credits[credits.Count - 1].Height + (_creditsFont.LineSpacing - _creditsFont.MeasureString("A").Y);
                TextSprite name = new TextSprite(Sprites.SpriteBatch, new Vector2(0, targetY), _boldCreditsFont, assistant.FullName, Color.White);
                TextSprite job = new TextSprite(Sprites.SpriteBatch, new Vector2(0, targetY), _creditsFont, string.Format(" - {0}\n", assistant.Job), Color.White);
                SizableCfg temp = new SizableCfg();
                temp.Width = name.Width + job.Width;
                temp.Height = (name.Height + job.Height) / 2;
                name.X = temp.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X;
                job.X = name.X + name.Width;
                name.Y = targetY;
                job.Y = targetY;
                credits.Add(name);
                credits.Add(job);
            }

            //The IEnumerable cast method
            AdditionalSprites.AddRange(credits.Cast<ISprite>());
            Sprites.Add(gameTitle);

            _elapsedTime = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState keyboard = Keyboard.GetState();

            _elapsedTime += gameTime.ElapsedGameTime;

            gameTitle.Position += _scrollingSpeed;

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
