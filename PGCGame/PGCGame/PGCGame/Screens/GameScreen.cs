using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Glib.XNA.SpriteLib;
using Glib.XNA;
using Glib;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using PGCGame.CoreTypes;
using System.Diagnostics;

using PGCGame.Ships.Enemies;
using PGCGame.Ships.Allies;
using Glib.XNA.InputLib;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.GamerServices;

namespace PGCGame.Screens
{
    public class GameScreen : BaseScreen
    {
        public static SpriteBatch World;

        private Vector2 _playableAreaOffset;
#if WINDOWS
        public static event EventHandler Paused;
#endif
        public static readonly ScreenType[] ScreensToAllowMusicProcessing = new ScreenType[] { ScreenType.Game, ScreenType.Options, ScreenType.Pause, ScreenType.Shop, ScreenType.UpgradeScreen, ScreenType.WeaponSelect };

        public GameScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            worldCam = new Camera2DMatrix();
            playerSb = new SpriteBatch(spriteBatch.GraphicsDevice);

            _playableAreaOffset = new Vector2(500);
            ClonesMade = GameContent.Assets.Sound[SoundEffectType.CloneMade];

#if XBOX
            GamePadManager.One.Buttons.StartButtonPressed += new EventHandler(delegate(object src, EventArgs e){
                if (Visible)
                {
                    StateManager.ScreenState = CoreTypes.ScreenType.Pause;
                }
            });
#endif
        }

        /// <summary>
        /// The amount to divide the background size by to generate the minimap size.
        /// </summary>
        public const int MinimapDivAmount = 45;

        internal List<BaseEnemyShip> enemies = new List<BaseEnemyShip>();
        internal BaseAllyShip playerShip;
        SpriteBatch playerSb;
        SpriteFont normal;
        SpriteFont bold;
        Texture2D bgImg;
        SpriteFont SegoeUIMono = GameContent.Assets.Fonts.NormalText;
        List<ISprite> playerSbObjects = new List<ISprite>();


        public override void InitScreen(ScreenType screenType)
        {
            base.InitScreen(screenType);

            bold = GameContent.Assets.Fonts.BoldText;
            normal = GameContent.Assets.Fonts.NormalText;

            StateManager.Options.ScreenResolutionChanged += new EventHandler<ViewportEventArgs>(Options_ScreenResolutionChanged);

            StateManager.NetworkData.DataReceived += new EventHandler<Glib.XNA.NetworkLib.NetworkInformationReceivedEventArgs>(NetworkData_DataReceived);
            bgImg = GameContent.Assets.Images.Backgrounds.Levels[GameLevel.Level1];
        }

        private Dictionary<byte, Bullet> _bulletsInProgress = new Dictionary<byte, Bullet>();

        void NetworkData_DataReceived(object sender, Glib.XNA.NetworkLib.NetworkInformationReceivedEventArgs e)
        {
            if (!Visible)
            {
                return;
            }

            string[] propNameComponents = e.Data.Name.Split('.');

            if (propNameComponents[0].Equals("NewBullet", StringComparison.InvariantCultureIgnoreCase))
            {
                #region Bullet Handling

                if (propNameComponents[1].Equals("PosSpeed", StringComparison.InvariantCultureIgnoreCase))
                {
                    Vector4 bulletData = (Vector4)e.Data.Data;
                    BaseAllyShip parent = null;
                    try
                    {
                        parent = StateManager.EnemyShips[e.Data.Sender];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        parent = StateManager.AllyShips[e.Data.Sender];
                    }
                    Bullet newBullet = StateManager.BulletPool.GetBullet();
                    newBullet.InitializePooledBullet(new Vector2(bulletData.X, bulletData.Y), parent);
                    newBullet.SpriteBatch = World;
                    newBullet.Speed = new Vector2(bulletData.Z, bulletData.W);
                    newBullet.MaximumDistance = new Vector2(4000f);
                    _bulletsInProgress[e.Data.Sender.Id] = newBullet;
                }
                else if (propNameComponents[1].Equals("FinalData", StringComparison.InvariantCultureIgnoreCase))
                {
                    Vector4 addlData = (Vector4)e.Data.Data;
                    Bullet newBullet = _bulletsInProgress[e.Data.Sender.Id];
                    newBullet.Rotation = SpriteRotation.FromRadians(addlData.Y);
                    newBullet.Damage = addlData.X.ToInt();

                    //Debug.WriteLine("Bullet received from {0}: X: {1}, Y: {2}", newBullet.ParentShip.Controller.Gamertag, newBullet.X, newBullet.Y);
                    (newBullet.ParentShip.PlayerType == PlayerType.Ally || newBullet.ParentShip.PlayerType == PlayerType.MyShip ? StateManager.AllyBullets : StateManager.EnemyBullets).Legit.Add(newBullet);
                    _bulletsInProgress.Remove(e.Data.Sender.Id);
                }
                #endregion
            }

            if (propNameComponents[0].Equals("MPShip", StringComparison.InvariantCultureIgnoreCase))
            {
                #region Ship Handling
                if (propNameComponents[1].Equals("CurrentShipState", StringComparison.InvariantCultureIgnoreCase))
                {
                    Vector4 shipData = (Vector4)e.Data.Data;
                    foreach (Ship s in StateManager.EnemyShips)
                    {
                        BaseAllyShip sh = s as BaseAllyShip;
                        if (sh != null && sh.Controller != null && sh.Controller.Id == e.Data.Sender.Id)
                        {
                            Debug.WriteLine("Received ship data from {0}: [{1}, {2}], health {3}", e.Data.Sender.Gamertag, shipData.X, shipData.Y, shipData.W);
                            sh.WorldCoords = new Vector2(shipData.X, shipData.Y);
                            sh.Rotation = SpriteRotation.FromRadians(shipData.Z);
                            sh.CurrentHealth = shipData.W.ToInt();
                            break;
                        }
                    }
                    foreach (Ship s in StateManager.AllyShips)
                    {
                        BaseAllyShip sh = s as BaseAllyShip;
                        if (sh != null && sh.Controller != null && sh.Controller.Id == e.Data.Sender.Id)
                        {
                            Debug.WriteLine("Received ship data from {0}: [{1}, {2}], health {3}", e.Data.Sender.Gamertag, shipData.X, shipData.Y, shipData.W);
                            sh.WorldCoords = new Vector2(shipData.X, shipData.Y);
                            sh.Rotation = SpriteRotation.FromRadians(shipData.Z);
                            sh.CurrentHealth = shipData.W.ToInt();
                            break;
                        }
                    }
                }
                #endregion
            }
        }

        private MusicBehaviour _music = new MusicBehaviour(ScreenMusic.Level1, true);

