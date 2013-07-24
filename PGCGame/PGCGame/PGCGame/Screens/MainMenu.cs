using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;

using PGCGame.CoreTypes;
using Glib.XNA.InputLib;
using Microsoft.Xna.Framework.Media;

namespace PGCGame.Screens
{
    public class MainMenu : BaseScreen
    {

        public MainMenu(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
#if WINDOWS
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
#endif
        }

#if WINDOWS
        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if(Visible)
            {
                elapsedButtonDelay = TimeSpan.Zero;
            }
        }
#endif

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

        Song _gameSong;




        public override void InitScreen(ScreenType screnType)
        {
            base.InitScreen(screnType);

            _gameSong = GameContent.GameAssets.Music[ScreenMusic.Level1];
          
            StateManager.Options.MusicStateChanged += new EventHandler(Options_MusicStateChanged);
            StateManager.ScreenStateChanged += new EventHandler(delegate(object src, EventArgs arg)
            {
                if (Visible)
                {
                    if (StateManager.Options.MusicEnabled)
                    {
                        if(MediaPlayer.State != MediaState.Playing)
                        {
                            MediaPlayer.Play(_gameSong);
                        }
                    }
                    else
                    {
                        MediaPlayer.Stop();
                    }
                }
            });
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
            SinglePlayerLabel.CallKeyboardClickEvent = false;
            SinglePlayerLabel.NonHoverColor = Color.White;
            SinglePlayerLabel.HoverColor = Color.MediumAquamarine;

            SinglePlayerLabel.ParentSprite = SinglePlayerButton;

            AdditionalSprites.Add(SinglePlayerLabel);


            MultiPlayerButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);

            Sprites.Add(MultiPlayerButton);

            MultiPlayerLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Multiplayer");
            MultiPlayerLabel.IsHoverable = true;
            MultiPlayerLabel.CallKeyboardClickEvent = false;
            MultiPlayerLabel.NonHoverColor = Color.White;
            MultiPlayerLabel.HoverColor = Color.MediumAquamarine;
            MultiPlayerLabel.ParentSprite = MultiPlayerButton;
            AdditionalSprites.Add(MultiPlayerLabel);


            BackButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);

            Sprites.Add(BackButton);

            BackLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Exit");
            BackLabel.IsHoverable = true;
            BackLabel.CallKeyboardClickEvent = false;
            BackLabel.ParentSprite = BackButton;
            BackLabel.NonHoverColor = Color.White;
            BackLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(BackLabel);


            OptionsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f), Sprites.SpriteBatch);

            Sprites.Add(OptionsButton);

            OptionsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Options");
            OptionsLabel.CallKeyboardClickEvent = false;
            OptionsLabel.ParentSprite = OptionsButton;
            OptionsLabel.IsHoverable = true;
            OptionsLabel.NonHoverColor = Color.White;
            OptionsLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(OptionsLabel);


            CreditsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);

            Sprites.Add(CreditsButton);


            CreditsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Credits");
            CreditsLabel.IsHoverable = true;
            CreditsLabel.CallKeyboardClickEvent = false;
            CreditsLabel.ParentSprite = CreditsButton;
            CreditsLabel.NonHoverColor = Color.White;
            CreditsLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(CreditsLabel);

#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { { SinglePlayerLabel, OptionsLabel }, { MultiPlayerLabel, CreditsLabel }, { BackLabel, null } }, InputType.LeftJoystick);
            AllButtons.ButtonPress += new EventHandler(AllButtons_ButtonPress);
            SinglePlayerLabel.IsSelected = true;
#elif WINDOWS
            BackLabel.Clicked += new EventHandler(delegate(object src, EventArgs e) { if (this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.Exit(); } });
            CreditsLabel.Clicked += new EventHandler(delegate(object src, EventArgs e) { if (this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.ScreenState = ScreenType.Credits; } });
            OptionsLabel.Clicked += new EventHandler(delegate(object src, EventArgs e) { if (this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.ScreenState = ScreenType.Options; } });
            SinglePlayerLabel.Clicked += new EventHandler(delegate(object src, EventArgs e) { if (this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.ScreenState = ScreenType.ShipSelect; } });
            MultiPlayerLabel.Clicked += new EventHandler(delegate(object src, EventArgs e) { if (this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.ScreenState = ScreenType.NetworkSelectScreen; } });
#endif
        }

#if XBOX
        void AllButtons_ButtonPress(object sender, EventArgs e)
        {
            if (BackLabel.IsSelected)
            {
                StateManager.Exit();
            }
            else if (SinglePlayerLabel.IsSelected)
            {
                StateManager.ScreenState = ScreenType.ShipSelect;
            }
            else if (MultiPlayerLabel.IsSelected)
            {
                //TODO: Multiplayer
            }
            else if (OptionsLabel.IsSelected)
            {
                StateManager.ScreenState = ScreenType.Options;
            }
            else if (CreditsLabel.IsSelected)
            {
                StateManager.ScreenState = ScreenType.Credits;
            }
        }
#endif

        void Options_MusicStateChanged(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
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
#elif WINDOWS
        //Preventing clickthrus
        TimeSpan elapsedButtonDelay = TimeSpan.Zero;
        TimeSpan totalButtonDelay = TimeSpan.FromMilliseconds(250);
#endif

        public override void Update(GameTime gameTime)
        {
#if WINDOWS
            elapsedButtonDelay += gameTime.ElapsedGameTime;
#elif XBOX 
            
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
