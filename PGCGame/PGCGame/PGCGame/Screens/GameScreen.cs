﻿using System;
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
            StateManager.Options.MusicStateChanged += new EventHandler(Options_MusicStateChanged);
            worldCam = new Camera2DMatrix();
            playerSb = new SpriteBatch(spriteBatch.GraphicsDevice);

            _playableAreaOffset = new Vector2(500);
            ClonesMade = GameContent.GameAssets.Sound[SoundEffectType.CloneMade];

#if XBOX
            GamePadManager.One.Buttons.StartButtonPressed += new EventHandler(delegate(object src, EventArgs e){
                if (Visible)
                {
                    StateManager.ScreenState = CoreTypes.ScreenType.Pause;
                }
            });
#endif
        }

        void Options_MusicStateChanged(object sender, EventArgs e)
        {
            RunNextUpdate = new Delegates.NextRun(delegate()
            {
                if (StateManager.Options.MusicEnabled)
                {
                    if (MediaPlayer.State == MediaState.Paused)
                    {
                        MediaPlayer.Resume();
                    }
                    else
                    {
                        MediaPlayer.Play(_gameSong);
                    }
                }
                else
                {
                    MediaPlayer.Stop();
                }
            });
        }

        /// <summary>
        /// The amount to divide the background size by to generate the minimap size.
        /// </summary>
        public const int MinimapDivAmount = 45;

        List<BaseEnemyShip> enemies = new List<BaseEnemyShip>();
        BaseAllyShip playerShip;
        SpriteBatch playerSb;
        SpriteFont normal;
        SpriteFont bold;
        Texture2D bgImg;
        SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;
        Song _gameSong;
        List<ISprite> playerSbObjects = new List<ISprite>();

        public override void InitScreen(ScreenType screenType)
        {
            base.InitScreen(screenType);

            bold = GameContent.GameAssets.Fonts.BoldText;
            normal = GameContent.GameAssets.Fonts.NormalText;
            StateManager.ScreenStateChanged += new EventHandler(delegate(object src, EventArgs arg)
            {
                if (Visible)
                {
                    if (StateManager.Options.MusicEnabled)
                    {
                        if (MediaPlayer.State == MediaState.Paused)
                        {
                            MediaPlayer.Resume();
                        }
                        else
                        {
                            MediaPlayer.Play(_gameSong);
                        }
                    }
                    else
                    {
                        MediaPlayer.Stop();
                    }
                }
            });
            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

            _gameSong = GameContent.GameAssets.Music[ScreenMusic.Level1];

            bgImg = GameContent.GameAssets.Images.Backgrounds.Levels[GameLevel.Level1];
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            if (playerShip != null)
            {
                playerShip.Position = playerShip.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport, true);
            }
            if (miniMap != null)
            {
                miniMap.Y = Graphics.Viewport.TitleSafeArea.Y + 7.5f;
                miniMap.X = Graphics.Viewport.TitleSafeArea.X + Graphics.Viewport.TitleSafeArea.Width - miniMap.Width - 7.5f;
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


        public void InitializeScreen<TShip>(ShipTier tier, bool spawnEnemies) where TShip : BaseAllyShip
        {
            //Reset any active ships, since we're re-initializing the game screen
            StateManager.EnemyShips.Clear();
            StateManager.AllyShips.Clear();

            //Reset music
            _gameHasStarted = false;
            //_allowMusicHandling = false;
            playerSbObjects.Clear();
            Sprites.Clear();
            enemies.Clear();

            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;

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

            StateManager.SpawnArea = new Rectangle(500, 500, bgspr.TotalWidth.Round() - 500, bgspr.TotalHeight.Round() - 500);

            Vector2 minSpawnArea = _playableAreaOffset;
            Vector2 maxSpawnArea = new Vector2(bgspr.TotalWidth, bgspr.TotalHeight) - _playableAreaOffset;

            #region Enemy Spawns
            if (spawnEnemies)
            {

                if (StateManager.CurrentLevel.ToInt() == 4)
                {
                    CloneBoss enemyBoss = new CloneBoss(GameContent.GameAssets.Images.Ships[ShipType.EnemyFighterCarrier, ShipTier.Tier4], Vector2.Zero, Sprites.SpriteBatch);
                    CloneBoss enemyCloneOne = new CloneBoss(GameContent.GameAssets.Images.Ships[ShipType.EnemyFighterCarrier, ShipTier.Tier4], Vector2.Zero, Sprites.SpriteBatch);
                    CloneBoss enemyCloneTwo = new CloneBoss(GameContent.GameAssets.Images.Ships[ShipType.EnemyFighterCarrier, ShipTier.Tier4], Vector2.Zero, Sprites.SpriteBatch);

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
                    for (int i = 0; i < 4 * StateManager.CurrentLevel.ToInt(); i++)
                    {
                        Texture2D enemyTexture = GameContent.GameAssets.Images.Ships[ShipType.Drone, StateManager.RandomGenerator.NextShipTier(ShipTier.Tier1, ShipTier.Tier2)];
                        EnemyBattleCruiser enemy = new EnemyBattleCruiser(GameContent.GameAssets.Images.Ships[ShipType.EnemyBattleCruiser, ShipTier.Tier1], Vector2.Zero, Sprites.SpriteBatch);

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



            if (typeof(TShip) == typeof(Drone))
            {
                throw new Exception("Can't create a Drone as the main ship");
            }

            TShip ship = null;

            //Can't compare on a type
            //These are full type names, if we change a namespace, this must change
            switch (typeof(TShip).FullName)
            {
                case "PGCGame.BattleCruiser":
                    ship = new BattleCruiser(GameContent.GameAssets.Images.Ships[ShipType.BattleCruiser, tier], Vector2.Zero, this.playerSb).Cast<TShip>();
                    break;
                case "PGCGame.FighterCarrier":
                    ship = new FighterCarrier(GameContent.GameAssets.Images.Ships[ShipType.FighterCarrier, tier], Vector2.Zero, playerSb, GameContent.GameAssets.Images.Ships[ShipType.Drone, ShipTier.Tier1]).Cast<TShip>();
                    break;
                case "PGCGame.TorpedoShip":
                    ship = new TorpedoShip(GameContent.GameAssets.Images.Ships[ShipType.TorpedoShip, tier], Vector2.Zero, playerSb).Cast<TShip>();
                    break;
            }

            ship.UseCenterAsOrigin = true;
            ship.WorldSb = Sprites.SpriteBatch;
            ship.Tier = tier;
            ship.Position = ship.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport, true);
            playerShip = ship;
            playerShip.WCMoved += new EventHandler(playerShip_WCMoved);
            playerShip.IsPlayerShip = true;
            playerShip.RotateTowardsMouse = true;
            playerSbObjects.Add(ship);

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
        }

        void playerShip_WCMoved(object sender, EventArgs e)
        {
            if (_minimapYou != null)
            {
                playerMinimapVisible = new Rectangle((_minimapYou.X - (Sprites.SpriteBatch.GraphicsDevice.Viewport.Width / 2) / MinimapDivAmount).ToInt(), (_minimapYou.Y - (Sprites.SpriteBatch.GraphicsDevice.Viewport.Height / 2) / MinimapDivAmount).ToInt(), (_minimapYou.Width + (Sprites.SpriteBatch.GraphicsDevice.Viewport.Width / 2) / MinimapDivAmount).ToInt(), (_minimapYou.Height + (Sprites.SpriteBatch.GraphicsDevice.Viewport.Height / 2) / MinimapDivAmount).ToInt());
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
            CloneBoss enemyCloneOne = new CloneBoss(GameContent.GameAssets.Images.Ships[ShipType.EnemyFighterCarrier, ShipTier.Tier4], Vector2.Zero, Sprites.SpriteBatch);
            CloneBoss enemyCloneTwo = new CloneBoss(GameContent.GameAssets.Images.Ships[ShipType.EnemyFighterCarrier, ShipTier.Tier4], Vector2.Zero, Sprites.SpriteBatch);

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
                miniShipInfo = new TextSprite(playerSb, normal, string.Format("HP: {0}/{1}\nDamage: {2}{3}", activeMiniShipDisplay.CurrentHealth, activeMiniShipDisplay.InitialHealth, activeMiniShipDisplay.DamagePerShot, StateManager.DebugData.ShowShipIDs ? "\nID: " + shipIdStr : ""));
                miniShipInfo.Color = Color.White;
                miniShipInfo.Position = new Vector2(miniShipInfoBg.X + (miniShipInfoBg.Width / 2f) - (miniShipInfo.Width / 2f), miniShipInfoTitle.Y + bold.LineSpacing);
                if (StateManager.HasBoughtScanner || StateManager.DebugData.ShowShipIDs)
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

        private void addShipToMinimap(Ship ship, ref Ship activeMiniShipDisplay)
        {
            if (ship.GetType() == typeof(Drone) || ship.ShipState == ShipState.Exploding || ship.ShipState == ShipState.Dead)
            {
                return;
            }

            Sprite miniShip = new Sprite(GameContent.GameAssets.Images.MiniShips[ship.ShipType], miniMap.Position + (ship.WorldCoords / MinimapDivAmount), playerSb);
            miniShip.Scale = ship.PlayerType == PlayerType.MyShip ? new Vector2(.1f) : new Vector2(.07f);
            miniShip.Color = ship.PlayerType == PlayerType.MyShip ? Color.Green : Color.Red;
            if (StateManager.HasBoughtScanner)
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

        public override void Update(GameTime gameTime)
        {
            miniMap.Update();

            if (!_gameHasStarted)
            {
                //_allowMusicHandling = false;
                MediaPlayer.Stop();
                if (StateManager.Options.MusicEnabled)
                {
                    MediaPlayer.Play(_gameSong);
                }
                //_allowMusicHandling = true;
                playerShip.WorldCoords = worldCam.Pos;
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


            if (playerShip.ShipState == ShipState.Exploding)
            {
                bgspr.Color = Color.Red;
                if (StateManager.Lives != 0)
                {
                    secondaryWeaponLabel.Text = String.Format("         You Died!!!\nYou Have {0} Extra Lives Remaining", StateManager.Lives - 1);
                    secondaryWeaponLabel.Position = new Vector2(StateManager.GraphicsManager.GraphicsDevice.Viewport.Width * .3f, StateManager.GraphicsManager.GraphicsDevice.Viewport.Height * .1f);
                }
                else
                {
                    secondaryWeaponLabel.Text = "       You Died!!!\n  You Are Out of Lives\n       Game Over";
                    secondaryWeaponLabel.Position = new Vector2(StateManager.GraphicsManager.GraphicsDevice.Viewport.Width * .35f, StateManager.GraphicsManager.GraphicsDevice.Viewport.Height * .1f);
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

            if (allEnemiesDead && !StateManager.nextLevel && StateManager.HighestUnlockedLevel != GameLevel.Level4)
            {
                if (StateManager.CurrentLevel == StateManager.HighestUnlockedLevel)
                {
                    StateManager.HighestUnlockedLevel++;
                }
                StateManager.ScreenState = ScreenType.LevelCompleteScreen;
                StateManager.AllScreens[ScreenType.LevelCompleteScreen.ToInt()].Cast<LevelCompleteScreen>().Sprites.Clear();
                StateManager.AllScreens[ScreenType.LevelCompleteScreen.ToInt()].Cast<LevelCompleteScreen>().AdditionalSprites.Clear();
                StateManager.AllScreens[ScreenType.LevelCompleteScreen.ToInt()].Cast<LevelCompleteScreen>().InitScreen(CoreTypes.ScreenType.LevelCompleteScreen);
            }
            else if (StateManager.HighestUnlockedLevel == GameLevel.Level4 && allEnemiesDead && !StateManager.nextLevel)
            {
                StateManager.ScreenState = ScreenType.Credits;
            }

            if (playerShip.CurrentHealth <= 0 || StateManager.nextLevel || playerShip.ShipState == ShipState.Dead)
            {
                if (playerShip.ShipState == ShipState.Dead && StateManager.Lives <= 0)
                {
                    StateManager.ScreenState = ScreenType.GameOver;
                }

                else if (StateManager.nextLevel || playerShip.ShipState == ShipState.Dead)
                {
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
                    StateManager.AllyBullets.Legit.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < StateManager.EnemyBullets.Legit.Count; i++)
            {
                Bullet b = StateManager.EnemyBullets.Legit[i];
                b.Update();

                foreach (Ship s in StateManager.AllyShips)
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


                if (_lastState.IsKeyUp(Keys.F11) && KeyboardManager.State.IsKeyDown(Keys.F11))
                {
                    StateManager.Options.ToggleFullscreen();
                }

                worldCam.Move(camMove);
                playerShip.WorldCoords = worldCam.Pos;
            }



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
            }

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

        public override void OpenSpriteBatch(ref SpriteBatch sb)
        {
            //base.OpenSpriteBatch(ref sb);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
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
