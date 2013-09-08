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
using Glib.XNA.InputLib;

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

        private Sprite BackButton;
        private TextSprite BackLabel;

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
            CoopLabel.Pressed += StateManager.ButtonSFXHelper;
            CoopLabel.Pressed += new EventHandler(CoopLabel_Pressed);

            Sprites.Add(CoopButton);
            AdditionalSprites.Add(CoopLabel);


            DeathMatchButton = new Sprite(GameContent.GameAssets.Images.Controls.Button, new Vector2(0, 20 + CoopLabel.Height + CoopLabel.Y), Sprites.SpriteBatch);
            DeathMatchButton.X = CoopButton.GetCenterPosition(Graphics.Viewport).X;

            DeathMatchLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "LMS", Color.White);
            DeathMatchLabel.HoverColor = Color.MediumAquamarine;
            DeathMatchLabel.NonHoverColor = Color.White;
            DeathMatchLabel.IsHoverable = true;
            DeathMatchLabel.ParentSprite = DeathMatchButton;
            DeathMatchLabel.Pressed += StateManager.ButtonSFXHelper;
            DeathMatchLabel.Pressed += new EventHandler(DeathMatchLabel_Pressed);

            Sprites.Add(DeathMatchButton);
            AdditionalSprites.Add(DeathMatchLabel);


            BackButton = new Sprite(GameContent.GameAssets.Images.Controls.Button, new Vector2(0, 0), Sprites.SpriteBatch);
            BackButton.X = BackButton.GetCenterPosition(Graphics.Viewport).X;
            BackButton.Y = StateManager.GraphicsManager.GraphicsDevice.Viewport.Height - 50 - BackButton.Height;

            BackLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "Back", Color.White);
            BackLabel.HoverColor = Color.MediumAquamarine;
            BackLabel.NonHoverColor = Color.White;
            BackLabel.IsHoverable = true;
            BackLabel.ParentSprite = BackButton;
            BackLabel.Pressed += StateManager.ButtonSFXHelper;
            BackLabel.Pressed += new EventHandler(BackLabel_Pressed);

            Sprites.Add(BackButton);
            AdditionalSprites.Add(BackLabel);

#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { { CoopLabel }, { DeathMatchLabel }, { BackLabel } }, InputType.LeftJoystick);
            AllButtons.FireTextSpritePressed = true;
#endif
        }

        void BackLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.GoBack();
        }

        void DeathMatchLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.NetworkData.SessionMode = MultiplayerSessionType.LMS;
            CreateMatch();
        }

        void CoopLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.NetworkData.SessionMode = MultiplayerSessionType.Coop;
            CreateMatch();
        }

        void hosting_finish(object sender, EventArgs r)
        {
            StateManager.ScreenState = isErroredHost ? ScreenType.NetworkSelectScreen : CoreTypes.ScreenType.MultiPlayerShipSelect;
        }

        bool isErroredHost = false;

        void FinishLanSectorHost(object arg)
        {
            isErroredHost = false;
            IAsyncResult getMySectors = arg as IAsyncResult;
            try
            {
                StateManager.NetworkData.CurrentSession = NetworkSession.EndCreate(getMySectors);
            }
            catch (GamerPrivilegeException)
            {
                isErroredHost = true;
                if (!Guide.IsVisible)
                {
                    string statement = "";
#if WINDOWS
                    statement = "A GFWL Silver (or greater) subscription is required to host a LIVE session. Please ensure that the signed in account has a Games for Windows LIVE silver or greater subscription.";
#elif XBOX
                    statement = "An Xbox LIVE account is required to host a LIVE session. Please ensure that the signed in account has an Xbox LIVE gold subscription.";
#endif
                    Guide.BeginShowMessageBox("LIVE Account Required", statement, new String[]{"OK"}, 0, MessageBoxIcon.Error, new AsyncCallback(ScreenStateToNetSelect), null);
                }
                return;
            }
            StateManager.NetworkData.CurrentSession.AllowHostMigration = false;
            StateManager.NetworkData.CurrentSession.AllowJoinInProgress = false;
            StateManager.NetworkData.RegisterNetworkSession();
        }

        void ScreenStateToNetSelect(IAsyncResult res)
        {
            StateManager.ScreenState = CoreTypes.ScreenType.NetworkSelectScreen;
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
            StateManager.NetworkData.LeaveSession();
            if (StateManager.NetworkData.AvailableSessions != null)
            {
                if (!StateManager.NetworkData.AvailableSessions.IsDisposed)
                {
                    StateManager.NetworkData.AvailableSessions.Dispose();
                }
                StateManager.NetworkData.AvailableSessions = null;
            }
            LoadingScreen lScr = StateManager.AllScreens[ScreenType.LoadingScreen.ToString()] as LoadingScreen;
            lScr.Reset();
            lScr.UserCallback = new PGCGame.CoreTypes.Delegates.AsyncHandlerMethod(FinishLanSectorHost);
            lScr.LoadingText = "Hosting\n"+(StateManager.NetworkData.SessionType == NetworkSessionType.SystemLink ? "LAN" : "LIVE")+" sector...";
            lScr.ScreenFinished += new EventHandler(hosting_finish);
            //lScr.ScreenFinished += new EventHandler(lScr_ScreenFinished);
            NetworkSessionProperties netSession = new NetworkSessionProperties();
            netSession[(int)NetworkSessionPropertyType.SessionType] = (int)StateManager.NetworkData.SessionMode;
            NetworkSession.BeginCreate(StateManager.NetworkData.SessionType, new SignedInGamer[] { Gamer.SignedInGamers[0] }, 8, 0, netSession, lScr.Callback, null);
            StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;
        }
    }
}
