using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;

namespace PGCGame.Screens
{
    public class LoadingScreen : BaseScreen
    {
        private string _loadingText = "";

        public string LoadingText
        {
            get { return _loadingText; }
            set
            {
                _loadingText = value;
                if (loadingText != null)
                {
                    loadingText.Text = value;
                }
            }
        }


        public LoadingScreen(SpriteBatch sb, Color bgColor)
            : base(sb, bgColor)
        {

        }

        private TimeSpan _elapsedTime;

        public TimeSpan MinimumTime = TimeSpan.FromSeconds(.4);

        private bool _hasFinishedTask = false;

        public AsyncCallback Callback
        {
            get
            {
                return new AsyncCallback(AsyncCallback);
            }
        }

        /// <summary>
        /// Mark the task as finished.
        /// </summary>
        public void FinishTask()
        {
            _hasFinishedTask = true;
        }

        public AsyncCallback UserCallback;

        public bool UserCallbackStartsTask = false;

        public void AsyncCallback(IAsyncResult result)
        {
            if (UserCallback != null)
            {
                UserCallback(result);
            }
            if (!UserCallbackStartsTask)
            {
                FinishTask();
            }
        }

        TextSprite loadingText;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            loadingText = new TextSprite(this.Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, _loadingText, Color.White);
            loadingText.Position = loadingText.GetCenterPosition(Graphics.Viewport);
            loadingText.TextChanged += new EventHandler(loadingText_TextChanged);
            AdditionalSprites.Add(loadingText);
        }

        void loadingText_TextChanged(object sender, EventArgs e)
        {
            loadingText.Position = loadingText.GetCenterPosition(Graphics.Viewport);
        }

        public event EventHandler ScreenFinished;

        public void Reset()
        {
            _elapsedTime = TimeSpan.Zero;
            ScreenFinished = null;
            _hasFinishedTask = false;
        }

        public override void Update(GameTime game)
        {
            base.Update(game);
            _elapsedTime += game.ElapsedGameTime;
            if (_elapsedTime >= MinimumTime && _hasFinishedTask && ScreenFinished != null)
            {
                ScreenFinished(this, EventArgs.Empty);
            }
        }
    }
}
