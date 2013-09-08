using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace PGCGame.Screens.Multiplayer
{
    public class AvailableSessionsScreen : BaseScreen
    {
        public AvailableSessionsScreen(SpriteBatch sb)
            : base(sb, Color.Black)
        {
            StateManager.Options.ScreenResolutionChanged += new EventHandler<ViewportEventArgs>(Options_ScreenResolutionChanged);
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
        }

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (StateManager.ScreenState == this.ScreenType)
            {
                netSessionRegrab();
            }
        }

        void Options_ScreenResolutionChanged(object sender, ViewportEventArgs e)
        {
            if (title != null)
            {
                title.Y = 5;
                title.X = title.GetCenterPosition(Graphics.Viewport).X;
            }
            if (reloadButton != null)
            {
                reloadButton.X = reloadButton.GetCenterPosition(Graphics.Viewport).X;
            }
        }

        TextSprite title;
        TextSprite reload;
        Sprite reloadButton;
        Sprite BackButton;
        TextSprite BackLabel;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            title = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.BoldText, "Available "+(StateManager.NetworkData.SessionType == NetworkSessionType.SystemLink ? "LAN" : "LIVE")+" Sectors");
            title.Color = Color.White;
            title.Y = 5;
            title.X = title.GetCenterPosition(Graphics.Viewport).X;

            reloadButton = new Sprite(GameContent.Assets.Images.Controls.Button, new Vector2(0, title.Y + title.Font.LineSpacing + 10), Sprites.SpriteBatch);
            reloadButton.X = reloadButton.GetCenterPosition(Graphics.Viewport).X;

            reload = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.NormalText, "Refresh");
            reload.ParentSprite = reloadButton;
            reload.NonHoverColor = Color.White;
            reload.HoverColor = Color.MediumAquamarine;
            
            reload.IsHoverable = true;
            reload.Pressed += new EventHandler(reload_Pressed);

            BackButton = new Sprite(GameContent.Assets.Images.Controls.Button, new Vector2(20, Graphics.Viewport.Height), Sprites.SpriteBatch);
            BackButton.Y -= BackButton.Height + 20;
            BackLabel = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.NormalText, "Back", Color.White) { IsHoverable = true, HoverColor = Color.MediumAquamarine, NonHoverColor = Color.White };
            BackLabel.ParentSprite = BackButton;
            BackLabel.Pressed += new EventHandler(BackLabel_Pressed);
            BackLabel.Visible = true;
            
            Sprites.Add(BackButton);

            AdditionalSprites.Add(title);
            AdditionalSprites.Add(reload);
            Sprites.Add(reloadButton);
        }

        void BackLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.GoBack();
        }

        void FinishLanSectorSearch(object res)
        {
            IAsyncResult getMySectors = res as IAsyncResult;
            if (StateManager.NetworkData.CurrentSession != null)
            {
                StateManager.NetworkData.LeaveSession();
                StateManager.NetworkData.CurrentSession = null;
            }
            if (StateManager.NetworkData.AvailableSessions != null)
            {
                StateManager.NetworkData.AvailableSessions.Dispose();
                StateManager.NetworkData.AvailableSessions = null;
            }

            StateManager.NetworkData.AvailableSessions = NetworkSession.EndFind(getMySectors);
        }

        void reload_Pressed(object sender, EventArgs e)
        {
            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }
            if (Gamer.SignedInGamers.Count == 0 && !Guide.IsVisible)
            {
                Guide.ShowSignIn(1, false);
                return;
            }
            else if (Gamer.SignedInGamers.Count == 0)
            {
                return;
            }
            LoadingScreen lScr = StateManager.AllScreens[ScreenType.LoadingScreen.ToString()] as LoadingScreen;
            lScr.Reset();
            lScr.UserCallback = new PGCGame.CoreTypes.Delegates.AsyncHandlerMethod(FinishLanSectorSearch);
            lScr.LoadingText = "Searching for\n" + (StateManager.NetworkData.SessionType == NetworkSessionType.SystemLink ? "LAN" : "LIVE") + " sectors...";
            lScr.ScreenFinished += new EventHandler(delegate(object evSender, EventArgs ea) { StateManager.ScreenState = CoreTypes.ScreenType.NetworkSessionsScreen; });
            NetworkSession.BeginFind(StateManager.NetworkData.SessionType, 1, null, lScr.Callback, null);
            StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;
        }

        private void netSessionRegrab()
        {
            AdditionalSprites.Clear();
            AdditionalSprites.Add(title);
            AdditionalSprites.Add(reload);
            AdditionalSprites.Add(BackLabel);

            AvailableNetworkSessionDisplayTextSprite prev = null;
            foreach (AvailableNetworkSession ans in StateManager.NetworkData.AvailableSessions)
            {
                AvailableNetworkSessionDisplayTextSprite curr = new AvailableNetworkSessionDisplayTextSprite(Sprites.SpriteBatch, prev == null ? reloadButton.Y + reloadButton.Height : prev.Y, ans);
                curr.Pressed += new EventHandler(curr_Pressed);
                AdditionalSprites.Add(curr);

                prev = curr;
            }
        }

        private void FinishJoin(object res)
        {
            IAsyncResult r = res as IAsyncResult;

            StateManager.NetworkData.CurrentSession = NetworkSession.EndJoin(r);
            StateManager.NetworkData.RegisterNetworkSession();

        }

        void curr_Pressed(object evsender, EventArgs e)
        {
            AvailableNetworkSessionDisplayTextSprite sender = evsender as AvailableNetworkSessionDisplayTextSprite;
            if (StateManager.NetworkData.IsMultiplayer)
            {
                StateManager.NetworkData.LeaveSession();
            }
            LoadingScreen lScr = StateManager.AllScreens[ScreenType.LoadingScreen.ToString()] as LoadingScreen;
            lScr.Reset();
            lScr.UserCallbackStartsTask = false;
            lScr.UserCallback = new PGCGame.CoreTypes.Delegates.AsyncHandlerMethod(FinishJoin);
            lScr.LoadingText = "Joining\nLAN sector...";
            lScr.ScreenFinished += new EventHandler(delegate(object evSender, EventArgs ea) {
                StateManager.ScreenState = CoreTypes.ScreenType.MultiPlayerShipSelect; 
            });
            NetworkSession.BeginJoin(sender.Session, lScr.Callback, null);
            StateManager.NetworkData.SessionMode = sender.SessionType;
            StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;
        }
    }
}
