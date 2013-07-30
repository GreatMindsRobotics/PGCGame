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

namespace PGCGame.Screens
{
    public class GameScreen : BaseScreen
    {

        private Vector2 _playableAreaOffset;

        public static readonly ScreenType[] ScreensToAllowMusicProcessing = new ScreenType[] { ScreenType.Game, ScreenType.Options, ScreenType.Pause, ScreenType.Shop, ScreenType.UpgradeScreen, ScreenType.WeaponSelect };

        public GameScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            StateManager.Options.MusicStateChanged += new EventHandler(Options_MusicStateChanged);
            worldCam = new Camera2DMatrix();
            playerSb = new SpriteBatch(spriteBatch.GraphicsDevice);

            _playableAreaOffset = new Vector2(500);

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
        }

        Sprite miniMap;

        TextSprite secondaryWeaponLabel;

        public void InitializeScreen<TShip>(ShipTier tier) where TShip : BaseAllyShip
        {
            //Reset any active ships, since we're re-initializing the game screen
            StateManager.ActiveShips.Clear();

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
            BackgroundSprite bgspr = new BackgroundSprite(bgImg, Sprites.SpriteBatch, 10, 2);
            bgspr.Drawn += new EventHandler(bgspr_Drawn);
            worldCam.Pos = new Vector2(bgspr.TotalWidth / 2, bgspr.TotalHeight - (bgspr.Height / 2));
            BackgroundSprite = bgspr;

            //Set the world size in StateManager for later access
            StateManager.WorldSize = new Rectangle(0, 0, (int)bgspr.TotalWidth, (int)bgspr.TotalHeight);

            Vector2 minSpawnArea = _playableAreaOffset;
            Vector2 maxSpawnArea = new Vector2(bgspr.TotalWidth, bgspr.TotalHeight) - _playableAreaOffset;

            for (int i = 0; i < 4 * StateManager.Level.ToInt(); i++)
            {
                Texture2D enemyTexture = GameContent.GameAssets.Images.Ships[ShipType.Drone, StateManager.RandomGenerator.NextShipTier(ShipTier.Tier1, ShipTier.Tier2)];
                EnemyBattleCruiser enemy = new EnemyBattleCruiser(GameContent.GameAssets.Images.Ships[ShipType.EnemyBattleCruiser, ShipTier.Tier1], Vector2.Zero, Sprites.SpriteBatch);

                enemy.WorldCoords = StateManager.RandomGenerator.NextVector2(minSpawnArea, maxSpawnArea);

                enemy.DistanceToNose = .5f;

                enemy.Tier = ShipTier.Tier1;

                Sprites.Add(enemy);
                enemies.Add(enemy);
            }



            miniMap = new Sprite(new PlainTexture2D(Sprites.SpriteBatch.GraphicsDevice, 1, 1, new Color(Color.Navy.R, Color.Navy.G, Color.Navy.B, 128)), Vector2.Zero, playerSb);
            miniMap.Width = bgspr.TotalWidth / MinimapDivAmount;
            miniMap.Color = Color.White;
            miniMap.Height = bgspr.TotalHeight / MinimapDivAmount;
            miniMap.Y = Graphics.Viewport.TitleSafeArea.Y + 7.5f;
            miniMap.X = Graphics.Viewport.TitleSafeArea.X + Graphics.Viewport.TitleSafeArea.Width - miniMap.Width - 7.5f;
            miniMap.Updated += new EventHandler(miniMap_Updated);
            miniShipInfoBg = new Sprite(new PlainTexture2D(Sprites.SpriteBatch.GraphicsDevice, 1, 1, new Color(0, 0, 0, 192)), new Vector2(7.5f, miniMap.Y), playerSb);
            miniShipInfoBg.Height = 0.01f;
            miniShipInfoBg.Width = 767.5f - miniShipInfoBg.X - 7.5f - miniMap.Width - 266.6666667f;
            miniShipInfoBg.X = miniMap.X - miniShipInfoBg.Width - 7.5f;
            miniShipInfoBg.Color = Color.Transparent;
            playerSbObjects.Add(miniShipInfoBg);
            playerSbObjects.Add(miniMap);

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
            playerShip.IsPlayerShip = true;
            playerShip.RotateTowardsMouse = true;
            playerSbObjects.Add(ship);

            //TODO: Tier change event handles this
            //playerShip.InitialHealth = 100;

            //Set as own ship
            playerShip.PlayerType = PlayerType.MyShip;
        }

        Sprite miniShipInfoBg;
        TextSprite miniShipInfoTitle = null;
        TextSprite miniShipInfo = null;
        List<Sprite> miniShips = new List<Sprite>();