        public override MusicBehaviour Music
        {
            get { return _music; }
        }

        void Options_ScreenResolutionChanged(object sender, ViewportEventArgs e)
        {
            if (playerShip != null)
            {
                playerShip.Position = playerShip.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport, playerShip.Origin * playerShip.Scale);
            }
            if (miniMap != null)
            {
                miniMap.Y = e.Viewport.TitleSafeArea.Y + 7.5f;
                miniMap.X = e.Viewport.TitleSafeArea.X + e.Viewport.TitleSafeArea.Width - miniMap.Width - 7.5f;
                miniShipInfoBg.X = miniMap.X - miniShipInfoBg.Width - 7.5f;
            }
            if (fogOfWar != null)
            {

                for (int row = 0; row <= fogOfWar.GetUpperBound(1); row++)
                {
                    for (int column = 0; column <= fogOfWar.GetUpperBound(0); column++)
                    {
                        fogOfWar[column, row].Width = miniMap.Width / fogOfWar.GetLength(0);
                        fogOfWar[column, row].Height = miniMap.Height / fogOfWar.GetLength(1);
                        fogOfWar[column, row].X = miniMap.X + fogOfWar[0, 0].Width * column;
                        fogOfWar[column, row].Y = miniMap.Y + fogOfWar[0, 0].Height * row;
                        fogOfWar[column, row].Color = Color.White;
                    }
                }
            }
        }


        Sprite miniMap;
        Sprite[,] fogOfWar;

        TextSprite secondaryWeaponLabel;
        BackgroundSprite bgspr;


        public void InitializeScreen(ShipStats ship, bool spawnEnemies)
        {
            //Reset any active ships, since we're re-initializing the game screen
            StateManager.EnemyShips.Clear();
            StateManager.AllyShips.Clear();
            playerMinimapVisible = null;
            _minimapYou = null;

            //Reset music
            _gameHasStarted = false;
            //_allowMusicHandling = false;
            playerSbObjects.Clear();
            Sprites.Clear();
            enemies.Clear();

            wcMovePrettyCode = new EventHandler(wcMovePreUpdate);

            SpriteFont SegoeUIMono = GameContent.Assets.Fonts.NormalText;

            secondaryWeaponLabel = new TextSprite(playerSb, SegoeUIMono, "No Secondary Weapon");
            secondaryWeaponLabel.Position = new Vector2((Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .1f - secondaryWeaponLabel.Width / 2) + 30, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f - secondaryWeaponLabel.Height / 2);
            secondaryWeaponLabel.Color = Color.White;
            playerSbObjects.Add(secondaryWeaponLabel);


            //Initialize the game map (world)
            bgspr = new BackgroundSprite(bgImg, Sprites.SpriteBatch, 10, 2);
            bgspr.Drawn += new EventHandler(bgspr_Drawn);
            worldCam.Pos = new Vector2(bgspr.TotalWidth / 2, bgspr.TotalHeight - (bgspr.Height / 2));
            BackgroundSprite = bgspr;

            //Set the world size in StateManager for later access
            StateManager.WorldSize = new Rectangle(0, 0, (int)bgspr.TotalWidth, (int)bgspr.TotalHeight);

            StateManager.SpawnArea = new Rectangle(500, 500, bgspr.TotalWidth.Round() - 1000, bgspr.TotalHeight.Round() - 1000);

            Vector2 minSpawnArea = _playableAreaOffset;
            Vector2 maxSpawnArea = new Vector2(bgspr.TotalWidth, bgspr.TotalHeight) - _playableAreaOffset;

            _enemies = spawnEnemies;

            #region Enemy Spawns
            if (spawnEnemies)
            {

                if (StateManager.CurrentLevel.ToInt() == 4)
                {
                    foreach (SignedInGamer sig in Gamer.SignedInGamers)
                    {
                        sig.Presence.PresenceMode = GamerPresenceMode.BattlingBoss;
                    }
                    CloneBoss enemyBoss = new CloneBoss(GameContent.Assets.Images.Ships[ShipType.EnemyFighterCarrier, ShipTier.Tier4], Vector2.Zero, Sprites.SpriteBatch);
                    CloneBoss enemyCloneOne = new CloneBoss(GameContent.Assets.Images.Ships[ShipType.EnemyFighterCarrier, ShipTier.Tier4], Vector2.Zero, Sprites.SpriteBatch);
                    CloneBoss enemyCloneTwo = new CloneBoss(GameContent.Assets.Images.Ships[ShipType.EnemyFighterCarrier, ShipTier.Tier4], Vector2.Zero, Sprites.SpriteBatch);

                    enemyBoss.WorldCoords = StateManager.RandomGenerator.NextVector2(minSpawnArea, maxSpawnArea);
                    enemyCloneOne.WorldCoords = enemyBoss.WorldCoords;
                    enemyCloneTwo.WorldCoords = enemyBoss.WorldCoords;

                    enemyBoss.isClone = false;

                    enemyBoss.targetPosition = StateManager.RandomGenerator.NextVector2(new Vector2(500), new Vector2(StateManager.WorldSize.Width - 500, StateManager.WorldSize.Height - 500));
                    enemyCloneOne.targetPosition = StateManager.RandomGenerator.NextVector2(new Vector2(500), new Vector2(StateManager.WorldSize.Width - 500, StateManager.WorldSize.Height - 500));
                    enemyCloneTwo.targetPosition = StateManager.RandomGenerator.NextVector2(new Vector2(500), new Vector2(StateManager.WorldSize.Width - 500, StateManager.WorldSize.Height - 500));

                    enemyCloneOne.EnemyCounts = false;
                    enemyCloneTwo.EnemyCounts = false;

                    Sprites.Add(enemyBoss);
                    enemies.Add(enemyBoss);

                    if (StateManager.Options.SFXEnabled)
                    {
                        ClonesMade.Play();
                    }

                    Sprites.Add(enemyCloneOne);
                    enemies.Add(enemyCloneOne);

                    if (StateManager.Options.SFXEnabled)
                    {
                        ClonesMade.Play();
                    }

                    Sprites.Add(enemyCloneTwo);
                    enemies.Add(enemyCloneTwo);
                }
                else
                {
                    foreach (SignedInGamer sig in Gamer.SignedInGamers)
                    {
                        sig.Presence.PresenceMode = GamerPresenceMode.InCombat;
                    }
                    for (int i = 0; i < 4 * StateManager.CurrentLevel.ToInt(); i++)
                    {
                        Texture2D enemyTexture = GameContent.Assets.Images.Ships[ShipType.Drone, StateManager.RandomGenerator.NextShipTier(ShipTier.Tier1, ShipTier.Tier2)];
                        EnemyBattleCruiser enemy = new EnemyBattleCruiser(GameContent.Assets.Images.Ships[ShipType.EnemyBattleCruiser, ShipTier.Tier1], Vector2.Zero, Sprites.SpriteBatch);

                        enemy.WorldCoords = StateManager.RandomGenerator.NextVector2(minSpawnArea, maxSpawnArea);

                        enemy.DistanceToNose = .5f;

                        enemy.Tier = ShipTier.Tier1;

                        Sprites.Add(enemy);
                        enemies.Add(enemy);
                    }
                }
            }
            #endregion

            TextureFactory creator = new TextureFactory(Graphics);

            miniMap = new Sprite(creator.CreateSquare(1, new Color(Color.Navy.R, Color.Navy.G, Color.Navy.B, 128)), Vector2.Zero, playerSb);
            miniMap.Width = bgspr.TotalWidth / MinimapDivAmount;
            miniMap.Color = Color.White;
            miniMap.Height = bgspr.TotalHeight / MinimapDivAmount;
            miniMap.Y = Graphics.Viewport.TitleSafeArea.Y + 7.5f;
            miniMap.X = Graphics.Viewport.TitleSafeArea.X + Graphics.Viewport.TitleSafeArea.Width - miniMap.Width - 7.5f;
            miniMap.Updated += new EventHandler(miniMap_Updated);
            miniShipInfoBg = new Sprite(creator.CreateSquare(1, new Color(0, 0, 0, 192)), new Vector2(7.5f, miniMap.Y), playerSb);
            miniShipInfoBg.Height = 0.01f;
            miniShipInfoBg.Width = 767.5f - miniShipInfoBg.X - 7.5f - miniMap.Width - 266.6666667f;
            miniShipInfoBg.X = miniMap.X - miniShipInfoBg.Width - 7.5f;
            miniShipInfoBg.Color = Color.Transparent;
            playerSbObjects.Add(miniShipInfoBg);
            playerSbObjects.Add(miniMap);

            if (StateManager.DebugData.FogOfWarEnabled)
            {
                //Create fog of war array
                // > 9x15 = Xbox lag
                // TODO: RenderTarget conversion
                fogOfWar = new Sprite[9, 15];
            }



            if (ship.Type == ShipType.Drone)
            {
                throw new Exception("Can't create a Drone as the main ship");
            }

            playerShip = BaseAllyShip.CreateShip(ship, playerSb);

            playerShip.WorldSb = Sprites.SpriteBatch;
            playerShip.Position = playerShip.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport, playerShip.Origin * playerShip.Scale);
            playerShip.WCMoved += new EventHandler(playerShip_WCMoved);
            playerShip.WCMoved += wcMovePrettyCode;
            playerShip.IsPlayerShip = true;
            playerShip.RotateTowardsMouse = true;
            playerSbObjects.Add(playerShip);

            //TODO: Tier change event handles this
            //playerShip.InitialHealth = 100;

            //Set as own ship
            playerShip.PlayerType = PlayerType.MyShip;
            playerShip.WorldCoords = worldCam.Pos;


            if (fogOfWar != null)
            {
                for (int row = 0; row <= fogOfWar.GetUpperBound(1); row++)
                {
                    for (int column = 0; column <= fogOfWar.GetUpperBound(0); column++)
                    {
                        fogOfWar[column, row] = new Sprite(creator.CreateSquare(1, Color.DarkGray), Vector2.Zero, playerSb);
                        fogOfWar[column, row].Width = miniMap.Width / fogOfWar.GetLength(0);
                        fogOfWar[column, row].Height = miniMap.Height / fogOfWar.GetLength(1);
                        fogOfWar[column, row].X = miniMap.X + fogOfWar[0, 0].Width * column;
                        fogOfWar[column, row].Y = miniMap.Y + fogOfWar[0, 0].Height * row;
                        fogOfWar[column, row].Color = Color.White;
                        StateManager.KnownMap[column, row] = false;
                    }
                }
            }

            World = Sprites.SpriteBatch;
            playerShip.BulletFired += new EventHandler<BulletEventArgs>(playerShip_BulletFired);



            _bulletsInProgress.Clear();

            foreach (Ship s in enemies)
            {
                s.particles.Add(GameContent.Assets.Images.particles[Particle.Circle]);
                s.particles.Add(GameContent.Assets.Images.particles[Particle.Square]);

                s.engine = new CoreTypes.Utilites.ParticleEngine(s.particles, new Vector2(s.Position.X - s.Width, s.Position.Y - s.Height));
            }

            playerShip.particles.Add(GameContent.Assets.Images.particles[Particle.Circle]);
            playerShip.particles.Add(GameContent.Assets.Images.particles[Particle.Square]);

            playerShip.engine = new CoreTypes.Utilites.ParticleEngine(playerShip.particles, new Vector2(playerShip.Position.X - playerShip.Width, playerShip.Position.Y - playerShip.Height));

        }

        /// <summary>
        /// Called when the player fires a bullet.
        /// </summary>
        /// <remarks>
        /// Currently only houses multiplayer bullet sending code.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void playerShip_BulletFired(object sender, BulletEventArgs e)
        {
            if (StateManager.NetworkData.IsMultiplayer)
            {
                //Send bullet

                //IsBullet is true
                e.Bullet.MaximumDistance = new Vector2(4000f);
                StateManager.NetworkData.SendData("NewBullet.PosSpeed", new Vector4(e.BulletPosition, e.BulletSpeed.X, e.BulletSpeed.Y));
                StateManager.NetworkData.SendData("NewBullet.FinalData", new Vector4(e.Bullet.Damage, e.BulletRotation, e.Bullet.ParentShip.ShipType.ToInt(), e.Bullet.ParentShip.Tier.ToInt()));
                StateManager.NetworkData.WriteNetworkData(SendDataOptions.None, null);

            }
        }

        void playerShip_WCMoved(object sender, EventArgs e)
        {
            if (_minimapYou != null)
            {
                playerMinimapVisible = new Rectangle((_minimapYou.X - (Sprites.SpriteBatch.GraphicsDevice.Viewport.Width / 2) / MinimapDivAmount).ToInt(), (_minimapYou.Y - (Sprites.SpriteBatch.GraphicsDevice.Viewport.Height / 2) / MinimapDivAmount).ToInt(), (Sprites.SpriteBatch.GraphicsDevice.Viewport.Width / MinimapDivAmount).ToInt(), (Sprites.SpriteBatch.GraphicsDevice.Viewport.Height / MinimapDivAmount).ToInt());
            }
        }

        Sprite miniShipInfoBg;
        TextSprite miniShipInfoTitle = null;
        TextSprite miniShipInfo = null;
        List<Sprite> miniShips = new List<Sprite>();

        public void ResetLastKS(params Keys[] allKeys)
        {
            _lastState = new KeyboardState(allKeys);
        }

        Vector2? bossWorldCoords;

        public void RegenerateClones()
        {
            if (StateManager.Options.SFXEnabled)
            {
                ClonesMade.Play();
            }
            bossWorldCoords = null;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] is CloneBoss)
                {
                    CloneBoss enemy = enemies[i] as CloneBoss;
                    if (enemy.isClone)
                    {
                        enemy.CurrentHealth = 0;
                        StateManager.EnemyShips.Remove(enemy);
                        Sprites.Remove(enemy);
                        enemies.RemoveAt(i);
                    }
                    else
                    {
                        bossWorldCoords = enemy.WorldCoords;
                        enemy.targetPosition = StateManager.RandomGenerator.NextVector2(new Vector2(500), new Vector2(StateManager.WorldSize.Width - 500, StateManager.WorldSize.Height - 500));
                    }
                }
            }
            CloneBoss enemyCloneOne = new CloneBoss(GameContent.Assets.Images.Ships[ShipType.EnemyFighterCarrier, ShipTier.Tier4], Vector2.Zero, Sprites.SpriteBatch);
            CloneBoss enemyCloneTwo = new CloneBoss(GameContent.Assets.Images.Ships[ShipType.EnemyFighterCarrier, ShipTier.Tier4], Vector2.Zero, Sprites.SpriteBatch);

            if (bossWorldCoords.HasValue)
            {
                enemyCloneOne.WorldCoords = bossWorldCoords.Value;
                enemyCloneTwo.WorldCoords = bossWorldCoords.Value;
            }
            enemyCloneOne.targetPosition = StateManager.RandomGenerator.NextVector2(new Vector2(500), new Vector2(StateManager.WorldSize.Width - 500, StateManager.WorldSize.Height - 500));
            enemyCloneTwo.targetPosition = StateManager.RandomGenerator.NextVector2(new Vector2(500), new Vector2(StateManager.WorldSize.Width - 500, StateManager.WorldSize.Height - 500));

            enemyCloneOne.noStackDelay = new TimeSpan(0);
            enemyCloneTwo.noStackDelay = new TimeSpan(0);

            enemyCloneOne.EnemyCounts = false;
            enemyCloneTwo.EnemyCounts = false;

            Sprites.Add(enemyCloneOne);
            enemies.Add(enemyCloneOne);

            Sprites.Add(enemyCloneTwo);
            enemies.Add(enemyCloneTwo);


        }

        bool _enemies = true;

        void miniMap_Updated(object sender, EventArgs e)
        {
            foreach (Sprite s in miniShips)
            {
                playerSbObjects.Remove(s);
            }
            if (miniShipInfoTitle != null)
            {
                playerSbObjects.Remove(miniShipInfoTitle);
            }
            if (miniShipInfo != null)
            {
                playerSbObjects.Remove(miniShipInfo);
            }
            miniShips.Clear();

            Ship activeMiniShipDisplay = null;

            foreach (Ship ship in StateManager.AllyShips)
            {
                addShipToMinimap(ship, ref activeMiniShipDisplay);
            }

            foreach (Ship ship in StateManager.EnemyShips)
            {
                addShipToMinimap(ship, ref activeMiniShipDisplay);
            }

            miniShipInfoBg.Color = activeMiniShipDisplay != null ? Color.White : Color.Transparent;
            if (activeMiniShipDisplay != null)
            {
                miniShipInfoTitle = new TextSprite(playerSb, bold, activeMiniShipDisplay.FriendlyName);
                miniShipInfoTitle.Color = Color.White;
                miniShipInfoTitle.Position = new Vector2(miniShipInfoBg.X + (miniShipInfoBg.Width / 2f) - (miniShipInfoTitle.Width / 2f), miniShipInfoBg.Y + (miniShipInfoBg.Height / 12.5f));
                playerSbObjects.Add(miniShipInfoTitle);
                miniShipInfoBg.Height = (miniShipInfoTitle.Y - miniShipInfoBg.Y) + miniShipInfoTitle.Height + (miniShipInfoTitle.Y - miniShipInfoBg.Y);
                string shipIdStr = activeMiniShipDisplay.ShipID.ToString();
                string[] digits = shipIdStr.Split('-');
                shipIdStr = digits[0] + "..." + digits[digits.Length - 1];
                miniShipInfo = new TextSprite(playerSb, normal, string.Format("Tier {4}\nHP: {0}/{1}\nDamage: {2}{3}", activeMiniShipDisplay.CurrentHealth, activeMiniShipDisplay.InitialHealth, activeMiniShipDisplay.DamagePerShot, StateManager.DebugData.ShowShipIDs ? "\nID: " + shipIdStr : "", activeMiniShipDisplay.Tier.ToInt()));
                miniShipInfo.Color = Color.White;
                miniShipInfo.Position = new Vector2(miniShipInfoBg.X + (miniShipInfoBg.Width / 2f) - (miniShipInfo.Width / 2f), miniShipInfoTitle.Y + bold.LineSpacing);
                if (StateManager.ShowShipData || StateManager.DebugData.ShowShipIDs)
                {
                    miniShipInfoBg.Height += miniShipInfo.Height;
                    playerSbObjects.Add(miniShipInfo);
                }

            }

            //This extension method (IEnumerable cast, NOT Glib cast) shouldn't be neccesary on XBOX, but I think it may be
            //Casting the Sprites to ISprites
            playerSbObjects.AddRange(miniShips.Cast<ISprite>());

            if (fogOfWar != null)
            {
                for (int row = 0; row <= fogOfWar.GetUpperBound(1); row++)
                {
                    for (int column = 0; column <= fogOfWar.GetUpperBound(0); column++)
                    {
                        playerSbObjects.Remove(fogOfWar[column, row]);
                        if (!StateManager.KnownMap[column, row])
                        {
                            playerSbObjects.Add(fogOfWar[column, row]);
                        }
                    }
                }
            }

        }

        Rectangle? playerMinimapVisible = null;

        private Sprite _minimapYou = null;

        private void wcMovePreUpdate(object o, EventArgs ea)
        {
            worldCam.Pos = playerShip.WorldCoords;
        }

        private void addShipToMinimap(Ship ship, ref Ship activeMiniShipDisplay)
        {
            if (ship.GetType() == typeof(Drone) || ship.ShipState == ShipState.Exploding || ship.ShipState == ShipState.Dead)
            {
                return;
            }

            Sprite miniShip = new Sprite(GameContent.Assets.Images.MiniShips[ship.ShipType], miniMap.Position + (ship.WorldCoords / MinimapDivAmount), playerSb);
            if (StateManager.NetworkData.IsMultiplayer && StateManager.NetworkData.SessionMode == MultiplayerSessionType.TDM)
            {
                miniShip.Texture = GameContent.Assets.Images.MiniShips[ship.PlayerType];
            }

            miniShip.Scale = ship.PlayerType == PlayerType.MyShip || ship.PlayerType == PlayerType.Ally ? new Vector2(.1f) : new Vector2(.07f);
            miniShip.Color = ship.PlayerType == PlayerType.MyShip || ship.PlayerType == PlayerType.Ally ? Color.Green : Color.Red;
            if (StateManager.ShowShipData)
            {
                miniShip.Rotation = ship.Rotation;
            }
            miniShip.UseCenterAsOrigin = true;
            miniShips.Add(miniShip);

            if (ship.PlayerType == PlayerType.MyShip)
            {
                _minimapYou = miniShip;
            }

#if WINDOWS
            //TODO: Minimap ship info showing up on Xbox
            if (miniShip.Intersects(MouseManager.CurrentMouseState) && activeMiniShipDisplay == null)
            {
                bool intersectsFOW = false;

                if (fogOfWar != null)
                {
                    for (int row = 0; row <= StateManager.KnownMap.GetUpperBound(1); row++)
                    {
                        for (int column = 0; column <= StateManager.KnownMap.GetUpperBound(0); column++)
                        {
                            intersectsFOW = !StateManager.KnownMap[column, row] && fogOfWar[column, row].Intersects(miniShip);
                            if (intersectsFOW)
                            {
                                break;
                            }
                        }

                        if (intersectsFOW)
                        {
                            break;
                        }
                    }
                }

                if (!intersectsFOW)
                {
                    activeMiniShipDisplay = ship;
                }
            }
#endif
        }

        void bgspr_Drawn(object sender, EventArgs e)
        {
            StateManager.AllyBullets.DrawAll(Sprites.SpriteBatch);
            StateManager.EnemyBullets.DrawAll(Sprites.SpriteBatch);

            foreach (SecondaryWeapon activeWeapon in playerShip.ActiveSecondaryWeapons)
            {

                if (activeWeapon != null && activeWeapon.ShouldDraw)
                {
                    switch (activeWeapon.GetType().FullName)
                    {
                        case "PGCGame.SpaceMine":
                            Sprites.SpriteBatch.Draw(activeWeapon);
                            break;
                        case "PGCGame.EMP":
                            Sprites.SpriteBatch.Draw(activeWeapon);
                            break;
                        case "PGCGame.ShrinkRay":
                            foreach (Bullet b in activeWeapon.Cast<ShrinkRay>().ShrinkRayBullets)
                            {
                                b.Update();
                                Sprites.SpriteBatch.Draw(b);
                            }
                            break;
                    }
                }
            }
        }

        bool _gameHasStarted = false;
        //bool _allowMusicHandling = false;
        KeyboardState _lastState = new KeyboardState();

        Boolean allEnemiesDead = true;
        EventHandler wcMovePrettyCode;

        public override void Update(GameTime gameTime)
        {
            miniMap.Update();

            if (!_gameHasStarted)
            {
                if (playerShip.WorldCoords == Vector2.Zero)
                {
                    //This is the temporary workaround
                    playerShip.WorldCoords = worldCam.Pos;
                }
            }

            base.Update(gameTime);


            if (fogOfWar != null)
            {
                for (int row = 0; row <= fogOfWar.GetUpperBound(1); row++)
                {
                    for (int column = 0; column <= fogOfWar.GetUpperBound(0); column++)
                    {
                        if (playerMinimapVisible.HasValue)
                        {
                            if (fogOfWar[column, row].Intersects(playerMinimapVisible.Value))
                            {
                                StateManager.KnownMap[column, row] = true;
                            }
                        }
                    }
                }
            }

            //KeyboardState keyboard = Keyboard.GetState();

            allEnemiesDead = true;

            if (_enemies)
            {
                foreach (BaseEnemyShip enemyShip in enemies)
                {
#if WINDOWS
                    if (StateManager.DebugData.KillAll && _lastState.IsKeyUp(Keys.F7) && KeyboardManager.State.IsKeyDown(Keys.F7))
                    {
                        enemyShip.CurrentHealth = (KeyboardManager.State.IsKeyDown(Keys.LeftAlt) || KeyboardManager.State.IsKeyDown(Keys.RightAlt)) && enemyShip.CurrentHealth > 0 ? 1 : 0;
                    }
#endif
                    if (enemyShip.ShipState != ShipState.Dead && enemyShip.EnemyCounts)
                    {
                        allEnemiesDead = false;
                    }
                }
            }
            else
            {
                foreach (Ship enemyShip in StateManager.EnemyShips)
                {
                    if (enemyShip.ShipState != ShipState.Dead && enemyShip.ShipState != ShipState.Exploding)
                    {
                        allEnemiesDead = false;
                    }
                    else if (enemyShip.TimeDead <= MPEnemyDeadTime)
                    {
                        allEnemiesDead = false;
                    }
                }


            }

            if ((_lastState.IsKeyUp(Keys.CapsLock) || _lastState.IsKeyUp(Keys.N)) && (KeyboardManager.State.IsKeyDown(Keys.CapsLock) && KeyboardManager.State.IsKeyDown(Keys.N)))
            {
                inNightMode = !inNightMode;
                rastState = inNightMode ? new RasterizerState() { DepthBias = 125, FillMode = FillMode.WireFrame, ScissorTestEnable = true } : RasterizerState.CullCounterClockwise;
            }

            if (playerShip.ShipState == ShipState.Exploding)
            {
                bgspr.Color = Color.Red;
                if (!StateManager.NetworkData.IsMultiplayer && StateManager.Lives > 0)
                {
                    secondaryWeaponLabel.Text = String.Format("         You Died!!!\nYou Have {0} Extra Lives Remaining", StateManager.Lives - 1);
                    secondaryWeaponLabel.Position = new Vector2(StateManager.GraphicsManager.GraphicsDevice.Viewport.Width * .3f, StateManager.GraphicsManager.GraphicsDevice.Viewport.Height * .1f);
                }
                else if (!StateManager.NetworkData.IsMultiplayer)
                {
                    secondaryWeaponLabel.Text = "       You Died!!!\n  You Are Out of Lives\n       Game Over";
                    secondaryWeaponLabel.Position = new Vector2(StateManager.GraphicsManager.GraphicsDevice.Viewport.Width * .35f, StateManager.GraphicsManager.GraphicsDevice.Viewport.Height * .1f);
                }
                else
                {
                    secondaryWeaponLabel.Text = "       You Died!!!\nGame Over";
                    secondaryWeaponLabel.Position = new Vector2(StateManager.GraphicsManager.GraphicsDevice.Viewport.Width * .3f, StateManager.GraphicsManager.GraphicsDevice.Viewport.Height * .1f);
                }
            }
            else
            {
                secondaryWeaponLabel.Position = new Vector2(StateManager.GraphicsManager.GraphicsDevice.Viewport.Width * .01f, StateManager.GraphicsManager.GraphicsDevice.Viewport.Height * .1f - secondaryWeaponLabel.Height / 2);
                secondaryWeaponLabel.Text = "No Secondary Weapon";
                if (playerShip.CurrentWeaponName != null)
                {
                    secondaryWeaponLabel.Text = playerShip.CurrentWeaponName;
                }
                secondaryWeaponLabel.Text += String.Format("\n{0} Extra Lives Remaining", StateManager.Lives);
            }

            if (!StateManager.NetworkData.IsMultiplayer)
            {
                if (allEnemiesDead && !StateManager.nextLevel)
                {
                    if (StateManager.CurrentLevel == StateManager.HighestUnlockedLevel)
                    {
                        StateManager.HighestUnlockedLevel++;
                    }
                    playerMinimapVisible = null;
                    _minimapYou = null;

                    StateManager.ScreenState = ScreenType.LevelCompleteScreen;
                    StateManager.AllScreens[ScreenType.LevelCompleteScreen.ToInt()].Cast<LevelCompleteScreen>().Sprites.Clear();
                    StateManager.AllScreens[ScreenType.LevelCompleteScreen.ToInt()].Cast<LevelCompleteScreen>().AdditionalSprites.Clear();
                    StateManager.AllScreens[ScreenType.LevelCompleteScreen.ToInt()].Cast<LevelCompleteScreen>().InitScreen(CoreTypes.ScreenType.LevelCompleteScreen);
                }
            }
            //Multiplayer
            else if (allEnemiesDead)
            {
                StateManager.ScreenState = ScreenType.MPWinningScreen;
            }


            if (playerShip.CurrentHealth <= 0 || StateManager.nextLevel || playerShip.ShipState == ShipState.Dead)
            {
                if (!StateManager.NetworkData.IsMultiplayer)
                {
                    if (playerShip.ShipState == ShipState.Dead && StateManager.Lives <= 0)
                    {
                        playerMinimapVisible = null;
                        _minimapYou = null;
                        StateManager.ScreenState = ScreenType.GameOver;
                    }

                    else if (StateManager.nextLevel || playerShip.ShipState == ShipState.Dead)
                    {
                        playerMinimapVisible = null;
                        _minimapYou = null;

                        if (StateManager.nextLevel)
                        {
                            StateManager.nextLevel = false;
                        }
                        else
                        {
                            StateManager.Lives--;
                            StateManager.Deaths++;
                            StateManager.AmountOfPointsRecievedInCurrentLevel = 0;
                            StateManager.AmountOfSpaceBucksRecievedInCurrentLevel = 0;
                        }
                        if (playerShip.ShipState == ShipState.Dead)
                        {
                            StateManager.InitializeSingleplayerGameScreen(playerShip.ShipType, playerShip.Tier);
                        }
                    }
                }
                else
                {
                    if (StateManager.NetworkData.SessionMode == MultiplayerSessionType.LMS)
                    {
                        //TODO: WE NEED A SCREEN FOR MULTIPLAYER DEAD THAT DOESN'T TOUCH THE NETWORKSESSION BUT ONLY LETS YOU LEAVE IF
                        // 1) YOU AREN'T THE HOST
                        // 2) NOBODY ELSE IS IN THE GAME
                    }
                    else
                    {
                        if (playerShip.ShipState == ShipState.Dead && StateManager.Lives <= 0)
                        {
                            playerMinimapVisible = null;
                            _minimapYou = null;
                            StateManager.NetworkData.LeaveSession();
                            //StateManager.ScreenState = MULTIPLAYER DEAD SCREEN!!!!!!!!!!!!!!!!!! (MENTIONED ABOVE)
                        }
                        else if (playerShip.ShipState == ShipState.Dead)
                        {
                            StateManager.Lives--;
                            StateManager.Deaths++;
                            StateManager.AmountOfPointsRecievedInCurrentLevel = 0;
                            StateManager.AmountOfSpaceBucksRecievedInCurrentLevel = 0;
                            playerShip.CurrentHealth = playerShip.InitialHealth;
                            playerShip.ShipState = ShipState.Alive;
                            playerShip.WorldCoords = StateManager.RandomGenerator.NextVector2(new Vector2(500), new Vector2(StateManager.SpawnArea.X + StateManager.SpawnArea.Width, StateManager.SpawnArea.Y + StateManager.SpawnArea.Height));
                            TintColor = Color.White;

                            if (StateManager.Lives >= 5)
                            {
                                StateManager.ScreenState = ScreenType.MPLoseScreen;
                            }
                        }
                    }
                    //StateManager.NetworkData.LeaveSession();
                    //StateManager.ScreenState = CoreTypes.ScreenType.NetworkSelectScreen;
                }

            }

            BackgroundSprite bg = BackgroundSprite.Cast<BackgroundSprite>();
            //TODO: UPDATE SPRITES


#if WINDOWS
            if (_lastState.IsKeyUp(Keys.Escape) && KeyboardManager.State.IsKeyDown(Keys.Escape))
            {
                Paused(null, null);
                StateManager.ScreenState = ScreenType.Pause;
                //_allowMusicHandling = false;
            }
#endif

            for (int e = 0; e < enemies.Count; e++)
            {
                BaseEnemyShip enemy = enemies[e];

                if (enemy.ShipState == ShipState.Dead)
                {
                    enemies.Remove(enemy);
                }
            }

            for (int spr = 0; spr < Sprites.Count; spr++)
            {
                Ship enemy = Sprites[spr] as Ship;

                if (enemy != null && enemy.ShipState == ShipState.Dead)
                {
                    Sprites.Remove(enemy);
                    spr--;
                }
            }

            /*
            if (playerShip.GetType() == typeof(FighterCarrier))
            {
                FighterCarrier ship = playerShip.Cast<FighterCarrier>();
            }
            */

            for (int i = 0; i < StateManager.AllyBullets.Legit.Count; i++)
            {
                Bullet b = StateManager.AllyBullets.Legit[i];
                b.Update();

                foreach (Ship s in StateManager.EnemyShips)
                {
                    //Once bullet list is separated, IsAllyWith call will be deprecated
                    if (!b.IsDead && s.ShipState != ShipState.Exploding && s.ShipState != ShipState.Dead && b.Intersects(s.WCrectangle))
                    {
                        s.CurrentHealth -= b.Damage;
                        b.IsDead = true;
                    }
                }

                b.IsDead = b.IsDead || b.X <= 0 || b.X >= bg.TotalWidth || b.Y <= 0 || b.Y >= bg.TotalHeight;
                if (b.IsDead)
                {
                    StateManager.BulletPool.ReturnBullet(StateManager.AllyBullets.Legit[i]);
                    StateManager.AllyBullets.Legit.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < StateManager.EnemyBullets.Legit.Count; i++)
            {
                Bullet b = StateManager.EnemyBullets.Legit[i];
                b.Update();

                if (StateManager.NetworkData.IsMultiplayer)
                {
                    if (!b.IsDead && playerShip.ShipState != ShipState.Exploding && playerShip.ShipState != ShipState.Dead && b.Intersects(playerShip.WCrectangle))
                    {
                        playerShip.CurrentHealth -= b.Damage;
                        b.IsDead = true;
                    }
                    //Don't handle other collision detection
                }
                else
                {
                    foreach (Ship s in StateManager.AllyShips)
                    {
                        //Once bullet list is separated, IsAllyWith call will be deprecated
                        if (!b.IsDead && s.ShipState != ShipState.Exploding && s.ShipState != ShipState.Dead && b.Intersects(s.WCrectangle))
                        {
                            s.CurrentHealth -= b.Damage;
                            b.IsDead = true;
                        }
                    }
                }

                b.IsDead = b.IsDead || b.X <= 0 || b.X >= bg.TotalWidth || b.Y <= 0 || b.Y >= bg.TotalHeight;
                if (b.IsDead)
                {
                    StateManager.BulletPool.ReturnBullet(StateManager.EnemyBullets.Legit[i]);
                    StateManager.EnemyBullets.Legit.RemoveAt(i);
                    i--;
                }
            }


            for (int i = 0; i < StateManager.AllyBullets.Dud.Count; i++)
            {
                Bullet b = StateManager.AllyBullets.Dud[i];
                b.Update();
                b.IsDead = b.IsDead || b.X <= 0 || b.X >= bg.TotalWidth || b.Y <= 0 || b.Y >= bg.TotalHeight;
                if (b.IsDead)
                {
                    StateManager.BulletPool.ReturnBullet(StateManager.AllyBullets.Dud[i]);
                    StateManager.AllyBullets.Dud.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < StateManager.EnemyBullets.Dud.Count; i++)
            {
                Bullet b = StateManager.EnemyBullets.Dud[i];
                b.Update();
                b.IsDead = b.IsDead || b.X <= 0 || b.X >= bg.TotalWidth || b.Y <= 0 || b.Y >= bg.TotalHeight;
                if (b.IsDead)
                {
                    StateManager.BulletPool.ReturnBullet(StateManager.EnemyBullets.Dud[i]);
                    StateManager.EnemyBullets.Dud.RemoveAt(i);
                    i--;
                }
            }


            /*
            foreach (Ship shootShip in StateManager.ActiveShips)
            {
                foreach (Ship hitShip in StateManager.ActiveShips)
                {
                    if (shootShip != hitShip && shootShip.PlayerType != hitShip.PlayerType && hitShip.CurrentHealth > 0)
                    {
                        if (hitShip.PlayerType == PlayerType.MyShip && shootShip.PlayerType == PlayerType.Ally)
                        {
                        }
                        else if (shootShip.PlayerType == PlayerType.MyShip && hitShip.PlayerType == PlayerType.Ally)
                        {
                        }
                        else
                        {

                           foreach (Bullet b in shootShip.FlyingBullets)
                           {
                               if (b.Rectangle.Intersects(hitShip.WCrectangle))
                               {
                                    hitShip.CurrentHealth -= b.Damage;
                                   b.IsDead = true;
                                }
                            }
                        }
                    }
                }
            }
             */

            #region Movement
            if (playerShip.ShipState == ShipState.Alive)
            {
                Vector2 camMove = Vector2.Zero;
                if (StateManager.InputManager.ShouldMove(MoveDirection.Up))
                {
                    float ymoveAmount = -playerShip.MovementSpeed.Y;
#if XBOX
                ymoveAmount *= Math.Abs(GamePadManager.One.Current.ThumbSticks.Left.Y);
#endif

                    if (worldCam.Pos.Y + ymoveAmount >= bg.Height / 2)
                    {
                        camMove.Y = ymoveAmount;
                    }
                    else
                    {
                        camMove.Y = bg.Height / 2 - worldCam.Pos.Y;
                    }
                }
                else if (StateManager.InputManager.ShouldMove(MoveDirection.Down))
                {
                    float ymoveAmount = playerShip.MovementSpeed.Y;
#if XBOX
                ymoveAmount *= Math.Abs(GamePadManager.One.Current.ThumbSticks.Left.Y);
#endif

                    if (worldCam.Pos.Y + ymoveAmount <= bg.TotalHeight - (bg.Height / 2))
                    {
                        camMove.Y = ymoveAmount;
                    }
                    else
                    {
                        camMove.Y = (bg.TotalHeight - (bg.Height / 2)) - worldCam.Pos.Y;
                    }
                }

                if (StateManager.InputManager.ShouldMove(MoveDirection.Right))
                {
                    float xmoveAmount = playerShip.MovementSpeed.X;
#if XBOX
                xmoveAmount *= Math.Abs(GamePadManager.One.Current.ThumbSticks.Left.X);
#endif

                    if (worldCam.Pos.X + xmoveAmount <= bg.TotalWidth - (bg.Width / 2))
                    {
                        camMove.X = xmoveAmount;
                    }
                    else
                    {
                        camMove.X = (bg.TotalWidth - (bg.Width / 2)) - worldCam.Pos.X;
                    }
                }
                else if (StateManager.InputManager.ShouldMove(MoveDirection.Left))
                {
                    float xmoveAmount = -playerShip.MovementSpeed.X;
#if XBOX
                xmoveAmount *= Math.Abs(GamePadManager.One.Current.ThumbSticks.Left.X);
#endif

                    if (worldCam.Pos.X + xmoveAmount >= bg.Width / 2)
                    {
                        camMove.X = xmoveAmount;
                    }
                    else
                    {
                        camMove.X = bg.Width / 2 - worldCam.Pos.X;
                    }
                }

                if (camMove.X != 0 || camMove.Y != 0)
                {
                    worldCam.Move(camMove);
                    playerShip.WorldCoords = worldCam.Pos;
                }
            }
            #endregion



            foreach (ISprite s in playerSbObjects)
            {
                if (s != miniMap)
                {
                    if (s is ITimerSprite)
                    {
                        (s as ITimerSprite).Update(gameTime);
                    }
                    else
                    {
                        s.Update();
                    }
                }
            }

            if (!_gameHasStarted)
            {
                for (int r = 0; r < StateManager.KnownMap.GetLength(0); r++)
                {
                    for (int c = 0; c < StateManager.KnownMap.GetLength(1); c++)
                    {
                        StateManager.KnownMap[r, c] = false;
                    }
                }
                playerShip.WCMoved -= wcMovePrettyCode;
                playerShip.Healthbar.Position = new Vector2(playerShip.Healthbar.Position.X - (playerShip.Healthbar.Width / 2), playerShip.Healthbar.Position.Y - (playerShip.Healthbar.Height / 1.5f));
            }

            #region (Non-GLib) Networking Code

            /*
            if (StateManager.NetworkData.IsMultiplayer)
            {
                while (StateManager.NetworkData.CurrentSession.LocalGamers[0].IsDataAvailable)
                {
                    NetworkGamer dataSender;
                    StateManager.NetworkData.CurrentSession.LocalGamers[0].ReceiveData(StateManager.NetworkData.DataReader, out dataSender);

                    if (!dataSender.IsLocal)
                    {
                        bool isBullet = StateManager.NetworkData.DataReader.ReadBoolean();

                        if (!isBullet)
                        {
                            Vector4 shipData = StateManager.NetworkData.DataReader.ReadVector4();
                            foreach (Ship s in StateManager.EnemyShips)
                            {
                                BaseAllyShip sh = s as BaseAllyShip;
                                if (sh != null && sh.Controller != null && sh.Controller.Id == dataSender.Id)
                                {
                                    Debug.WriteLine("Received ship data from {0}: [{1}, {2}], health {3}", dataSender.Gamertag, shipData.X, shipData.Y, shipData.W);
                                    sh.WorldCoords = new Vector2(shipData.X, shipData.Y);
                                    sh.Rotation = SpriteRotation.FromRadians(shipData.Z);
                                    sh.CurrentHealth = shipData.W.ToInt();
                                    break;
                                }
                            }
                            foreach (Ship s in StateManager.AllyShips)
                            {
                                BaseAllyShip sh = s as BaseAllyShip;
                                if (sh != null && sh.Controller != null && sh.Controller.Id == dataSender.Id)
                                {
                                    Debug.WriteLine("Received ship data from {0}: [{1}, {2}], health {3}", dataSender.Gamertag, shipData.X, shipData.Y, shipData.W);
                                    sh.WorldCoords = new Vector2(shipData.X, shipData.Y);
                                    sh.Rotation = SpriteRotation.FromRadians(shipData.Z);
                                    sh.CurrentHealth = shipData.W.ToInt();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Vector4 bulletData = StateManager.NetworkData.DataReader.ReadVector4();
                            Vector4 addlData = StateManager.NetworkData.DataReader.ReadVector4();
                            bool senderAlly = false;
                            BaseAllyShip parent = null;
                            try
                            {
                                parent = StateManager.EnemyShips[dataSender];
                            }
                            catch (IndexOutOfRangeException)
                            {
                                senderAlly = true;
                                parent = StateManager.AllyShips[dataSender];
                            }
                            //Bullet newBullet = new Bullet(GameContent.Assets.Images.Ships.Bullets[(ShipType)Enum.Parse(typeof(ShipType), addlData.Z.ToInt().ToString(), true), (ShipTier)Enum.Parse(typeof(ShipTier), addlData.W.ToInt().ToString(), true)], new Vector2(bulletData.X, bulletData.Y), World, parent);
                            Bullet newBullet = StateManager.BulletPool.GetBullet();
                            newBullet.InitializePooledBullet(new Vector2(bulletData.X, bulletData.Y), parent);
                            newBullet.SpriteBatch = World;
                            newBullet.Texture = GameContent.Assets.Images.Ships.Bullets[(ShipType)Enum.Parse(typeof(ShipType), addlData.Z.ToInt().ToString(), true), (ShipTier)Enum.Parse(typeof(ShipTier), addlData.W.ToInt().ToString(), true)];
                            
                            newBullet.Speed = new Vector2(bulletData.Z, bulletData.W);
                            newBullet.Rotation = SpriteRotation.FromRadians(addlData.Y);
                            newBullet.Damage = addlData.X.ToInt();
                            newBullet.MaximumDistance = new Vector2(4000f);
                            //Debug.WriteLine("Bullet received from {0}: X: {1}, Y: {2}", parent.Controller.Gamertag, newBullet.X, newBullet.Y);
                            (senderAlly ? StateManager.AllyBullets : StateManager.EnemyBullets).Legit.Add(newBullet);
                        }
                    }
                }
            }
            */
            #endregion

            _gameHasStarted = true;



            _lastState = KeyboardManager.State;
        }

        Camera2DMatrix worldCam;

        public override void MiscellaneousProcessing()
        {
            //TODO: Draw player spritebatch stuff
            playerSb.Begin();
            foreach (ISprite s in playerSbObjects)
            {
                if (s is ISpriteBatchManagerSprite)
                {
                    s.Cast<ISpriteBatchManagerSprite>().DrawNonAuto();
                }
                else
                {
                    s.Draw();
                }
            }
            playerSb.End();
        }

        bool inNightMode = false;
        RasterizerState rastState = RasterizerState.CullCounterClockwise;
        public readonly static TimeSpan MPEnemyDeadTime = TimeSpan.FromSeconds(3.5);

        public override void OpenSpriteBatch(ref SpriteBatch sb)
        {
            //base.OpenSpriteBatch(ref sb);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, rastState, null,
                                    worldCam.GetTransformation(sb.GraphicsDevice));
        }

        /*
        internal void HandleMusicChange()
        {
            if (_allowMusicHandling && ScreensToAllowMusicProcessing.Contains(StateManager.ScreenState))
            {
            
                //This is an expensive call (unmanaged code transition), better to only call it once
                MediaState currentState = MediaPlayer.State;

                if (StateManager.Options.MusicEnabled && currentState != MediaState.Playing && currentState != MediaState.Paused)
                {
                    RunNextUpdate = new Delegates.NextRun(delegate() { if (StateManager.Options.MusicEnabled) { MediaPlayer.Play(_gameSong); } });
                }
                else if ((StateManager.Options.MusicEnabled && currentState == MediaState.Paused))
                {
                    RunNextUpdate = new Delegates.NextRun(delegate() { MediaPlayer.Resume(); });
                }
            }
         
        }
         */

    }
}