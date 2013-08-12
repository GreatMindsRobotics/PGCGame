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
    public class MPShipsScreen : BaseScreen
    {
        public MPShipsScreen(SpriteBatch sb) : base(sb, Color.Black)
        {
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
        }

        TextSprite[] gamerInfos;
        Dictionary<Gamer, TextSprite> GamerInfos = new Dictionary<Gamer, TextSprite>();

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (_firstInit && StateManager.ScreenState == this.ScreenType)
            {
                title.Text = StateManager.NetworkData.SessionMode.ToFriendlyString() + " Lobby";
                char[] allChars = title.Text.ToCharArray();
                allChars[0] = char.ToUpper(allChars[0]);
                title.Text = new string(allChars);
                title.X = title.GetCenterPosition(Graphics.Viewport).X;

                gamerInfos = new TextSprite[StateManager.NetworkData.CurrentSession.MaxGamers];

                for (int i = 0; i < StateManager.NetworkData.CurrentSession.MaxGamers; i++)
                {
                    string text = "";
                    bool visible = false;
                    if (i < StateManager.NetworkData.CurrentSession.AllGamers.Count)
                    {
                        text = string.Format("{0} - {1}", StateManager.NetworkData.CurrentSession.AllGamers[i].Gamertag, StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal ? StateManager.NetworkData.SelectedNetworkShip.ToFriendlyString() : "No ship");
                        visible = true;
                    }
                    TextSprite gamerInfo = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, text, Color.White);
                    gamerInfo.Y = i == 0 ? title.Y + title.Height + 10 : gamerInfos[i-1].Y+gamerInfos[i-1].Height+5;
                    gamerInfo.X = gamerInfo.GetCenterPosition(Graphics.Viewport).X;
                    gamerInfo.Visible = visible;
                    AdditionalSprites.Add(gamerInfo);
                    gamerInfos[i] = gamerInfo;
                    if (i < StateManager.NetworkData.CurrentSession.AllGamers.Count)
                    {
                        GamerInfos.Add(StateManager.NetworkData.CurrentSession.AllGamers[i], gamerInfo);
                    }
                }

                _firstInit = false;
            }
        }

        

        private bool _firstInit = true;
        TextSprite title;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            title = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.BoldText, StateManager.NetworkData.SessionMode.ToFriendlyString()+" Lobby", Color.White);
            title.Y = 5;
            title.X = title.GetCenterPosition(Graphics.Viewport).X;
            AdditionalSprites.Add(title);
        }

        public override void Update(GameTime game)
        {
            base.Update(game);
            foreach (LocalNetworkGamer gamer in StateManager.NetworkData.CurrentSession.LocalGamers)
            {
                // Keep reading while packets are available.
                while (gamer.IsDataAvailable)
                {
                    NetworkGamer sender;

                    // Read a single packet.
                    gamer.ReceiveData(StateManager.NetworkData.DataReader, out sender);

                    if (!sender.IsLocal)
                    {
                        string ship = StateManager.NetworkData.DataReader.ReadString();
                        ShipType shipOfPlayer = (ShipType) Enum.Parse(typeof(ShipType), ship, true);
                        TextSprite gamerInfo = GamerInfos[sender];
                        gamerInfo.Text = string.Format("{0} - {1}", sender.Gamertag, shipOfPlayer.ToFriendlyString());
                        gamerInfo.X = gamerInfo.GetCenterPosition(Graphics.Viewport).X;
                    }
                }
            }
        }
    }
}