        public void ResetLastKS(params Keys[] allKeys)
        {
            _lastState = new KeyboardState(allKeys);
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
            foreach (Ship s in StateManager.ActiveShips)
            {
                if (s.GetType() == typeof(Drone))
                {
                    continue;
                }
                //Sprite miniShip = new Sprite(GameContent.GameAssets.Images.MiniShips[s.PlayerType], miniMap.Position + (s.WorldCoords / MinimapDivAmount), playerSb);
                Sprite miniShip = new Sprite(GameContent.GameAssets.Images.MiniShips[s.ShipType], miniMap.Position + (s.WorldCoords / MinimapDivAmount), playerSb);
                miniShip.Scale = s.PlayerType == PlayerType.MyShip ? new Vector2(.1f) : new Vector2(.07f);
                miniShip.Color = s.PlayerType == PlayerType.MyShip ? Color.Green : Color.Red;
                if (StateManager.HasBoughtScanner)
                {
                    miniShip.Rotation = s.Rotation;
                }
                miniShip.UseCenterAsOrigin = true;
                miniShips.Add(miniShip);
#if WINDOWS
                //TODO: Minimap ship info showing up on Xbox
                if (miniShip.Intersects(MouseManager.CurrentMouseState) && activeMiniShipDisplay == null)
                {
                    activeMiniShipDisplay = s;
                }
#endif
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

        }

        void bgspr_Drawn(object sender, EventArgs e)
        {
            //foreach (Bullet b in playerShip.FlyingBullets)
            //{
            //    Sprites.SpriteBatch.Draw(b);
            //}
            //foreach (var enemy in enemies)
            //{
            //    foreach (Bullet b in enemy.FlyingBullets)
            //    {
            //        Sprites.SpriteBatch.Draw(b);
            //    }
            //}
            //if (playerShip.GetType() == typeof(FighterCarrier))
            //{
            //    foreach (Drone drone in playerShip.Cast<FighterCarrier>().Drones)
            //    {
            //        foreach (Bullet b in drone.FlyingBullets)
            //        {
            //            Sprites.SpriteBatch.Draw(b);
            //        }

            //    }
            //}

            foreach (Bullet b in StateManager.LegitBullets)
            {
                Sprites.SpriteBatch.Draw(b);
            }
            foreach (Bullet b in StateManager.DudBullets)
            {
                Sprites.SpriteBatch.Draw(b);
            }

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

        public override void Update(GameTime gameTime)
        {
            if (!_gameHasStarted)
            {
                //_allowMusicHandling = false;
                MediaPlayer.Stop();
                if (StateManager.Options.MusicEnabled)
                {
                    MediaPlayer.Play(_gameSong);
                }
                //_allowMusicHandling = true;
            }

            base.Update(gameTime);
            KeyboardState keyboard = Keyboard.GetState();
            Boolean allEnemiesDead = true;
            foreach (BaseEnemyShip enemyShip in enemies)
            {
                if (StateManager.DebugData.KillAll && _lastState.IsKeyUp(Keys.F7) && keyboard.IsKeyDown(Keys.F7))
                {
                    enemyShip.CurrentHealth = (keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt)) && enemyShip.CurrentHealth > 0 ? 1 : 0;
                }
                if (enemyShip.CurrentHealth > 0)
                {
                    allEnemiesDead = false;
                }
            }

            if (allEnemiesDead && !StateManager.nextLevel && StateManager.Level != GameLevel.Level4)
            {
                StateManager.ScreenState = ScreenType.LevelSelect;
            }
            else if (StateManager.Level == GameLevel.Level4 && allEnemiesDead && !StateManager.nextLevel)
            {
                
              StateManager.ScreenState = ScreenType.Credits;
            }

            secondaryWeaponLabel.Text = "No Secondary Weapon";

            if (playerShip.CurrentWeaponName != null)
            {
                secondaryWeaponLabel.Text = playerShip.CurrentWeaponName;
            }


            secondaryWeaponLabel.Text += "\n" + StateManager.lives + " lives remaining";


            if (playerShip.CurrentHealth <= 0 || StateManager.nextLevel)
            {
                if (StateManager.lives > 0 || StateManager.nextLevel)
                {
                    if (StateManager.nextLevel)
                    {
                        StateManager.nextLevel = false;
                    }
                    else
                    {
                        StateManager.lives -= 1;
                    }
                    if (playerShip.ShipType == ShipType.BattleCruiser)
                    {
                        InitializeScreen<BattleCruiser>(playerShip.Tier);
                    }
                    else if (playerShip.ShipType == ShipType.FighterCarrier)
                    {
                        InitializeScreen<FighterCarrier>(playerShip.Tier);
                    }
                    else if (playerShip.ShipType == ShipType.TorpedoShip)
                    {
                        InitializeScreen<TorpedoShip>(playerShip.Tier);
                    }
                }
                else if(playerShip.ShipState == ShipState.Dead)
                {
                    StateManager.ScreenState = ScreenType.GameOver;
                }
            }

            BackgroundSprite bg = BackgroundSprite.Cast<BackgroundSprite>();
            //TODO: UPDATE SPRITES


#if WINDOWS
            if (_lastState.IsKeyUp(Keys.Escape) && keyboard.IsKeyDown(Keys.Escape))
            {
                StateManager.ScreenState = ScreenType.Pause;
                //_allowMusicHandling = false;
            }
#endif
            /*
            for (int i = 0; i < playerShip.FlyingBullets.Count; i++)
            {
                Bullet b = playerShip.FlyingBullets[i];
                if (b.IsDead || b.X <= 0 || b.X >= bg.TotalWidth || b.Y <= 0 || b.Y >= bg.TotalHeight)
                {
                    playerShip.FlyingBullets.RemoveAt(i);
                    i--;
                }
            }
            */

            for (int e = 0; e < enemies.Count; e++)
            {
                BaseEnemyShip enemy = enemies[e];

                if (enemy.IsDead)
                {
                    enemies.Remove(enemy);
                }
            }

            if (playerShip.GetType() == typeof(FighterCarrier))
            {
                FighterCarrier ship = playerShip.Cast<FighterCarrier>();
            }

            for (int i = 0; i < StateManager.LegitBullets.Count; i++)
            {
                Bullet b = StateManager.LegitBullets[i];
                b.Update();
                foreach (Ship s in StateManager.ActiveShips)
                {
                    if (!b.IsDead && s.ShipID != b.ParentShip.ShipID && !s.IsAllyWith(b.ParentShip.PlayerType) && b.Intersects(s.WCrectangle))
                    {
                        s.CurrentHealth -= b.Damage;
                        b.IsDead = true;
                    }
                }
                b.IsDead = b.IsDead || b.X <= 0 || b.X >= bg.TotalWidth || b.Y <= 0 || b.Y >= bg.TotalHeight;
                if (b.IsDead)
                {
                    StateManager.LegitBullets.RemoveAt(i);
                    i--;
                }

            }
            

            for (int i = 0; i < StateManager.DudBullets.Count; i++)
            {
                Bullet b = StateManager.DudBullets[i];
                b.Update();
                b.IsDead = b.IsDead || b.X <= 0 || b.X >= bg.TotalWidth || b.Y <= 0 || b.Y >= bg.TotalHeight;
                if (b.IsDead)
                {
                    StateManager.DudBullets.RemoveAt(i);
                    i--;
                }
            }



            //foreach (Ship shootShip in StateManager.ActiveShips)
            //{
            //    foreach (Ship hitShip in StateManager.ActiveShips)
            //    {
            //        if (shootShip != hitShip && shootShip.PlayerType != hitShip.PlayerType && hitShip.CurrentHealth > 0)
            //        {
            //            if (hitShip.PlayerType == PlayerType.MyShip && shootShip.PlayerType == PlayerType.Ally)
            //            {
            //            }
            //            else if (shootShip.PlayerType == PlayerType.MyShip && hitShip.PlayerType == PlayerType.Ally)
            //            {
            //            }
            //            else
            //            {

            //                foreach (Bullet b in shootShip.FlyingBullets)
            //                {
            //                    if (b.Rectangle.Intersects(hitShip.WCrectangle))
            //                    {
            //                        hitShip.CurrentHealth -= b.Damage;
            //                        b.IsDead = true;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}


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

            if (_lastState.IsKeyUp(Keys.F11) && keyboard.IsKeyDown(Keys.F11))
            {
                StateManager.Options.ToggleFullscreen();
            }

            worldCam.Move(camMove);
            playerShip.WorldCoords = worldCam.Pos;

            foreach (ISprite s in playerSbObjects)
            {
                if (s != miniMap)
                {
                    if (s.GetType().Implements(typeof(ITimerSprite)))
                    {
                        s.Cast<ITimerSprite>().Update(gameTime);
                    }
                    else
                    {
                        s.Update();
                    }
                }
            }

            _gameHasStarted = true;

            miniMap.Update();

            _lastState = keyboard;
        }

        Camera2DMatrix worldCam;

        public override void MiscellaneousProcessing()
        {
            //TODO: Draw player spritebatch stuff
            playerSb.Begin();
            foreach (ISprite s in playerSbObjects)
            {
                if (s.GetType().Implements(typeof(ISpriteBatchManagerSprite)))
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
