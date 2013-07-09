using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib.XNA.SpriteLib;

namespace PGCGame.CoreTypes
{
    public abstract class BaseScreen : Glib.XNA.SpriteLib.Screen
    {
        public BaseScreen(SpriteBatch spriteBatch, Color color)
            : base(spriteBatch, _debugBackground ? Color.Red : color)
        {
            
        }

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
        }

        public virtual void InitScreen(ScreenType screenName)
        {
            _screenType = screenName;
            base.Name = screenName.ToString();
        }

#if XBOX
        protected Vector2 _currentlySelectedButton = Vector2.Zero;
        protected Dictionary<Vector2, TextSprite> _screenButtons = new Dictionary<Vector2, TextSprite>();
#endif
    }
}
