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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.GamerServices;

namespace PGCGame.Screens.Multiplayer
{
    public class NetworkSelectScreen : BaseScreen
    {
        public NetworkSelectScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
            ButtonClick = GameContent.GameAssets.Sound[SoundEffectType.ButtonPressed];
        }
        TimeSpan elapsedButtonDelay = TimeSpan.Zero;

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                elapsedButtonDelay = TimeSpan.Zero;
            }
            if ((Visible || StateManager.ScreenState == CoreTypes.ScreenType.MainMenu) && (StateManager.NetworkData.AvailableSessions != null))
            {
                if (!StateManager.NetworkData.AvailableSessions.IsDisposed)
                {
                    StateManager.NetworkData.AvailableSessions.Dispose();
                }
                StateManager.NetworkData.AvailableSessions = null;
                if (StateManager.NetworkData.IsMultiplayer)
                {
                    StateManager.NetworkData.LeaveSession();
                }
            }
        }

        Sprite LANButton;
        TextSprite LANLabel;

        Sprite HostButton;
        TextSprite HostLabel;

        Sprite BackButton;
        TextSprite BackLabel;

        public override void InitScreen(ScreenType screnType)
        {
            base.InitScreen(screnType);

            StateManager.Options.MusicStateChanged += new EventHandler(Options_MusicStateChanged);

            Texture2D planetTexture = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
            Texture2D altPlanetTexture = GameContent.GameAssets.Images.NonPlayingObjects.AltPlanet;
            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;



            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            BackButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            LANButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            HostButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .06f), Sprites.SpriteBatch);

            Sprites.Add(BackButton);

            BackLabel = new TextSprite(Sprites.SpriteBatch, Vector2.One * 10, SegoeUIMono, "Back");
            BackLabel.IsHoverable = true;
#if WINDOWS
            BackLabel.CallKeyboardClickEvent = false;
#endif
            BackLabel.ParentSprite = BackButton;
            BackLabel.NonHoverColor = Color.White;
            BackLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(BackLabel);

            Sprites.Add(LANButton);
            LANButton.Scale = new Vector2(1.5f, 1);
            LANLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Scan for LAN sectors");
            LANLabel.IsHoverable = true;
            LANLabel.ParentSprite = LANButton;
#if WINDOWS
            LANLabel.CallKeyboardClickEvent = false;
#endif
            LANLabel.NonHoverColor = Color.White;
            
            LANLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(LANLabel);

            Sprites.Add(HostButton);
            HostButton.Scale = new Vector2(1.1f, 1);
            HostLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Host LAN sector");
            HostLabel.IsHoverable = true;
#if WINDOWS
            HostLabel.CallKeyboardClickEvent = false;
#endif
            HostLabel.ParentSprite = HostButton;
            
            HostLabel.NonHoverColor = Color.White;
            HostLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(HostLabel);

#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { { HostLabel, null }, {BackLabel, LANLabel} }, InputType.LeftJoystick);
            AllButtons.FireTextSpritePressed = true;
#endif
            LANLabel.Pressed += new EventHandler(LANLabel_Pressed);
            HostLabel.Pressed += StateManager.ButtonSFXHelper;
            HostLabel.Pressed += new EventHandler(HostLabel_Pressed);

            BackLabel.Pressed += new EventHandler(BackLabel_Pressed);
        }

        void BackLabel_Pressed(object sender, EventArgs e)
        {
            if (this.Visible && elapsedButtonDelay > totalButtonDelay)
            {
                if (StateManager.NetworkData.AvailableSessions != null)
                {
                    if (!StateManager.NetworkData.AvailableSessions.IsDisposed)
                    {
                        StateManager.NetworkData.AvailableSessions.Dispose();
                    }
                    StateManager.NetworkData.AvailableSessions = null;
                }
                if (StateManager.NetworkData.CurrentSession != null)
                {
                    if (!StateManager.NetworkData.CurrentSession.IsDisposed)
                    {
                        StateManager.NetworkData.CurrentSession.Dispose();
                    }
                    StateManager.NetworkData.CurrentSession = null;
                }
                StateManager.ScreenState = CoreTypes.ScreenType.MainMenu;
                if (StateManager.Options.SFXEnabled) { ButtonClick.Play(); }
            }
        }

        void HostLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.ScreenState = CoreTypes.ScreenType.NetworkMatchSelection;
        }





        void FinishLanSectorSearch(object ar)
        {
            IAsyncResult getMySectors = ar as IAsyncResult;
            if (StateManager.NetworkData.AvailableSessions != null)
            {
                if (!StateManager.NetworkData.AvailableSessions.IsDisposed)
                {
                    StateManager.NetworkData.AvailableSessions.Dispose();
                }
                StateManager.NetworkData.AvailableSessions = null;
            }
            StateManager.NetworkData.AvailableSessions = NetworkSession.EndFind(getMySectors);
        }

        void LANLabel_Pressed(object sender, EventArgs e)
        {
            if (Gamer.SignedInGamers.Count == 0 && !Guide.IsVisible)
            {
                Guide.ShowSignIn(1, false);
                return;
            }
            else if (Gamer.SignedInGamers.Count == 0)
            {
                return;
            }
            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }
            LoadingScreen lScr = StateManager.GetScreen<LoadingScreen>(CoreTypes.ScreenType.LoadingScreen);
            lScr.Reset();
            lScr.UserCallback = new PGCGame.CoreTypes.Delegates.AsyncHandlerMethod(FinishLanSectorSearch);
            lScr.LoadingText = "Searching for\nLAN sectors...";
            lScr.ScreenFinished += new EventHandler(lScr_ScreenFinished);
            StateManager.NetworkData.LeaveSession();
            NetworkSession.BeginFind(NetworkSessionType.SystemLink, new SignedInGamer[] {Gamer.SignedInGamers[PlayerIndex.One]}, null, lScr.Callback, null);
            StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;
        }

        void lScr_ScreenFinished(object sender, EventArgs e)
        {
            StateManager.ScreenState = CoreTypes.ScreenType.NetworkSessionsScreen;
        }

        void Options_MusicStateChanged(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {

            BackButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f);


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
