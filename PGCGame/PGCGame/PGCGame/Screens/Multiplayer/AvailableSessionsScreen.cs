﻿using System;
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
            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
        }

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (StateManager.ScreenState == this.ScreenType)
            {
                netSessionRegrab();
            }
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
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
            title = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.BoldText, "Available LAN Sectors");
            title.Color = Color.White;
            title.Y = 5;
            title.X = title.GetCenterPosition(Graphics.Viewport).X;

            reloadButton = new Sprite(GameContent.GameAssets.Images.Controls.Button, new Vector2(0, title.Y + title.Font.LineSpacing + 10), Sprites.SpriteBatch);
            reloadButton.X = reloadButton.GetCenterPosition(Graphics.Viewport).X;

            reload = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "Refresh");
            reload.ParentSprite = reloadButton;
            reload.NonHoverColor = Color.White;
            reload.HoverColor = Color.MediumAquamarine;
            
            reload.IsHoverable = true;
            reload.Pressed += new EventHandler(reload_Pressed);

            BackButton = new Sprite(GameContent.GameAssets.Images.Controls.Button, new Vector2(20, Graphics.Viewport.Height), Sprites.SpriteBatch);
            BackButton.Y -= BackButton.Height + 20;
            BackLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "Back", Color.White) { IsHoverable = true, HoverColor = Color.MediumAquamarine, NonHoverColor = Color.White };
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

        void FinishLanSectorSearch(IAsyncResult getMySectors)
        {
            if (StateManager.NetworkData.AvailableSessions != null)
            {
                StateManager.NetworkData.AvailableSessions.Dispose();
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
            lScr.UserCallback = new AsyncCallback(FinishLanSectorSearch);
            lScr.LoadingText = "Searching for\nLAN sectors...";
            lScr.ScreenFinished += new EventHandler(delegate(object evSender, EventArgs ea) { StateManager.ScreenState = CoreTypes.ScreenType.NetworkSessionsScreen; });
            NetworkSession.BeginFind(NetworkSessionType.SystemLink, Gamer.SignedInGamers, null, lScr.Callback, null);
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

        void FinishJoin(IAsyncResult r)
        {
            StateManager.NetworkData.CurrentSession = NetworkSession.EndJoin(r);
            StateManager.NetworkData.RegisterNetworkSession();
        }

        void curr_Pressed(object evsender, EventArgs e)
        {
            AvailableNetworkSessionDisplayTextSprite sender = evsender as AvailableNetworkSessionDisplayTextSprite;
            LoadingScreen lScr = StateManager.AllScreens[ScreenType.LoadingScreen.ToString()] as LoadingScreen;
            lScr.Reset();
            lScr.UserCallbackStartsTask = false;
            lScr.UserCallback = new AsyncCallback(FinishJoin);
            lScr.LoadingText = "Joining\nLAN sector...";
            lScr.ScreenFinished += new EventHandler(delegate(object evSender, EventArgs ea) {
                StateManager.ScreenState = CoreTypes.ScreenType.NetworkLobbyScreen;
                (StateManager.AllScreens[ScreenType.NetworkLobbyScreen.ToString()] as LobbyScreen).InitScreen(); 
            });
            NetworkSession.BeginJoin(sender.Session, lScr.Callback, null);
            StateManager.NetworkData.SessionMode = sender.SessionType;
            StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;
        }
    }
}
