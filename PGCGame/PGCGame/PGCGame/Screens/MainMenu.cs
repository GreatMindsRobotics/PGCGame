using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;

using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

#if XBOX

using Glib.XNA.InputLib;
using Microsoft.Xna.Framework.Input;

#endif

namespace PGCGame.Screens
{
    public class MainMenu : BaseScreen
    {

        public MainMenu(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);

            ButtonClick = GameContent.GameAssets.Sound[SoundEffectType.ButtonPressed];
        }

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                elapsedButtonDelay = TimeSpan.Zero;
            }
        }

        Sprite TitleSprite;

        Sprite planet;
        Sprite planettwo;

        Sprite SinglePlayerButton;
        TextSprite SinglePlayerLabel;

        Sprite MultiPlayerButton;
        TextSprite MultiPlayerLabel;

        Sprite BackButton;
        TextSprite BackLabel;

        Sprite OptionsButton;
        TextSprite OptionsLabel;

        Sprite CreditsButton;
        TextSprite CreditsLabel;






        public override void InitScreen(ScreenType screnType)
        {
            base.InitScreen(screnType);


            Texture2D planetTexture = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
            Texture2D altPlanetTexture = GameContent.GameAssets.Images.NonPlayingObjects.AltPlanet;
            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;



            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            TitleSprite = new Sprite(GameContent.GameAssets.Images.Controls.Title, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .05f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .07f), Sprites.SpriteBatch);
            Sprites.Add(TitleSprite);

            planet = new Sprite(altPlanetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planet.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1515f);
            planet.Scale = new Vector2(.7f);
            Sprites.Add(planet);

            planettwo = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.8f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .75f);
            planettwo.Scale = new Vector2(1f);
            Sprites.Add(planettwo);


            SinglePlayerButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f), Sprites.SpriteBatch);

            Sprites.Add(SinglePlayerButton);

            SinglePlayerLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Singleplayer");
            SinglePlayerLabel.IsHoverable = true;
#if WINDOWS
            SinglePlayerLabel.CallKeyboardClickEvent = false;
#endif
            SinglePlayerLabel.NonHoverColor = Color.White;
            SinglePlayerLabel.HoverColor = Color.MediumAquamarine;

            SinglePlayerLabel.ParentSprite = SinglePlayerButton;

            AdditionalSprites.Add(SinglePlayerLabel);


            MultiPlayerButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);

            Sprites.Add(MultiPlayerButton);

            MultiPlayerLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Multiplayer");
            MultiPlayerLabel.IsHoverable = true;
#if WINDOWS
            MultiPlayerLabel.CallKeyboardClickEvent = false;
#endif
            MultiPlayerLabel.NonHoverColor = Color.White;
            MultiPlayerLabel.HoverColor = Color.MediumAquamarine;
            MultiPlayerLabel.ParentSprite = MultiPlayerButton;
            AdditionalSprites.Add(MultiPlayerLabel);


            BackButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);

            Sprites.Add(BackButton);

            BackLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Exit");
            BackLabel.IsHoverable = true;

#if WINDOWS
            BackLabel.CallKeyboardClickEvent = false;
#endif
            BackLabel.ParentSprite = BackButton;
            BackLabel.NonHoverColor = Color.White;
            BackLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(BackLabel);


            OptionsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f), Sprites.SpriteBatch);

            Sprites.Add(OptionsButton);

            OptionsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Options");
#if WINDOWS
            OptionsLabel.CallKeyboardClickEvent = false;
#endif
            OptionsLabel.ParentSprite = OptionsButton;
            OptionsLabel.IsHoverable = true;
            OptionsLabel.NonHoverColor = Color.White;
            OptionsLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(OptionsLabel);


            CreditsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);

            Sprites.Add(CreditsButton);


            CreditsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Credits");
            CreditsLabel.IsHoverable = true;
#if WINDOWS
            CreditsLabel.CallKeyboardClickEvent = false;
#endif
            CreditsLabel.ParentSprite = CreditsButton;
            CreditsLabel.NonHoverColor = Color.White;
            CreditsLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(CreditsLabel);

#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { { SinglePlayerLabel, OptionsLabel }, { MultiPlayerLabel, CreditsLabel }, { BackLabel, null } }, InputType.LeftJoystick);
            SinglePlayerLabel.IsSelected = true;
            AllButtons.FireTextSpritePressed = true;
#endif
            BackLabel.Pressed += new EventHandler(delegate(object src, EventArgs e) { if (!Guide.IsVisible && this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.Exit(); ButtonClick.Play(); } });
            CreditsLabel.Pressed += new EventHandler(delegate(object src, EventArgs e) { if (!Guide.IsVisible && this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.ScreenState = ScreenType.Credits; if (StateManager.Options.SFXEnabled) ButtonClick.Play(); } });
            OptionsLabel.Pressed += new EventHandler(delegate(object src, EventArgs e) { if (!Guide.IsVisible && this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.ScreenState = ScreenType.Options; if (StateManager.Options.SFXEnabled)  ButtonClick.Play(); } });
            SinglePlayerLabel.Pressed += new EventHandler(SinglePlayerLabel_Pressed);
            MultiPlayerLabel.Pressed += new EventHandler(delegate(object src, EventArgs e) { if (!Guide.IsVisible && this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.ScreenState = ScreenType.NetworkSelectScreen; if (StateManager.Options.SFXEnabled) ButtonClick.Play(); } });
        }

        void SinglePlayerLabel_Pressed(object sender, EventArgs e)
        {
            if (!Guide.IsVisible && Visible && elapsedButtonDelay > totalButtonDelay)
            {
                
                if (StateManager.Options.SFXEnabled)
                {
                    ButtonClick.Play();
                }
                if (StateManager.SelectedStorage == null)
                {
                    StorageDevice.BeginShowSelector(PlayerIndex.One, new AsyncCallback(onSelectedStorage), null);
                }
                else if (StateManager.SelectedStorage != null)
                {
                    StateManager.ScreenState = ScreenType.LevelSelect;
                }
            }
        }

        void onSelectedStorage(IAsyncResult res)
        {

            try{
                StorageDevice dev = StorageDevice.EndShowSelector(res);
                if (dev == null)
                {
                    return;
                }
                StateManager.SelectedStorage = dev;
                StateManager.ScreenState = CoreTypes.ScreenType.LevelSelect;
            }catch{};
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            planet.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1515f);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.8f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .75f);
            TitleSprite.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .05f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .07f);
            CreditsButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f);
            BackButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f);
            SinglePlayerButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f);
            MultiPlayerButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f);
            OptionsButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f);

            //to unselect options label when changing to full screens and back
            foreach (ISprite s in AdditionalSprites)
            {
                if (s.GetType() == typeof(TextSprite))
                {
                    //We can deselect
                    s.Cast<TextSprite>().IsSelected = false;
                }
            }
        }

#if XBOX
        GamePadButtonEnumerator AllButtons;
#endif
        //Preventing clickthrus
        TimeSpan elapsedButtonDelay = TimeSpan.Zero;
        TimeSpan totalButtonDelay = TimeSpan.FromMilliseconds(250);

        public override void Update(GameTime gameTime)
        {
            elapsedButtonDelay += gameTime.ElapsedGameTime;
#if XBOX

            AllButtons.Update(gameTime);

            currentGamePad = GamePad.GetState(PlayerIndex.One);

            lastGamePad = currentGamePad;

#endif
            base.Update(gameTime);
        }

#if XBOX
        GamePadState currentGamePad;
        GamePadState lastGamePad = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, Buttons.A);
#endif

    }
}
