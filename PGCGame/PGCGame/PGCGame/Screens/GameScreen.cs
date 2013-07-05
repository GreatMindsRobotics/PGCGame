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
using Glib.XNA.InputLib.Mouse;

namespace PGCGame.Screens
{
    public class GameScreen : Screen
    {
        public GameScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            worldCam = new Camera2DMatrix();
            playerSb = new SpriteBatch(spriteBatch.GraphicsDevice);
        }
        
        /// <summary>
        /// The amount to divide the background size by to generate the minimap size.
        /// </summary>
        public const int MinimapDivAmount = 45;

        List<EnemyDrone> enemies = new List<EnemyDrone>();
        BaseAllyShip playerShip;
        SpriteBatch playerSb;
        SpriteFont normal;
        SpriteFont bold;
        Texture2D bgImg;
        Song _gameSong;
        List<ISprite> playerSbObjects = new List<ISprite>();
        
        public void LoadContent()
        {
            bold = GameContent.GameAssets.Fonts.BoldText;
            normal = GameContent.GameAssets.Fonts.NormalText;
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
                miniMap.Y = 7.5f;
                miniMap.X = playerSb.GraphicsDevice.Viewport.Width - miniMap.Width - 7.5f;
                miniShipInfoBg.X = miniMap.X - miniShipInfoBg.Width - 7.5f;
            }
        }

        Sprite miniMap;

        public void InitializeScreen<TShip>(ShipTier tier) where TShip : BaseAllyShip
        {
            playerSbObjects.Clear();
            Sprites.Sprites.Clear();
            enemies.Clear();

            for (int i = 0; i < 4; i++)
            {
                Texture2D enemyTexture = GameContent.GameAssets.Images.Ships[ShipType.Drone, StateManager.RandomGenerator.NextShipTier(ShipTier.Tier1, ShipTier.Tier2)];
                EnemyDrone enemy = new EnemyDrone(enemyTexture, Vector2.Zero, Sprites.SpriteBatch);

                enemy.WorldCoords = StateManager.RandomGenerator.NextVector2(new Vector2(500, 6500), new Vector2(3550, 10500));
                //TODO: Different texture
                enemy.Color = Color.Green;
                enemy.Tier = ShipTier.Tier1;

                Sprites.Add(enemy);
                enemies.Add(enemy);
            }

            BackgroundSprite bgspr = new BackgroundSprite(bgImg, Sprites.SpriteBatch, 10, 2);
            bgspr.Drawn += new EventHandler(bgspr_Drawn);
            worldCam.Pos = new Vector2(bgspr.TotalWidth / 2, bgspr.TotalHeight - (bgspr.Height / 2));
            BackgroundSprite = bgspr;

            miniMap = new Sprite(new PlainTexture2D(Sprites.SpriteBatch.GraphicsDevice, 1, 1, new Color(Color.Navy.R, Color.Navy.G, Color.Navy.B, 128)), Vector2.Zero, playerSb);
            miniMap.Width = bgspr.TotalWidth / MinimapDivAmount;
            miniMap.Color = Color.Transparent;
            miniMap.Height = bgspr.TotalHeight / MinimapDivAmount;
            miniMap.Y = 7.5f;
            miniMap.Updated += new EventHandler(miniMap_Updated);
            miniMap.X = playerSb.GraphicsDevice.Viewport.Width-miniMap.Width-7.5f;
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
            if (typeof(TShip) == typeof(FighterCarrier))
            {
                ship = new FighterCarrier(null, Vector2.Zero, playerSb, GameContent.GameAssets.Images.Ships[ShipType.Drone, ShipTier.Tier1]).Cast<TShip>();
            }
            else
            {
                ship = Activator.CreateInstance(typeof(TShip), null, Vector2.Zero, playerSb).Cast<TShip>();
            }

            ship.Texture = GameContent.GameAssets.Images.Ships[ship.ShipType, ShipTier.Tier1];
            ship.UseCenterAsOrigin = true;
            ship.WorldSb = Sprites.SpriteBatch;
            ship.Tier = tier;
            ship.Position = ship.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport, true);
            playerShip = ship;
            playerShip.IsPlayerShip = true;
            playerShip.RotateTowardsMouse = true;
            playerSbObjects.Add(ship);

            playerShip.InitialHealth = 100;
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

            if (miniMap.Color == Color.White)
            {
                Ship activeMiniShipDisplay = null;
                foreach (Ship s in StateManager.ActiveShips)
                {
                    if (s.GetType() == typeof(Drone))
                    {
                        continue;
                    }
                    Sprite miniShip = new Sprite(new PlainTexture2D(playerSb.GraphicsDevice, 3, 3, s == playerShip ? Color.Lime : (s.PlayerType == PlayerType.Enemy ? Color.Red : Color.CornflowerBlue)), miniMap.Position + (s.WorldCoords / MinimapDivAmount), playerSb);
                    miniShip.UseCenterAsOrigin = true;
                    miniShips.Add(miniShip);
                    if (miniShip.Intersects(MouseManager.CurrentMouseState) && activeMiniShipDisplay == null)
                    {
                        activeMiniShipDisplay = s;
                    }
                }
                miniShipInfoBg.Color = activeMiniShipDisplay != null ? Color.White : Color.Transparent;
                if (activeMiniShipDisplay != null)
                {
                    miniShipInfoTitle = new TextSprite(playerSb, bold, activeMiniShipDisplay.FriendlyName);
                    miniShipInfoTitle.Color = Color.White;
                    miniShipInfoTitle.Position = new Vector2(miniShipInfoBg.X + (miniShipInfoBg.Width / 2f) - (miniShipInfoTitle.Width / 2f), miniShipInfoBg.Y + (miniShipInfoBg.Height / 12.5f));
                    playerSbObjects.Add(miniShipInfoTitle);
                    miniShipInfoBg.Height = (miniShipInfoTitle.Y - miniShipInfoBg.Y) + miniShipInfoTitle.Height + (miniShipInfoTitle.Y - miniShipInfoBg.Y);
                    miniShipInfo = new TextSprite(playerSb, normal, string.Format("HP: {0}/{1}\nDamage: {2}", activeMiniShipDisplay.CurrentHealth, activeMiniShipDisplay.InitialHealth, activeMiniShipDisplay.DamagePerShot));
                    miniShipInfo.Color = Color.White;
                    miniShipInfo.Position = new Vector2(miniShipInfoBg.X + (miniShipInfoBg.Width / 2f) - (miniShipInfo.Width / 2f), miniShipInfoTitle.Y + bold.LineSpacing);
                    if (StateManager.HasBoughtScanner)
                    {
                        miniShipInfoBg.Height += miniShipInfo.Height;
                        playerSbObjects.Add(miniShipInfo);
                    }

                }

                playerSbObjects.AddRange(miniShips);
            }
            else
            {
                miniShipInfoBg.Color = Color.Transparent;

            }
        }

        void bgspr_Drawn(object sender, EventArgs e)
        {
            foreach (Bullet b in playerShip.FlyingBullets)
            {
                Sprites.SpriteBatch.Draw(b);
            }
            foreach (var enemy in enemies)
            {
                foreach (Bullet b in enemy.FlyingBullets)
                {
                    Sprites.SpriteBatch.Draw(b);
                }
            }
            if (playerShip.GetType() == typeof(FighterCarrier))
            {
                foreach (Bullet b in playerShip.Cast<FighterCarrier>().DroneBullets)
                {
                    Sprites.SpriteBatch.Draw(b);
                }

            }

            if (playerShip.ActiveSpaceMine != null && playerShip.ActiveSpaceMine.SpaceMineState != SpaceMineState.Stowed)
            {
                Sprites.SpriteBatch.Draw(playerShip.ActiveSpaceMine);
            }
        }

        KeyboardState _lastState = new KeyboardState();

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (StateManager.Options.MusicEnabled && MediaPlayer.State != MediaState.Playing && MediaPlayer.State != MediaState.Paused)
            {
                MediaPlayer.Play(_gameSong);
            }
            else if ((StateManager.Options.MusicEnabled && MediaPlayer.State == MediaState.Paused))
            {
                MediaPlayer.Resume();
            }


            BackgroundSprite bg = BackgroundSprite.Cast<BackgroundSprite>();
            //TODO: UPDATE SPRITES
            KeyboardState keyboard = Keyboard.GetState();
            if (_lastState.IsKeyUp(Keys.Escape) && keyboard.IsKeyDown(Keys.Escape))
            {
                StateManager.ScreenState = ScreenState.Pause;
                MediaPlayer.Pause();
            }

            foreach (Bullet b in playerShip.FlyingBullets)
            {
                b.IsDead = b.IsDead || b.X <= 0 || b.X >= bg.TotalWidth || b.Y <= 0 || b.Y >= bg.TotalHeight;
            }
            playerShip.FlyingBullets.RemoveAll(b => b.IsDead);
            foreach (var enemy in enemies)
            {
                foreach (Bullet b in enemy.FlyingBullets)
                {
                    b.IsDead = b.IsDead || b.X <= 0 || b.X >= bg.TotalWidth || b.Y <= 0 || b.Y >= bg.TotalHeight;
                }
                enemy.FlyingBullets.RemoveAll(b => b.IsDead);
            }
            
            if (playerShip.GetType() == typeof(FighterCarrier))
            {
                foreach (Bullet b in playerShip.Cast<FighterCarrier>().DroneBullets)
                {
                    b.IsDead = b.IsDead || b.X <= 0 || b.X >= bg.TotalWidth || b.Y <= 0 || b.Y >= bg.TotalHeight;
                }
                playerShip.Cast<FighterCarrier>().DroneBullets.RemoveAll(b => b.IsDead);
            }

            Vector2 camMove = Vector2.Zero;
            if (StateManager.InputManager.ShouldMove(MoveDirection.Up))
            {
                if (worldCam.Pos.Y - playerShip.MovementSpeed.Y >= bg.Height / 2)
                {
                    camMove.Y = -playerShip.MovementSpeed.Y;
                }
                else
                {
                    camMove.Y = bg.Height / 2 - worldCam.Pos.Y;
                }
            }
            else if (StateManager.InputManager.ShouldMove(MoveDirection.Down))
            {
                if (worldCam.Pos.Y + playerShip.MovementSpeed.Y <= bg.TotalHeight - (bg.Height / 2))
                {
                    camMove.Y = playerShip.MovementSpeed.Y;
                }
                else
                {
                    camMove.Y = (bg.TotalHeight - (bg.Height / 2)) - worldCam.Pos.Y;
                }
            }

            if (StateManager.InputManager.ShouldMove(MoveDirection.Right))
            {
                if (worldCam.Pos.X + playerShip.MovementSpeed.X <= bg.TotalWidth - (bg.Width / 2))
                {
                    camMove.X = playerShip.MovementSpeed.X;
                }
                else
                {
                    camMove.X = (bg.TotalWidth - (bg.Width / 2)) - worldCam.Pos.X;
                }
            }
            else if (StateManager.InputManager.ShouldMove(MoveDirection.Left))
            {
                if (worldCam.Pos.X - playerShip.MovementSpeed.X >=  bg.Width / 2)
                {
                    camMove.X = -playerShip.MovementSpeed.X;
                }
                else
                {
                    camMove.X = bg.Width / 2 - worldCam.Pos.X;
                }
            }

            if (_lastState.IsKeyUp(Keys.M) && keyboard.IsKeyDown(Keys.M))
            {
                miniMap.Color = miniMap.Color == Color.White ? Color.Transparent : Color.White;
                
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
    }
}
