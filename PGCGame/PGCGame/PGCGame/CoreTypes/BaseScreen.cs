using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib.XNA.SpriteLib;
using Glib.XNA.InputLib;
using Microsoft.Xna.Framework.Audio;

namespace PGCGame.CoreTypes
{
    public abstract class BaseScreen : Glib.XNA.SpriteLib.Screen
    {
        public BaseScreen(SpriteBatch spriteBatch, Color color)
            : base(spriteBatch, StateManager.DebugData.DebugBackground ? Color.Red : color)
        {
            ButtonClick = GameContent.GameAssets.Sound[SoundEffectType.ButtonPressed];
#if XBOX
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
            GamePadManager.One.Buttons.BButtonPressed += new EventHandler(Buttons_BButtonPressed);
#endif
        }



#if XBOX
        protected Sprite bButton;
        protected TextSprite bLabel;

        protected Sprite aButton;
        protected TextSprite aLabel;

        TimeSpan elapsedBackButtonTime = TimeSpan.Zero;
        TimeSpan requiredBackButtonTime = TimeSpan.FromMilliseconds(250);

        protected GamePadButtonEnumerator AllButtons;

        void Buttons_BButtonPressed(object sender, EventArgs e)
        {
            if (_screenType != ScreenType.MainMenu && _screenType != ScreenType.LoadingScreen && _screenType != ScreenType.Game && Visible && elapsedBackButtonTime > requiredBackButtonTime)
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

        public PGCGame.CoreTypes.Delegates.NextRun RunNextUpdate = null;

        private ScreenType _screenType;
        public ScreenType ScreenType
        {
            get { return _screenType; }
        }

        public override string Name
        {
            get { return _screenType.ToString(); }
            set { }
        }

        public SoundEffectInstance DeploySound { get; set; }
        public SoundEffectInstance SpaceShipLeaving { get; set; }
        public SoundEffectInstance ItemBought { get; set; }
        public SoundEffectInstance ButtonClick { get; set; }
        public SoundEffectInstance ClonesMade { get; set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (RunNextUpdate != null)
            {
                RunNextUpdate();
                RunNextUpdate = null;
            }
#if XBOX
            elapsedBackButtonTime += gameTime.ElapsedGameTime;
            if(AllButtons != null)
            {
                AllButtons.Update(gameTime);
            }
#endif
        }

        public virtual void InitScreen(ScreenType screenName)
        {
#if XBOX
            if (screenName != CoreTypes.ScreenType.LoadingScreen)
            {
                bButton = new Sprite(GameContent.GameAssets.Images.Controls.BButton, new Vector2(50, 420), Sprites.SpriteBatch);
                bButton.Scale = new Vector2(0.5f);

                string backButtonText = "Back";
                if (screenName == ScreenType.MainMenu || screenName == ScreenType.Title)
                {
                    backButtonText = "Exit";
                }

                bLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, backButtonText);
                bLabel.Color = Color.White;
                bLabel.Position = new Vector2(90, 425);

                Sprites.Add(bButton);
                AdditionalSprites.Add(bLabel);

                aButton = new Sprite(GameContent.GameAssets.Images.Controls.AButton, new Vector2(160, 420), Sprites.SpriteBatch);
                aButton.Scale = new Vector2(0.5f);

                aLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "Select");
                aLabel.Position = new Vector2(200, 425);
                aLabel.Color = Color.White;

                if (screenName == ScreenType.Credits)
                {
                    aLabel.Color = Color.Transparent;
                    aButton.Color = Color.Transparent;
                }
                else if (screenName == ScreenType.LevelSelect)
                {
                    aLabel.Text = "Play";
                }

                Sprites.Add(aButton);
                AdditionalSprites.Add(aLabel);
            }
#endif
            _screenType = screenName;
            base.Name = screenName.ToString();
        }

#if XBOX
        //protected Vector2 _currentlySelectedButton = Vector2.Zero;
        //protected Dictionary<Vector2, TextSprite> _screenButtons = new Dictionary<Vector2, TextSprite>();
#endif
    }
}
