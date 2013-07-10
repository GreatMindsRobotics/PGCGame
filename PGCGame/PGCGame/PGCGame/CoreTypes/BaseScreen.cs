using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib.XNA.SpriteLib;
using Glib.XNA.InputLib;

namespace PGCGame.CoreTypes
{
    public abstract class BaseScreen : Glib.XNA.SpriteLib.Screen
    {
        public BaseScreen(SpriteBatch spriteBatch, Color color)
            : base(spriteBatch, _debugBackground ? Color.Red : color)
        {
#if XBOX
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
            GamePadManager.One.Buttons.BButtonPressed += new EventHandler(Buttons_BButtonPressed);
#endif
        }



#if XBOX
        TimeSpan elapsedBackButtonTime = TimeSpan.Zero;
        TimeSpan requiredBackButtonTime = TimeSpan.FromMilliseconds(250);

        void Buttons_BButtonPressed(object sender, EventArgs e)
        {
            if (_screenType != ScreenType.MainMenu && Visible && elapsedBackButtonTime > requiredBackButtonTime)
            {
                StateManager.GoBack();
            }
            else if (_screenType == CoreTypes.ScreenType.MainMenu && Visible && elapsedBackButtonTime > requiredBackButtonTime)
            {
                StateManager.Exit();
            }
        }

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                elapsedBackButtonTime = TimeSpan.Zero;
            }
        }
#endif

        private static bool _debugBackground = false;
        public static bool DebugBackground
        {
            get
            {
                return _debugBackground;
            }
        }

        public PGCGame.CoreTypes.Delegates.NextRun RunNextUpdate = null;

        private ScreenType _screenType;
        public ScreenType ScreenType
        {
            get { return _screenType; }
        }

        public new string Name
        {
            get { return _screenType.ToString(); }
            private set { }
        }

        public override void Update(GameTime game)
        {
            base.Update(game);
            if (RunNextUpdate != null)
            {
                RunNextUpdate();
                RunNextUpdate = null;
            }
#if XBOX
            elapsedBackButtonTime += game.ElapsedGameTime;
#endif
        }

        public virtual void InitScreen(ScreenType screenName)
        {
            _screenType = screenName;
            base.Name = screenName.ToString();
        }

#if XBOX
        //protected Vector2 _currentlySelectedButton = Vector2.Zero;
        //protected Dictionary<Vector2, TextSprite> _screenButtons = new Dictionary<Vector2, TextSprite>();
#endif
    }
}
