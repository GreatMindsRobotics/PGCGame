﻿using System;
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
        public override MusicBehaviour Music
        {
            get { return MusicBehaviour.NoMusic; }
        }

        public NetworkSelectScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
            ButtonClick = GameContent.Assets.Sound[SoundEffectType.ButtonPressed];
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

        Sprite HostLIVEButton;
        TextSprite HostLIVELabel;

        Sprite ScanLIVEButton;
        TextSprite ScanLIVELabel;

        Sprite LANButton;
        TextSprite LANLabel;

        Sprite HostLANButton;
        TextSprite HostLANLabel;

        Sprite BackButton;
        TextSprite BackLabel;

        public override void InitScreen(ScreenType screnType)
        {
            base.InitScreen(screnType);

            Texture2D planetTexture = GameContent.Assets.Images.NonPlayingObjects.Planet;
            Texture2D altPlanetTexture = GameContent.Assets.Images.NonPlayingObjects.AltPlanet;
            Texture2D buttonImage = GameContent.Assets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.Assets.Fonts.NormalText;

            StateManager.Options.ScreenResolutionChanged += new EventHandler<ViewportEventArgs>(Options_ScreenResolutionChanged);

            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            BackButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            //LANButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            HostLANButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .06f), Sprites.SpriteBatch);
            LANButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .6f, HostLANButton.Y), Sprites.SpriteBatch);

            HostLIVEButton = new Sprite(buttonImage, new Vector2(HostLANButton.X, HostLANButton.Y+HostLANButton.Height+SegoeUIMono.LineSpacing), Sprites.SpriteBatch);
            HostLIVEButton.Scale = new Vector2(1.1775f, 1);
            Sprites.Add(HostLIVEButton);

            HostLIVELabel = new TextSprite(Sprites.SpriteBatch, SegoeUIMono, "Host LIVE Sector");
            HostLIVELabel.ParentSprite = HostLIVEButton;
            HostLIVELabel.IsHoverable = true;
            HostLIVELabel.Pressed += new EventHandler(HostLIVELabel_Pressed);
            HostLIVELabel.HoverColor = Color.MediumAquamarine;
            HostLIVELabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(HostLIVELabel);

            

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

            Sprites.Add(HostLANButton);
            HostLANButton.Scale = new Vector2(1.1f, 1);
            HostLANLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Host LAN sector");
            HostLANLabel.IsHoverable = true;
#if WINDOWS
            HostLANLabel.CallKeyboardClickEvent = false;
#endif
            HostLANLabel.ParentSprite = HostLANButton;
            
            HostLANLabel.NonHoverColor = Color.White;
            HostLANLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(HostLANLabel);

            ScanLIVEButton = new Sprite(buttonImage, new Vector2(LANButton.X, HostLIVEButton.Y), Sprites.SpriteBatch);
            ScanLIVEButton.Scale = new Vector2(1.5f, 1);
            Sprites.Add(ScanLIVEButton);

            ScanLIVELabel = new TextSprite(Sprites.SpriteBatch, SegoeUIMono, "Scan for LIVE Sectors", Color.White);
            ScanLIVELabel.ParentSprite = ScanLIVEButton;
            ScanLIVELabel.IsHoverable = true;
            ScanLIVELabel.Pressed += new EventHandler(ScanLIVELabel_Pressed);
            ScanLIVELabel.HoverColor = Color.MediumAquamarine;
            ScanLIVELabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(ScanLIVELabel);

#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { { HostLANLabel, LANLabel }, {HostLIVELabel, ScanLIVELabel}, {BackLabel, null} }, InputType.LeftJoystick);
            AllButtons.FireTextSpritePressed = true;
