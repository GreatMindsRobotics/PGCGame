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
using PGCGame.Ships.Network;

namespace PGCGame.Screens.Multiplayer
{
    struct ShipData
    {
        public ShipTier Tier;
        public ShipType Type;
    }

    public class MPShipsScreen : BaseScreen
    {
        public MPShipsScreen(SpriteBatch sb)
            : base(sb, Color.Black)
        {
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
            
        }

        Sprite StartButton;
        TextSprite StartLabel;

        void CurrentSession_GamerLeft(object sender, GamerLeftEventArgs e)
        {
            if (GamerInfos.Count > 0)
            {
                GamerInfos[e.Gamer].Visible = false;
                SelectedShips.Remove(e.Gamer);
                bool isHost = false;
                foreach (LocalNetworkGamer lng in StateManager.NetworkData.CurrentSession.LocalGamers)
                {
                    if (lng.IsHost)
                    {
                        isHost = true;
                    }
                }
                if (AllGamersHaveShips && isHost)
                {
                    StartButton.Visible = true;
                    StartLabel.Visible = true;
                }
            }
        }

        bool AllGamersHaveShips
        {
            get
            {
                foreach (NetworkGamer gamer in StateManager.NetworkData.CurrentSession.AllGamers)
                {
                    if (SelectedShips[gamer].Type == ShipType.NoShip || SelectedShips[gamer].Tier == ShipTier.NoShip)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        TextSprite[] gamerInfos;
        Dictionary<Gamer, TextSprite> GamerInfos = new Dictionary<Gamer, TextSprite>();

        Dictionary<Gamer, ShipData> SelectedShips = new Dictionary<Gamer, ShipData>();

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
                GamerInfos.Clear();
                for (int i = 0; i < StateManager.NetworkData.CurrentSession.MaxGamers; i++)
                {
                    string text = "";
                    bool visible = false;
                    if (i < StateManager.NetworkData.CurrentSession.AllGamers.Count)
                    {
                        if (StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal)
                        {
                            SelectedShips.Add(StateManager.NetworkData.CurrentSession.AllGamers[i], new ShipData() { Type = StateManager.NetworkData.SelectedNetworkShip, Tier = ShipTier.Tier2 });
                            if(SelectedShips[StateManager.NetworkData.CurrentSession.AllGamers[i]].Type == ShipType.NoShip)
                            {
                                SelectedShips[StateManager.NetworkData.CurrentSession.AllGamers[i]] = new ShipData() {Tier = ShipTier.NoShip, Type = ShipType.NoShip};
                            }
                        }
                        text = string.Format("{0} - {1}{2}{3}", StateManager.NetworkData.CurrentSession.AllGamers[i].Gamertag, StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal ? StateManager.NetworkData.SelectedNetworkShip.ToFriendlyString() : "No ship", StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal ? StateManager.NetworkData.SelectedNetworkShip == ShipType.NoShip ? "" : ", " : "", StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal ? StateManager.NetworkData.SelectedNetworkShip == ShipType.NoShip ? "" : ShipTier.Tier2.ToFriendlyString().ToLower() : "");
                        visible = true;
                    }
                    TextSprite gamerInfo = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, text, Color.White);
                    gamerInfo.Y = i == 0 ? title.Y + title.Height + 10 : gamerInfos[i - 1].Y + gamerInfos[i - 1].Height + 5;
                    gamerInfo.X = gamerInfo.GetCenterPosition(Graphics.Viewport).X;
                    gamerInfo.Visible = visible;
                    AdditionalSprites.Add(gamerInfo);
                    gamerInfos[i] = gamerInfo;
                    if (i < StateManager.NetworkData.CurrentSession.AllGamers.Count)
                    {
                        GamerInfos.Add(StateManager.NetworkData.CurrentSession.AllGamers[i], gamerInfo);
                    }
                }

                foreach (NetworkGamer ng in StateManager.NetworkData.CurrentSession.AllGamers)
                {
                    if (!SelectedShips.ContainsKey(ng))
                    {
                        SelectedShips.Add(ng, new ShipData() { Type = ShipType.NoShip, Tier = ShipTier.NoShip} );
                    }
                }
                StateManager.NetworkData.CurrentSession.GamerLeft += new EventHandler<GamerLeftEventArgs>(CurrentSession_GamerLeft);
                _firstInit = false;
            }
        }



        private bool _firstInit = true;
        TextSprite title;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            title = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.BoldText, StateManager.NetworkData.SessionMode.ToFriendlyString() + " Lobby", Color.White);
            title.Y = 5;
            title.X = title.GetCenterPosition(Graphics.Viewport).X;
            AdditionalSprites.Add(title);

            StartButton = new Sprite(GameContent.GameAssets.Images.Controls.Button, new Vector2(0, Graphics.Viewport.Height), Sprites.SpriteBatch);
            StartButton.X = StartButton.GetCenterPosition(Graphics.Viewport).X;
            StartButton.Y -= StartButton.Height + 20;
            StartLabel = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "Start", Color.White) { ParentSprite = StartButton, IsHoverable = true, HoverColor = Color.MediumAquamarine, NonHoverColor = Color.White };
            StartLabel.Pressed += new EventHandler(StartLabel_Pressed);
            StartButton.Visible = false;
            StartLabel.Visible = false;
            Sprites.Add(StartButton);
            AdditionalSprites.Add(StartLabel);
        }

        void StartLabel_Pressed(object sender, EventArgs e)
        {
            if (StateManager.NetworkData.SessionMode == MultiplayerSessionType.Deathmatch)
            {
                StateManager.SelectedShip = SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0]].Type;

                StateManager.InitializeSingleplayerGameScreen(SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0]].Type, SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0]].Tier, false);

                if (StateManager.NetworkData.CurrentSession.LocalGamers[0].IsHost)
                {
                    foreach (NetworkGamer g in StateManager.NetworkData.CurrentSession.RemoteGamers)
                    {
                        StateManager.EnemyShips.Add(new SoloNetworkShip(SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0]].Type, SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0]].Tier, GameScreen.World) { ControllingGamer = g });
                    }
                }
                StateManager.CurrentLevel = GameLevel.Level1;
                StateManager.ScreenState = CoreTypes.ScreenType.Game;
            }
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
                        ShipType shipOfPlayer = (ShipType)Enum.Parse(typeof(ShipType), ship, true);
                        ShipTier playerShipTier = ShipTier.Tier2;
                        if (StateManager.NetworkData.SessionMode == MultiplayerSessionType.Coop)
                        {
                            //TODO: Send/receive tiers of ships
                        }
                        
                        TextSprite gamerInfo = GamerInfos[sender];
                        SelectedShips[sender] = new ShipData(){Type = shipOfPlayer, Tier = playerShipTier};

                        

                        gamerInfo.Text = string.Format("{0} - {1}{3}{2}", sender.Gamertag, shipOfPlayer.ToFriendlyString(), shipOfPlayer == ShipType.NoShip ? "" : playerShipTier.ToFriendlyString().ToLower(), shipOfPlayer == ShipType.NoShip ? "" : ", ");
                        gamerInfo.X = gamerInfo.GetCenterPosition(Graphics.Viewport).X;
                        if (AllGamersHaveShips && gamer.IsHost)
                        {
                            StartButton.Visible = true;
                            StartLabel.Visible = true;
                        }
                    }
                }
            }
        }
    }
}
