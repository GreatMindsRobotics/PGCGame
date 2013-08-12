using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;
using Glib.XNA.InputLib;

namespace PGCGame.Screens.Multiplayer
{
    public class LobbyScreen : BaseScreen
    {
        public LobbyScreen(SpriteBatch sb)
            : base(sb, Color.Black)
        {

        }

        TextSprite title;
        TextSprite[] allGamerInfos;
        Sprite BackButton;
        TextSprite BackLabel;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            title = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.BoldText, "Lobby", Color.White);
            title.Position = new Vector2(title.GetCenterPosition(Graphics.Viewport).X, 5);
            AdditionalSprites.Add(title);

            BackButton = new Sprite(GameContent.GameAssets.Images.Controls.Button, new Vector2(20, Graphics.Viewport.Height), Sprites.SpriteBatch);
            BackButton.Y -= BackButton.Height + 20;
            BackLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "Back", Color.White) { ParentSprite = BackButton, IsHoverable = true, HoverColor = Color.MediumAquamarine, NonHoverColor = Color.White};
            BackLabel.Pressed += new EventHandler(BackLabel_Pressed);

            Sprites.Add(BackButton);
            AdditionalSprites.Add(BackLabel);
        }

        void BackLabel_Pressed(object sender, EventArgs e)
        {
            //TODO
            StateManager.GoBack();
        }

        public void InitScreen()
        {
            StateManager.NetworkData.CurrentSession.GamerJoined += new EventHandler<Microsoft.Xna.Framework.Net.GamerJoinedEventArgs>(CurrentSession_GamerJoined);
            StateManager.NetworkData.CurrentSession.GamerLeft += new EventHandler<Microsoft.Xna.Framework.Net.GamerLeftEventArgs>(CurrentSession_GamerLeft);
            //gamersInSession.AddRange(StateManager.NetworkData.CurrentSession.AllGamers);
            
        }

        void CurrentSession_GamerLeft(object sender, Microsoft.Xna.Framework.Net.GamerLeftEventArgs e)
        {
            foreach (TextSprite t in allGamerInfos)
            {
                if (t.Text == e.Gamer.Gamertag)
                {
                    t.Visible = false;
                }
            }
        }

        void gamerInfo_TextChanged(object sender, EventArgs e)
        {
            TextSprite gamerInfo = sender as TextSprite;
            gamerInfo.X = gamerInfo.GetCenterPosition(Graphics.Viewport).X;
        }

        void CurrentSession_GamerJoined(object sender, Microsoft.Xna.Framework.Net.GamerJoinedEventArgs e)
        {
            gamersInSession.Add(e.Gamer);

            if (e.Gamer.IsHost && e.Gamer.IsLocal)
            {
                e.Gamer.IsReady = true;
            }

            float y = title.Y + title.Font.LineSpacing;
            allGamerInfos = new TextSprite[StateManager.NetworkData.CurrentSession.MaxGamers];
            for (int i = 0; i < StateManager.NetworkData.CurrentSession.MaxGamers; i++)
            {
                TextSprite gamerInfo = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "A RANDOM GAMER THAT LIKES YOU", Color.White);
                gamerInfo.Visible = allGamerInfos[i] != null;
                gamerInfo.X = gamerInfo.GetCenterPosition(Graphics.Viewport).X;
                gamerInfo.Y = y + 5;
                gamerInfo.TextChanged += new EventHandler(gamerInfo_TextChanged);
                y += gamerInfo.Height;
                allGamerInfos[i] = gamerInfo;
                AdditionalSprites.Add(gamerInfo);
            }

            allGamerInfos[gamersInSession.Count - 1].Text = e.Gamer.Gamertag;
            allGamerInfos[gamersInSession.Count - 1].Visible = true;
        }

        readonly List<Gamer> gamersInSession = new List<Gamer>();

        private KeyboardState _lastState;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_lastState.IsKeyUp(Keys.R) && KeyboardManager.State.IsKeyDown(Keys.R))
            {
                foreach (LocalNetworkGamer g in StateManager.NetworkData.CurrentSession.LocalGamers)
                {
                    g.IsReady = !g.IsReady;
                }
            }

            foreach (TextSprite t in allGamerInfos)
            {
                foreach (NetworkGamer g in StateManager.NetworkData.CurrentSession.AllGamers)
                {
                    if (t.Visible)
                    {
                        if (t.Text == g.Gamertag)
                        {
                            t.Color = g.IsReady ? Color.LimeGreen : Color.White;
                        }
                    }
                }

                _lastState = KeyboardManager.State;
            }
        }
    }
}
