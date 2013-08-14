﻿using System;
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
using Glib;
using System.ComponentModel;

namespace PGCGame.Screens.Multiplayer
{

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
                GamerInfos[e.Gamer.Id].Visible = false;
                SelectedShips.Remove(e.Gamer.Id);
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
                    if (SelectedShips[gamer.Id].Type == ShipType.NoShip || SelectedShips[gamer.Id].Tier == ShipTier.NoShip)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        TextSprite[] gamerInfos;
        Dictionary<byte, TextSprite> GamerInfos = new Dictionary<byte, TextSprite>();

        // Goes by gamer ID
        Dictionary<byte, ShipStats> SelectedShips = new Dictionary<byte, ShipStats>();

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
                            SelectedShips.Add(StateManager.NetworkData.CurrentSession.AllGamers[i].Id, new ShipStats() { Type = StateManager.NetworkData.SelectedNetworkShip.Type, Tier = ShipTier.Tier2 });
                            if (SelectedShips[StateManager.NetworkData.CurrentSession.AllGamers[i].Id].Type == ShipType.NoShip)
                            {
                                SelectedShips[StateManager.NetworkData.CurrentSession.AllGamers[i].Id] = new ShipStats() { Tier = ShipTier.NoShip, Type = ShipType.NoShip };
                            }
                        }
                        text = string.Format("{0} - {1}{2}{3}", StateManager.NetworkData.CurrentSession.AllGamers[i].Gamertag, StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal ? StateManager.NetworkData.SelectedNetworkShip.Type.ToFriendlyString() : "No ship", StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal ? StateManager.NetworkData.SelectedNetworkShip.Type == ShipType.NoShip ? "" : ", " : "", StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal ? StateManager.NetworkData.SelectedNetworkShip.Type == ShipType.NoShip ? "" : ShipTier.Tier2.ToFriendlyString().ToLower() : "");
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
                        GamerInfos.Add(StateManager.NetworkData.CurrentSession.AllGamers[i].Id, gamerInfo);
                    }
                }

                foreach (NetworkGamer ng in StateManager.NetworkData.CurrentSession.AllGamers)
                {
                    if (!SelectedShips.ContainsKey(ng.Id))
                    {
                        SelectedShips.Add(ng.Id, new ShipStats() { Type = ShipType.NoShip, Tier = ShipTier.NoShip });
                    }
                }
                StateManager.NetworkData.CurrentSession.GamerLeft += new EventHandler<GamerLeftEventArgs>(CurrentSession_GamerLeft);
                StateManager.NetworkData.CurrentSession.GamerJoined += new EventHandler<GamerJoinedEventArgs>(CurrentSession_GamerJoined);
                StateManager.NetworkData.CurrentSession.GameStarted += new EventHandler<GameStartedEventArgs>(CurrentSession_GameStarted);
                _firstInit = false;
            }
        }

        void CurrentSession_GameStarted(object sender, GameStartedEventArgs e)
        {
            //TODO: This is Async
            //TODO: Need to tag ship w/ gamer ID to mark your ship
            /*
            while (!StateManager.NetworkData.CurrentSession.LocalGamers[0].IsDataAvailable)
            {

            }
            while (StateManager.NetworkData.CurrentSession.LocalGamers[0].IsDataAvailable)
            {
                NetworkGamer dataSender;
                StateManager.NetworkData.CurrentSession.LocalGamers[0].ReceiveData(StateManager.NetworkData.DataReader, out dataSender);
            }
            */
            if (!StateManager.NetworkData.CurrentSession.LocalGamers[0].IsHost)
            {
                BackgroundWorker preDataRecv = new BackgroundWorker();
                
                LoadingScreen lScr = StateManager.AllScreens[ScreenType.LoadingScreen.ToString()] as LoadingScreen;
                lScr.Reset();
                preDataRecv.DoWork += new DoWorkEventHandler(preDataRecv_DoWork);
                preDataRecv.RunWorkerCompleted += lScr.BackgroundWorkerCallback;
                lScr.UserCallback = new Delegates.AsyncHandlerMethod(onDataRecv);
                lScr.ScreenFinished += new EventHandler(lScr_ScreenFinished);
                lScr.LoadingText = "Waiting for\ninformation\nfrom host...";
                StateManager.ScreenState = CoreTypes.ScreenType.LoadingScreen;
                preDataRecv.RunWorkerAsync();
                //StateManager.ScreenState = CoreTypes.ScreenType.Game;
            }
            //StartButton.Visible = true;
            //StartLabel.Visible = true;
        }

        void lScr_ScreenFinished(object sender, EventArgs e)
        {
            StateManager.ScreenState = CoreTypes.ScreenType.Game;
        }

        void onDataRecv(object res)
        {
            Vector4 myShip = (Vector4)res;
            StateManager.SelectedShip = StateManager.NetworkData.SelectedNetworkShip.Type;
            StateManager.SelectedTier = StateManager.NetworkData.SelectedNetworkShip.Tier;

            /*Ship player = StateManager.GetScreen<GameScreen>(CoreTypes.ScreenType.Game).playerShip;
            player.Position = new Vector2(myShip.X, myShip.Y);
            player.Rotation = SpriteRotation.FromRadians(myShip.Z);
            player.CurrentHealth = myShip.W.ToInt();*/

            foreach (NetworkGamer g in StateManager.NetworkData.CurrentSession.RemoteGamers)
            {
                SoloNetworkShip sns = new SoloNetworkShip(SelectedShips[g.Id].Type, SelectedShips[g.Id].Tier, GameScreen.World, g);
                StateManager.EnemyShips.Add(sns);
                StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(sns);
            }
        }

        List<NetworkShip> netShips = new List<NetworkShip>();

        void preDataRecv_DoWork(object sender, DoWorkEventArgs e)
        {
            LocalNetworkGamer netGamer = StateManager.NetworkData.CurrentSession.LocalGamers[0];
            while (!netGamer.IsDataAvailable)
            {

            }
            netShips.Clear();
            Vector4 myShip = new Vector4();

            StateManager.InitializeSingleplayerGameScreen(SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Type, SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Tier, false);

            while (netGamer.IsDataAvailable)
            {
                NetworkGamer infosender;
                netGamer.ReceiveData(StateManager.NetworkData.DataReader, out infosender);
                Vector4 ship = StateManager.NetworkData.DataReader.ReadVector4();

                if (infosender.Id == netGamer.Id)
                {
                    myShip = ship;
                }
                else
                {
                    NetworkShip netShip = NetworkShip.CreateFromData(ship, infosender);
                    netShips.Add(netShip);
                    //StateManager.EnemyShips.Add(netShip);
                    //StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(netShip);
                }
            }
            e.Result = myShip;
        }

        void CurrentSession_GamerJoined(object sender, GamerJoinedEventArgs e)
        {
            //gamerInfos = new TextSprite[StateManager.NetworkData.CurrentSession.MaxGamers];
            GamerInfos.Clear();
            for (int i = 0; i < StateManager.NetworkData.CurrentSession.MaxGamers; i++)
            {
                if (gamerInfos[i] != null)
                {
                    AdditionalSprites.Remove(gamerInfos[i]);
                }
                string text = "";
                bool visible = false;
                if (i < StateManager.NetworkData.CurrentSession.AllGamers.Count)
                {
                    if (StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal)
                    {
                        SelectedShips.Remove(StateManager.NetworkData.CurrentSession.AllGamers[i].Id);
                        SelectedShips.Add(StateManager.NetworkData.CurrentSession.AllGamers[i].Id, new ShipStats() { Type = StateManager.NetworkData.SelectedNetworkShip.Type, Tier = ShipTier.Tier2 });
                        if (SelectedShips[StateManager.NetworkData.CurrentSession.AllGamers[i].Id].Type == ShipType.NoShip)
                        {
                            SelectedShips[StateManager.NetworkData.CurrentSession.AllGamers[i].Id] = new ShipStats() { Tier = ShipTier.NoShip, Type = ShipType.NoShip };
                        }
                    }
                    text = string.Format("{0} - {1}{2}{3}", StateManager.NetworkData.CurrentSession.AllGamers[i].Gamertag, StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal ? StateManager.NetworkData.SelectedNetworkShip.Type.ToFriendlyString() : "No ship", StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal ? StateManager.NetworkData.SelectedNetworkShip.Type == ShipType.NoShip ? "" : ", " : "", StateManager.NetworkData.CurrentSession.AllGamers[i].IsLocal ? StateManager.NetworkData.SelectedNetworkShip.Type == ShipType.NoShip ? "" : ShipTier.Tier2.ToFriendlyString().ToLower() : "");
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
                    GamerInfos.Add(StateManager.NetworkData.CurrentSession.AllGamers[i].Id, gamerInfo);
                }
            }

            foreach (NetworkGamer ng in StateManager.NetworkData.CurrentSession.AllGamers)
            {
                if (!SelectedShips.ContainsKey(ng.Id))
                {
                    SelectedShips.Add(ng.Id, new ShipStats() { Type = ShipType.NoShip, Tier = ShipTier.NoShip });
                }
            }

            if (!e.Gamer.IsLocal)
            {
                StateManager.NetworkData.DataWriter.Write(StateManager.NetworkData.SelectedNetworkShip.Type.ToString());
                foreach (LocalNetworkGamer g in StateManager.NetworkData.CurrentSession.LocalGamers)
                {
                    g.SendData(StateManager.NetworkData.DataWriter, SendDataOptions.Reliable);
                }
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
                StateManager.SelectedShip = SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Type;

                StateManager.InitializeSingleplayerGameScreen(SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Type, SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Tier, false);

                foreach (NetworkGamer g in StateManager.NetworkData.CurrentSession.RemoteGamers)
                {
                    SoloNetworkShip sns = new SoloNetworkShip(SelectedShips[g.Id].Type, SelectedShips[g.Id].Tier, GameScreen.World, g);
                    StateManager.EnemyShips.Add(sns);
                    StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(sns);
                }
                foreach (LocalNetworkGamer locGamer in StateManager.NetworkData.CurrentSession.LocalGamers)
                {
                    locGamer.SendData(StateManager.NetworkData.DataWriter, SendDataOptions.Reliable);
                }
                StateManager.NetworkData.CurrentSession.StartGame();

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

                        TextSprite gamerInfo = GamerInfos[sender.Id];
                        SelectedShips[sender.Id] = new ShipStats() { Type = shipOfPlayer, Tier = playerShipTier };



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
