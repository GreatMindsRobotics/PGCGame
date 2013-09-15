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
using Glib.XNA.InputLib;
using PGCGame.Ships.Enemies;

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
            for (int i = 0; i < StateManager.EnemyShips.Count; i++)
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
                        TextSprite gamerInfo = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.NormalText, text, Color.White);
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

        private static class RandomTeamGenerator
        {
            public static MultiplayerTeam EnemyOf(MultiplayerTeam team)
            {
                if (team == MultiplayerTeam.Red)
                {
                    return MultiplayerTeam.Blue;
                }
                else if (team == MultiplayerTeam.Blue)
                {
                    return MultiplayerTeam.Red;
                }
                throw new NotImplementedException("The specified team is not implemented.");
            }

            private static MultiplayerTeam RandomTeam()
            {
                return StateManager.RandomGenerator.NextDouble() <= 0.5 ? MultiplayerTeam.Red : MultiplayerTeam.Blue;
            }

            private static MultiplayerTeam RandomTeamModifyCounter(ref int redCount, ref int blueCount)
            {
                MultiplayerTeam rand = RandomTeam();
                if (rand == MultiplayerTeam.Blue)
                {
                    blueCount++;
                }
                else if (rand == MultiplayerTeam.Red)
                {
                    redCount++;
                }
                return rand;
            }

            public static MultiplayerTeam GenerateTeam(ref int redCount, ref int blueCount)
            {
                if (redCount + blueCount > 0)
                {
                    if (redCount == 0)
                    {
                        redCount++;
                        return MultiplayerTeam.Red;
                    }
                    else if (blueCount == 0)
                    {
                        blueCount++;
                        return MultiplayerTeam.Blue;
                    }
                    else
                    {
                        return RandomTeamModifyCounter(ref redCount, ref blueCount);
                    }
                }
                else
                {
                    return RandomTeamModifyCounter(ref redCount, ref blueCount);
                }
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

        private struct NetworkPlayerID
        {
            public Byte? ID;
            public Guid? NonPlayerId;

            public NetworkPlayerID(Guid id)
            {
                NonPlayerId = id;
                ID = null;
            }

            public NetworkPlayerID(Byte id)
            {
                ID = id;
                NonPlayerId = null;
            }
        }

        /// <summary>
        /// Called on the CLIENT side when the host-sent ship data has been received.
        /// </summary>
        void onDataRecv(object res)
        {
            Dictionary<NetworkPlayerID, KeyValuePair<MultiplayerTeam?, Vector4>> myShips = res as Dictionary<NetworkPlayerID, KeyValuePair<MultiplayerTeam?, Vector4>>;

            GameScreen game = StateManager.GetScreen<GameScreen>(CoreTypes.ScreenType.Game);

            /*Ship player = StateManager.GetScreen<GameScreen>(CoreTypes.ScreenType.Game).playerShip;
            player.Position = new Vector2(myShip.X, myShip.Y);
            player.Rotation = SpriteRotation.FromRadians(myShip.Z);
            player.CurrentHealth = myShip.W.ToInt();*/
            KeyValuePair<MultiplayerTeam?, Vector4> me = myShips[new NetworkPlayerID(StateManager.NetworkData.CurrentSession.LocalGamers[0].Id)];


            game.playerShip.WorldCoords = new Vector2(me.Value.X, me.Value.Y);
            game.playerShip.Rotation.Radians = (me.Value.Z);
            game.playerShip.Controller = StateManager.NetworkData.CurrentSession.LocalGamers[0];
            game.playerShip.CurrentHealth = me.Value.W.ToInt();
            game.playerShip.WCMoved += new EventHandler(playerShip_NetworkStateChanged);
            game.playerShip.Rotation.ValueChanged += new EventHandler(playerShip_NetworkStateChanged);
            //game.playerShip.WCMoved += new EventHandler(breakHandle);
            game.playerShip.HealthChanged += new EventHandler(playerShip_NetworkStateChanged);

            foreach (NetworkGamer g in StateManager.NetworkData.CurrentSession.RemoteGamers)
            {
                BaseAllyShip sns = BaseAllyShip.CreateShip(SelectedShips[g.Id], GameScreen.World, false);
                sns.Controller = g;
                KeyValuePair<MultiplayerTeam?, Vector4> gamerShip = myShips[new NetworkPlayerID(g.Id)];
                sns.PlayerType = gamerShip.Key.HasValue && gamerShip.Key.Value == me.Key.Value ? PlayerType.Ally : PlayerType.Solo;
                sns.RotateTowardsMouse = false;

                sns.WorldCoords = new Vector2(gamerShip.Value.X, gamerShip.Value.Y);
                sns.Rotation.Radians = gamerShip.Value.Z;
                sns.CurrentHealth = gamerShip.Value.W.ToInt();

                (gamerShip.Key.HasValue && gamerShip.Key.Value == me.Key.Value ? StateManager.AllyShips : StateManager.EnemyShips).Add(sns);

                StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(sns);
            }

            foreach (var v in myShips)
            {
                if (!v.Key.ID.HasValue)
                {

                    BaseEnemyShip enemy = BaseEnemyShip.CreateRandomEnemy(GameScreen.World);
                    enemy.WorldCoords = new Vector2(v.Value.Value.X, v.Value.Value.Y);
                    enemy.Rotation.Radians = v.Value.Value.Z;
                    enemy.CurrentHealth = v.Value.Value.W.ToInt();

                    game.Sprites.Add(enemy);
                    game.enemies.Add(enemy);
                    StateManager.EnemyShips.Add(enemy);

                    /*
                    BaseAllyShip sns = BaseAllyShip.CreateShip(SelectedShips[g.Id], GameScreen.World, false);
                    sns.Controller = g;
                    KeyValuePair<MultiplayerTeam?, Vector4> gamerShip = myShips[new NetworkPlayerID(g.Id)];
                    sns.PlayerType = gamerShip.Key.HasValue && gamerShip.Key.Value == me.Key.Value ? PlayerType.Ally : PlayerType.Solo;
                    sns.RotateTowardsMouse = false;

                    sns.WorldCoords = new Vector2(gamerShip.Value.X, gamerShip.Value.Y);
                    sns.Rotation.Radians = gamerShip.Value.Z;
                    sns.CurrentHealth = gamerShip.Value.W.ToInt();

                    (gamerShip.Key.HasValue && gamerShip.Key.Value == me.Key.Value ? StateManager.AllyShips : StateManager.EnemyShips).Add(sns);

                    StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(sns);
                    */
                }
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

            Dictionary<NetworkPlayerID, KeyValuePair<MultiplayerTeam?, Vector4>> ships = new Dictionary<NetworkPlayerID, KeyValuePair<MultiplayerTeam?, Vector4>>();

            StateManager.SelectedShip = StateManager.NetworkData.SelectedNetworkShip.Type;
            StateManager.SelectedTier = StateManager.NetworkData.SelectedNetworkShip.Tier;
            StateManager.InitializeSingleplayerGameScreen(SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Type, SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Tier, false);

            int gamersReceived = 0;

            while (netGamer.IsDataAvailable && (gamersReceived < StateManager.NetworkData.CurrentSession.AllGamers.Count || StateManager.NetworkData.SessionMode == MultiplayerSessionType.Coop))
            {
                NetworkGamer infosender;
                netGamer.ReceiveData(StateManager.NetworkData.DataReader, out infosender);

                MultiplayerTeam? mpTeam = null;
                if (StateManager.NetworkData.SessionMode == MultiplayerSessionType.TDM || StateManager.NetworkData.SessionMode == MultiplayerSessionType.Coop)
                {
                    mpTeam = (MultiplayerTeam)Enum.Parse(typeof(MultiplayerTeam), StateManager.NetworkData.DataReader.ReadInt32().ToString(), true);
                }

                Vector4 ship = StateManager.NetworkData.DataReader.ReadVector4();

                NetworkPlayerID targetPlayer = new NetworkPlayerID(Guid.NewGuid());
                if (StateManager.NetworkData.DataReader.ReadBoolean())
                {
                    targetPlayer = new NetworkPlayerID(StateManager.NetworkData.DataReader.ReadByte());
                }

                ships.Add(targetPlayer, new KeyValuePair<MultiplayerTeam?, Vector4>(mpTeam, ship));
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
            if (Gamer.SignedInGamers[PlayerIndex.One].Privileges.AllowCommunication != GamerPrivilegeSetting.Everyone)
            {
                if (Gamer.SignedInGamers[PlayerIndex.One].Privileges.AllowCommunication == GamerPrivilegeSetting.Blocked || (Gamer.SignedInGamers[PlayerIndex.One].Privileges.AllowCommunication == GamerPrivilegeSetting.FriendsOnly && !Gamer.SignedInGamers[PlayerIndex.One].IsFriend(e.Gamer)))
                {
                    StateManager.NetworkData.CurrentSession.LocalGamers[0].EnableSendVoice(e.Gamer, false);
                }

            }

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
                TextSprite gamerInfo = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.NormalText, text, Color.White);
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
            Sprites.Clear();
            AdditionalSprites.Clear();
            _firstInit = true;
            gamerInfos = null;
            GamerInfos.Clear();
            SelectedShips.Clear();

            base.InitScreen(screenName);

            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            title = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.BoldText, StateManager.NetworkData.SessionMode.ToFriendlyString() + " Lobby", Color.White);
            title.Y = 5;
            title.X = title.GetCenterPosition(Graphics.Viewport).X;
            AdditionalSprites.Add(title);

            StartButton = new Sprite(GameContent.Assets.Images.Controls.Button, new Vector2(0, Graphics.Viewport.Height), Sprites.SpriteBatch);
            StartButton.X = StartButton.GetCenterPosition(Graphics.Viewport).X;
            StartButton.Y -= StartButton.Height + 20;
            StartLabel = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.NormalText, "Start", Color.White) { ParentSprite = StartButton, IsHoverable = true, HoverColor = Color.MediumAquamarine, NonHoverColor = Color.White };
            StartLabel.Pressed += new EventHandler(StartLabel_Pressed);
            StartButton.Visible = false;
            StartLabel.Visible = false;
            Sprites.Add(StartButton);
            AdditionalSprites.Add(StartLabel);

#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { { StartLabel } }, InputType.LeftJoystick);
            AllButtons.FireTextSpritePressed = true;
#endif
        }


        #region Host-specific Variables
        int redTeamCount;
        int blueTeamCount;
        #endregion

        /// <summary>
        /// Start label pressed event handler.
        /// </summary>
        /// <remarks>
        /// Called when the HOST presses the start label.
        /// </remarks>
        void StartLabel_Pressed(object sender, EventArgs e)
        {
            redTeamCount = 0;
            blueTeamCount = 0;
            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }
            //if (StateManager.NetworkData.SessionMode == MultiplayerSessionType.LMS || StateManager.NetworkData.SessionMode == MultiplayerSessionType.TDM)
            //{
            StateManager.SelectedShip = SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Type;

            StateManager.InitializeSingleplayerGameScreen(SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Type, SelectedShips[StateManager.NetworkData.CurrentSession.LocalGamers[0].Id].Tier, false);

            GameScreen game = StateManager.GetScreen<GameScreen>(CoreTypes.ScreenType.Game);

            Ship my = game.playerShip;

            MultiplayerTeam? hostTeam = null;

            if (StateManager.NetworkData.SessionMode == MultiplayerSessionType.TDM || StateManager.NetworkData.SessionMode == MultiplayerSessionType.Coop)
            {
                //Random teams
                hostTeam = RandomTeamGenerator.GenerateTeam(ref redTeamCount, ref blueTeamCount);

                StateManager.NetworkData.DataWriter.Write(hostTeam.Value.ToInt());
            }

            float lastHostX;
            if (StateManager.NetworkData.SessionMode == MultiplayerSessionType.Coop)
            {
                my.WorldCoords = new Vector2(StateManager.SpawnArea.X+my.Width*1.875f, my.WorldCoords.Y);
            }
            lastHostX = my.WorldCoords.X;
            StateManager.NetworkData.DataWriter.Write(new Vector4(my.WorldCoords.X, my.WorldCoords.Y, my.Rotation.Radians, my.CurrentHealth));
            StateManager.NetworkData.DataWriter.Write(true);
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
                if (StateManager.NetworkData.SessionMode == MultiplayerSessionType.Coop)
                {
                    sns.WorldCoords = new Vector2(lastHostX + my.Width, my.WorldCoords.Y);
                }

                bool isAlly = false;

                if (StateManager.NetworkData.SessionMode == MultiplayerSessionType.TDM || StateManager.NetworkData.SessionMode == MultiplayerSessionType.Coop)
                {
                    //Random team
                    MultiplayerTeam team = StateManager.NetworkData.SessionMode == MultiplayerSessionType.Coop ? hostTeam.Value : RandomTeamGenerator.GenerateTeam(ref redTeamCount, ref blueTeamCount);
                    isAlly = hostTeam.Value == team;
                    StateManager.NetworkData.DataWriter.Write(team.ToInt());
                }

                StateManager.NetworkData.DataWriter.Write(new Vector4(sns.WorldCoords.X, sns.WorldCoords.Y, sns.Rotation.Radians, sns.CurrentHealth));
                StateManager.NetworkData.DataWriter.Write(true);
                StateManager.NetworkData.DataWriter.Write(g.Id);

                if (isAlly)
                {
                    sns.PlayerType = PlayerType.Ally;
                    StateManager.AllyShips.Add(sns);
                }
                else
                {
                    StateManager.EnemyShips.Add(sns);
                }
                StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(sns);
                StateManager.NetworkData.CurrentSession.LocalGamers[0].SendData(StateManager.NetworkData.DataWriter, SendDataOptions.Reliable);
            }

            if (StateManager.NetworkData.SessionMode == MultiplayerSessionType.Coop)
            {
                for (int i = 0; i < 4; i++)
                {

                    BaseEnemyShip enemy = BaseEnemyShip.CreateRandomEnemy(GameScreen.World);
                    game.Sprites.Add(enemy);
                    game.enemies.Add(enemy);

                    StateManager.NetworkData.DataWriter.Write(RandomTeamGenerator.EnemyOf(hostTeam.Value).ToInt());


                    StateManager.NetworkData.DataWriter.Write(new Vector4(enemy.WorldCoords.X, enemy.WorldCoords.Y, enemy.Rotation.Radians, enemy.CurrentHealth));
                    //StateManager.NetworkData.DataWriter.Write(g.Id);
                    StateManager.NetworkData.DataWriter.Write(false);
                    StateManager.EnemyShips.Add(enemy);

                    //StateManager.AllScreens[ScreenType.Game.ToString()].Sprites.Add(sns);
                    StateManager.NetworkData.CurrentSession.LocalGamers[0].SendData(StateManager.NetworkData.DataWriter, SendDataOptions.Reliable);
                }
            }



            StateManager.NetworkData.CurrentSession.StartGame();

            StateManager.CurrentLevel = GameLevel.Level1;
            StateManager.ScreenState = CoreTypes.ScreenType.Game;
            //}
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