#endif
            LANLabel.Pressed += new EventHandler(LANLabel_Pressed);
            HostLANLabel.Pressed += StateManager.ButtonSFXHelper;
            HostLANLabel.Pressed += new EventHandler(HostLabel_Pressed);

            BackLabel.Pressed += new EventHandler(BackLabel_Pressed);
        }

        void ScanLIVELabel_Pressed(object sender, EventArgs e)
        {
            if (Gamer.SignedInGamers.Count == 0 && !Guide.IsVisible)
            {
                Guide.ShowSignIn(1, true);
                return;
            }
            else if (Gamer.SignedInGamers.Count == 0)
            {
                return;
            }
            StateManager.NetworkData.SessionType = NetworkSessionType.PlayerMatch;
            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }
            if (!Gamer.SignedInGamers[PlayerIndex.One].Privileges.AllowOnlineSessions)
            {
                if (!Guide.IsVisible)
                {
                    Guide.BeginShowMessageBox("Insufficient Permissions", "Your gamer profile does not allow you to play online sessions.", new String[] { "OK" }, 0, MessageBoxIcon.Error, null, null);
                }
                return;
            }
            LoadingScreen lScr = StateManager.GetScreen<LoadingScreen>(CoreTypes.ScreenType.LoadingScreen);
            lScr.Reset();
            lScr.UserCallback = new PGCGame.CoreTypes.Delegates.AsyncHandlerMethod(FinishLanSectorSearch);
            lScr.LoadingText = "Searching for\nLIVE sectors...";
            lScr.ScreenFinished += new EventHandler(lScr_ScreenFinished);
            StateManager.NetworkData.LeaveSession();
            NetworkSession.BeginFind(StateManager.NetworkData.SessionType, 1, null, lScr.Callback, null);
            StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;
        }

        void HostLIVELabel_Pressed(object sender, EventArgs e)
        {
            if (Gamer.SignedInGamers.Count == 0 && !Guide.IsVisible)
            {
                Guide.ShowSignIn(1, true);
                return;
            }
            else if (Gamer.SignedInGamers.Count == 0)
            {
                return;
            }
            if (!Gamer.SignedInGamers[PlayerIndex.One].Privileges.AllowOnlineSessions)
            {

                if (!Guide.IsVisible)
                {
                    Guide.BeginShowMessageBox("Insufficient Permissions", "Your gamer profile does not allow you to play online sessions.", new String[] { "OK" }, 0, MessageBoxIcon.Error, null, null);
                }
                return;
            }
            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }
            StateManager.NetworkData.SessionType = NetworkSessionType.PlayerMatch;
            StateManager.ScreenState = CoreTypes.ScreenType.NetworkMatchSelection;
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
                if (StateManager.NetworkData.IsMultiplayer)
                {
                    StateManager.NetworkData.LeaveSession();
                }
                StateManager.ScreenState = CoreTypes.ScreenType.MainMenu;
                if (StateManager.Options.SFXEnabled) { ButtonClick.Play(); }
            }
        }

        void HostLabel_Pressed(object sender, EventArgs e)
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
            StateManager.NetworkData.SessionType = NetworkSessionType.SystemLink;
            StateManager.ScreenState = CoreTypes.ScreenType.NetworkMatchSelection;
        }

        void FinishLanSectorSearch(object ar)
        {
            isError = false;
            IAsyncResult getMySectors = ar as IAsyncResult;
            if (StateManager.NetworkData.AvailableSessions != null)
            {
                if (!StateManager.NetworkData.AvailableSessions.IsDisposed)
                {
                    StateManager.NetworkData.AvailableSessions.Dispose();
                }
                StateManager.NetworkData.AvailableSessions = null;
            }

            try
            {
                StateManager.NetworkData.AvailableSessions = NetworkSession.EndFind(getMySectors);
            }
            catch (GamerPrivilegeException)
            {
                isError = true;
                if (!Guide.IsVisible)
                {
                    string statement = "";
#if WINDOWS
                    statement = "A GFWL Silver (or greater) subscription is required to join LIVE sessions. Please ensure that the signed in account has a Games for Windows LIVE silver or greater subscription.";
#elif XBOX
                    statement = "An Xbox LIVE account is required to join LIVE sessions. Please ensure that the signed in account has an Xbox LIVE gold subscription.";
#endif
                    Guide.BeginShowMessageBox("LIVE Account Required", statement, new String[] { "OK" }, 0, MessageBoxIcon.Error, null, null);
                }
                return;
            }
            
        }

        bool isError = false;

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
            StateManager.NetworkData.SessionType = NetworkSessionType.SystemLink;
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
            NetworkSession.BeginFind(StateManager.NetworkData.SessionType, 1, null, lScr.Callback, null);
            StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;
        }

        void lScr_ScreenFinished(object sender, EventArgs e)
        {
            StateManager.ScreenState = isError ? ScreenType.NetworkSelectScreen : CoreTypes.ScreenType.NetworkSessionsScreen;
        }

        void Options_ScreenResolutionChanged(object sender, ViewportEventArgs e)
        {
            BackButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f);

            //to unselect options label when changing to full screens and back
            foreach (ISprite s in AdditionalSprites)
            {
                if (s.GetType() == typeof(TextSprite))
                {
                    //We can deselect
                    (s as TextSprite).IsSelected = false;
                }
            }
        }

        TimeSpan totalButtonDelay = TimeSpan.FromMilliseconds(250);

        public override void Update(GameTime gameTime)
        {
            elapsedButtonDelay += gameTime.ElapsedGameTime;

            base.Update(gameTime);
        }

    }
}
