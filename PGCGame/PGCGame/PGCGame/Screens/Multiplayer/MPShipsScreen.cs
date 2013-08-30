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
using Glib;
using System.ComponentModel;
using PGCGame.Ships.Allies;

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
            if (Visible && GamerInfos.Count > 0)
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

        /// <summary>
        /// Gets a boolean indicating whether or not all gamers have ships.
        /// </summary>
        /// <remarks>
        /// Expensive-ish.
        /// </remarks>
        public bool AllGamersHaveShips
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

        /// <summary>
        /// Mid-game player leaving code.
        /// </summary>
        void gameLeave(object o, GamerLeftEventArgs glea)
        {
            if (glea.Gamer.IsHost)
            {
                StateManager.ScreenState = CoreTypes.ScreenType.NetworkSelectScreen;
            }
            for(int i = 0; i < StateManager.EnemyShips.Count; i++)
            {
                BaseAllyShip sh = StateManager.EnemyShips[i] as BaseAllyShip;
                if (sh != null && sh.Controller != null && sh.Controller.Id == glea.Gamer.Id)
                {
                    sh.CurrentHealth = 0;
                    StateManager.EnemyShips.RemoveAt(i);
                    return;
                }
            }
        }

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                title.Text = StateManager.NetworkData.SessionMode.ToFriendlyString() + " Lobby";
                title.X = title.GetCenterPosition(Graphics.Viewport).X;


                if (_firstInit)
                {
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
                }
                _firstInit = false;
            }
        }

        /// <summary>
        /// Called on the computer of ALL PLAYERS when the game is started by the host.
        /// </summary>
        void CurrentSession_GameStarted(object sender, GameStartedEventArgs e)
        {
            //Non-host code
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

            //All player code
            StateManager.NetworkData.CurrentSession.GamerLeft += new EventHandler<GamerLeftEventArgs>(gameLeave);
            foreach (NetworkGamer g in StateManager.NetworkData.CurrentSession.RemoteGamers)
            {
                StateManager.NetworkData.CurrentSession.LocalGamers[0].EnableSendVoice(g, false);
            }
        }

        /// <summary>
        /// Client-side switch to GameScreen after data reception. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lScr_ScreenFinished(object sender, EventArgs e)
        {
            StateManager.ScreenState = CoreTypes.ScreenType.Game;
        }

        /// <summary>
        /// Called on the CLIENT side when the host-sent ship data has been received.
        /// </summary>
        void onDataRecv(object res)
        {
            Dictionary<byte, Vector4> myShips = (Dictionary<byte, Vector4>)res;

            GameScreen game = StateManager.GetScreen<GameScreen>(CoreTypes.ScreenType.Game);

            /*Ship player = StateManager.GetScreen<GameScreen>(CoreTypes.ScreenType.Game).playerShip;
            player.Position = new Vector2(myShip.X, myShip.Y);
            player.Rotation = SpriteRotation.FromRadians(myShip.Z);
            player.CurrentHealth = myShip.W.ToInt();*/
            Vector4 me = myShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id];

            game.playerShip.WorldCoords = new Vector2(me.X, me.Y);
            game.playerShip.Rotation.Radians = (me.Z);
            game.playerShip.Controller = StateManager.NetworkData.CurrentSession.LocalGamers[0];
            game.playerShip.CurrentHealth = me.W.ToInt();
            game.playerShip.WCMoved += new EventHandler(playerShip_NetworkStateChanged);
            game.playerShip.Rotation.ValueChanged += new EventHandler(playerShip_NetworkStateChanged);
            //game.playerShip.WCMoved += new EventHandler(breakHandle);
            game.playerShip.HealthChanged += new EventHandler(playerShip_NetworkStateChanged);

            foreach (NetworkGamer g in StateManager.NetworkData.CurrentSession.RemoteGamers)
            {
                BaseAllyShip sns = BaseAllyShip.CreateShip(SelectedShips[g.Id], GameScreen.World, false);
                sns.Controller = g;
                sns.PlayerType = PlayerType.Solo;
                sns.RotateTowardsMouse = false;
                Vector4 gamerShip = myShips[g.Id];
                sns.WorldCoords = new Vector2(gamerShip.X, gamerShip.Y);
                sns.Rotation.Radians = gamerShip.Z;
                sns.CurrentHealth = gamerShip.W.ToInt();

                StateManager.EnemyShips.Add(sns);
                StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(sns);
            }
        }

        /// <summary>
        /// Called when the LOCAL SHIP needs to send network data.
        /// </summary>
        void playerShip_NetworkStateChanged(object sender, EventArgs e)
        {
            BaseAllyShip yourShip = StateManager.GetScreen<GameScreen>(CoreTypes.ScreenType.Game).playerShip;
            //False = not a bullet
            StateManager.NetworkData.DataWriter.Write(false);
            
            StateManager.NetworkData.DataWriter.Write(new Vector4(yourShip.WorldCoords.X, yourShip.WorldCoords.Y, yourShip.Rotation.Radians, yourShip.CurrentHealth));

            //InOrder - because old versions never arrive after a more recent version
            StateManager.NetworkData.CurrentSession.LocalGamers[0].SendData(StateManager.NetworkData.DataWriter, SendDataOptions.InOrder);
        }

        
        /// <summary>
        /// Called when the CLIENT begins waiting for host ship data.
        /// </summary>
        void preDataRecv_DoWork(object sender, DoWorkEventArgs e)
        {
            LocalNetworkGamer netGamer = StateManager.NetworkData.CurrentSession.LocalGamers[0];
            while (!netGamer.IsDataAvailable)
            {

            }

            Dictionary<byte, Vector4> ships = new Dictionary<byte, Vector4>();

            StateManager.SelectedShip = StateManager.NetworkData.SelectedNetworkShip.Type;
            StateManager.SelectedTier = StateManager.NetworkData.SelectedNetworkShip.Tier;
            StateManager.InitializeSingleplayerGameScreen(SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Type, SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Tier, false);

            int gamersReceived = 0;

            while (netGamer.IsDataAvailable && gamersReceived < StateManager.NetworkData.CurrentSession.AllGamers.Count)
            {
                NetworkGamer infosender;
                netGamer.ReceiveData(StateManager.NetworkData.DataReader, out infosender);
                Vector4 ship = StateManager.NetworkData.DataReader.ReadVector4();
                Byte targetPlayer = StateManager.NetworkData.DataReader.ReadByte();

                ships.Add(targetPlayer, ship);
                gamersReceived++;
                /*
            else
            {
                NetworkShip netShip = NetworkShip.CreateFromData(ship, infosender);
                netShips.Add(netShip);
                //StateManager.EnemyShips.Add(netShip);
                //StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(netShip);
            }
                 */
            }

            e.Result = ships;
        }

        void CurrentSession_GamerJoined(object sender, GamerJoinedEventArgs e)
        {
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

        /// <summary>
        /// Start label pressed event handler.
        /// </summary>
        /// <remarks>
        /// Called when the HOST presses the start label.
        /// </remarks>
        void StartLabel_Pressed(object sender, EventArgs e)
        {
            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }
            if (StateManager.NetworkData.SessionMode == MultiplayerSessionType.LMS)
            {
                StateManager.SelectedShip = SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Type;

                StateManager.InitializeSingleplayerGameScreen(SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Type, SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Tier, false);

                GameScreen game = StateManager.GetScreen<GameScreen>(CoreTypes.ScreenType.Game);

                Ship my = game.playerShip;

                StateManager.NetworkData.DataWriter.Write(new Vector4(my.WorldCoords.X, my.WorldCoords.Y, my.Rotation.Radians, my.CurrentHealth));
                StateManager.NetworkData.DataWriter.Write(StateManager.NetworkData.CurrentSession.LocalGamers[0].Id);

                StateManager.NetworkData.CurrentSession.LocalGamers[0].SendData(StateManager.NetworkData.DataWriter, SendDataOptions.Reliable);

                my.WCMoved += new EventHandler(playerShip_NetworkStateChanged);
                my.Rotation.ValueChanged += new EventHandler(playerShip_NetworkStateChanged);
                my.HealthChanged += new EventHandler(playerShip_NetworkStateChanged);

                foreach (NetworkGamer g in StateManager.NetworkData.CurrentSession.RemoteGamers)
                {
                    BaseAllyShip sns = BaseAllyShip.CreateShip(SelectedShips[g.Id], GameScreen.World, false);
                    sns.PlayerType = PlayerType.Solo;
                    sns.RotateTowardsMouse = false;
                    sns.Controller = g;
                    sns.WorldCoords = StateManager.RandomGenerator.NextVector2(new Vector2(500), new Vector2(StateManager.SpawnArea.X + StateManager.SpawnArea.Width, StateManager.SpawnArea.Y + StateManager.SpawnArea.Height));

                    StateManager.NetworkData.DataWriter.Write(new Vector4(sns.WorldCoords.X, sns.WorldCoords.Y, sns.Rotation.Radians, sns.CurrentHealth));
                    StateManager.NetworkData.DataWriter.Write(g.Id);

                    StateManager.EnemyShips.Add(sns);
                    StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(sns);
                    StateManager.NetworkData.CurrentSession.LocalGamers[0].SendData(StateManager.NetworkData.DataWriter, SendDataOptions.Reliable);
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
