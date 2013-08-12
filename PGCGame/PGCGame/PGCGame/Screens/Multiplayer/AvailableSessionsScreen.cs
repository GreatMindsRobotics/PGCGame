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
            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);
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

            AdditionalSprites.Add(title);
            AdditionalSprites.Add(reload);
            Sprites.Add(reloadButton);
        }

        void FinishLanSectorSearch(IAsyncResult getMySectors)
        {
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
            lScr.ScreenFinished += new EventHandler(delegate(object evSender, EventArgs ea) { StateManager.ScreenState = CoreTypes.ScreenType.NetworkSessionsScreen; this.InitializeNetSessions(); });
            NetworkSession.BeginFind(NetworkSessionType.SystemLink, 1, null, lScr.Callback, null);
            StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;
        }

        public void InitializeNetSessions()
        {
            AdditionalSprites.Clear();
            AdditionalSprites.Add(title);
            AdditionalSprites.Add(reload);

            AvailableNetworkSessionDisplayTextSprite prev = null;
            foreach (AvailableNetworkSession ans in StateManager.NetworkData.AvailableSessions)
            {
                AvailableNetworkSessionDisplayTextSprite curr = new AvailableNetworkSessionDisplayTextSprite(Sprites.SpriteBatch, prev == null ? reloadButton.Y + reloadButton.Height : prev.Y, ans);
                curr.Pressed += new EventHandler(curr_Pressed);
                AdditionalSprites.Add(curr);

                prev = curr;
            }
        }

        void curr_Pressed(object evsender, EventArgs e)
        {
            AvailableNetworkSessionDisplayTextSprite sender = evsender as AvailableNetworkSessionDisplayTextSprite;
        }
    }
}
