using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib.XNA.SpriteLib;
using Glib.XNA;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace PGCGame.Screens.Multiplayer
{
    public class NetworkMatchTypeSelectionScreen : BaseScreen
    {
        public NetworkMatchTypeSelectionScreen(SpriteBatch sb)
            : base(sb, Color.Black)
        {

        }

        private Sprite CoopButton;
        private TextSprite CoopLabel;

        private Sprite DeathMatchButton;
        private TextSprite DeathMatchLabel;

        private TextSprite TitleLabel;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            TitleLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(0, 5), GameContent.GameAssets.Fonts.BoldText, "Select Match Type", Color.White);
            TitleLabel.X = TitleLabel.GetCenterPosition(Graphics.Viewport).X;
            AdditionalSprites.Add(TitleLabel);

            CoopButton = new Sprite(GameContent.GameAssets.Images.Controls.Button, new Vector2(0, 10 + TitleLabel.Height), Sprites.SpriteBatch);
            CoopButton.X = CoopButton.GetCenterPosition(Graphics.Viewport).X;

            CoopLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "Co-op match", Color.White);
            CoopLabel.HoverColor = Color.MediumAquamarine;
            CoopLabel.NonHoverColor = Color.White;
            CoopLabel.IsHoverable = true;
            CoopLabel.ParentSprite = CoopButton;
            CoopLabel.Pressed += new EventHandler(CoopLabel_Pressed);

            Sprites.Add(CoopButton);
            AdditionalSprites.Add(CoopLabel);


            DeathMatchButton = new Sprite(GameContent.GameAssets.Images.Controls.Button, new Vector2(0, 20 + CoopLabel.Height + CoopLabel.Y), Sprites.SpriteBatch);
            DeathMatchButton.X = CoopButton.GetCenterPosition(Graphics.Viewport).X;

            DeathMatchLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "Deathmatch", Color.White);
            DeathMatchLabel.HoverColor = Color.MediumAquamarine;
            DeathMatchLabel.NonHoverColor = Color.White;
            DeathMatchLabel.IsHoverable = true;
            DeathMatchLabel.ParentSprite = DeathMatchButton;
            DeathMatchLabel.Pressed += new EventHandler(DeathMatchLabel_Pressed);

            Sprites.Add(DeathMatchButton);
            AdditionalSprites.Add(DeathMatchLabel);
        }

        

        void DeathMatchLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.NetworkData.SessionMode = MultiplayerSessionType.Deathmatch;
            CreateMatch();
        }

        void CoopLabel_Pressed(object sender, EventArgs e)
        {
            //TODO: Implement
        }

        void hosting_finish(object sender, EventArgs r)
        {
            LobbyScreen lobby = StateManager.AllScreens[ScreenType.NetworkLobbyScreen.ToString()] as LobbyScreen;
            lobby.InitScreen();
            StateManager.ScreenState = CoreTypes.ScreenType.NetworkLobbyScreen;
        }

        void FinishLanSectorHost(IAsyncResult getMySectors)
        {
            StateManager.NetworkData.CurrentSession = NetworkSession.EndCreate(getMySectors);
            StateManager.NetworkData.CurrentSession.AllowHostMigration = false;
            StateManager.NetworkData.CurrentSession.AllowJoinInProgress = false;
            StateManager.NetworkData.RegisterNetworkSession();
        }
        
        public void CreateMatch()
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
            LoadingScreen lScr = StateManager.AllScreens[ScreenType.LoadingScreen.ToString()] as LoadingScreen;
            lScr.Reset();
            lScr.UserCallback = new AsyncCallback(FinishLanSectorHost);
            lScr.LoadingText = "Hosting\nLAN sector...";
            lScr.ScreenFinished += new EventHandler(hosting_finish);
            //lScr.ScreenFinished += new EventHandler(lScr_ScreenFinished);
            NetworkSessionProperties netSession = new NetworkSessionProperties();
            netSession[(int)NetworkSessionPropertyType.SessionType] = (int)StateManager.NetworkData.SessionMode;
            NetworkSession.BeginCreate(NetworkSessionType.SystemLink, 1, 8, 0, netSession, lScr.Callback, null);
            StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;
        }
    }
}
