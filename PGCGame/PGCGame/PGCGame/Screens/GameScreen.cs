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

        EnemyDrone enemy;
        Ship playerShip;
        SpriteBatch playerSb;
        Texture2D bgImg;
        Song _gameSong;
        List<ISprite> playerSbObjects = new List<ISprite>();
        ContentManager storedCm;

        List<KeyValuePair<string, Texture2D>> shipTextures = new List<KeyValuePair<string, Texture2D>>();

        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT
            storedCm = content;
            
            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

            _gameSong = content.Load<Song>("Songs\\Movement Proposition");

            bgImg = content.Load<Texture2D>("Images\\Background\\NebulaSky");

            //enemy = new EnemyDrone(content.Load<Texture2D>("Images\\Drones\\Drone1"), Vector2.Zero, Sprites.SpriteBatch);

            enemy = new EnemyDrone(content.Load<Texture2D>("Images\\Torpedo Ship\\Tier1"), Vector2.Zero, Sprites.SpriteBatch);
            enemy.WorldCoords = StateManager.RandomGenerator.NextVector2(new Vector2(200, 9500), new Vector2(600, 10500));
            //TODO: Different texture
            enemy.Color = Color.Green;
            enemy.Tier = ShipTier.Tier1;
            enemy.RotateTowardsMouse = false;
            Sprites.Add(enemy);
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            if (playerShip != null)
            {
                playerShip.Position = playerShip.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport, true);
            }
        }

        Sprite miniMap;

        public void InitializeScreen<TShip>(ShipTier tier) where TShip : Ship
        {
            playerSbObjects.Clear();
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
            playerSbObjects.Add(miniMap);

            if (typeof(TShip) == typeof(Drone))
            {
                throw new Exception("Can't create a Drone as the main ship");
            }

            TShip ship = null;
            Texture2D shipTexture = null;
            if (typeof(TShip) == typeof(FighterCarrier))
            {
                Texture2D droneTexture = null;
                foreach (KeyValuePair<string, Texture2D> kvp in shipTextures)
                {
                    if (kvp.Key.Trim().Equals("Drone"))
                    {
                        droneTexture = kvp.Value;
                    }
                }
                if (droneTexture == null)
                {
                    droneTexture = storedCm.Load<Texture2D>("Images\\Drones\\Drone1");
                    shipTextures.Add(new KeyValuePair<string, Texture2D>("Drone", droneTexture));
                }
                ship = new FighterCarrier(shipTexture, Vector2.Zero, playerSb, droneTexture).Cast<TShip>();
            }
            else
            {
                ship = Activator.CreateInstance(typeof(TShip), null, Vector2.Zero, playerSb).Cast<TShip>();
            }

            foreach (KeyValuePair<string, Texture2D> kvp in shipTextures)
            {
                if (kvp.Key.Trim().Equals(ship.TextureFolder + "\\" + tier.ToString()))
                {
                    shipTexture = kvp.Value;
                }
            }

            if (shipTexture == null)
            {
                shipTexture = storedCm.Load<Texture2D>("Images\\" + ship.TextureFolder + "\\" + tier.ToString().Replace("ShipTier.", ""));
                shipTextures.Add(new KeyValuePair<string, Texture2D>(ship.TextureFolder + "\\" + tier.ToString(), shipTexture));
            }

            ship.Texture = shipTexture;
            ship.UseCenterAsOrigin = true;
            ship.WorldSb = Sprites.SpriteBatch;
            ship.Tier = tier;
            ship.Position = ship.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport, true);
            playerShip = ship;
            playerShip.IsPlayerShip = true;
            playerSbObjects.Add(ship);


            //TEST CODE: Start with one mine; TODO: purchase mines!!!
            //SpaceMine spaceMine = new SpaceMine(Ship.SpaceMine, Vector2.Zero, playerShip.WorldSb);
            //spaceMine.ParentShip = playerShip;
            //playerShip.SpaceMines.Push(spaceMine);
            
        }
        
        List<Sprite> miniShips = new List<Sprite>();

        void miniMap_Updated(object sender, EventArgs e)
        {
            foreach (Sprite s in miniShips)
            {
                playerSbObjects.Remove(s);
            }
            miniShips.Clear();

            if (miniMap.Color == Color.White)
            {

                foreach (Ship s in StateManager.ActiveShips)
                {
                    if (s.GetType() == typeof(Drone))
                    {
                        continue;
                    }
                    Sprite miniShip = new Sprite(new PlainTexture2D(playerSb.GraphicsDevice, 3, 3, s.PlayerType == PlayerType.Enemy ? Color.Red : Color.Lime), miniMap.Position + (s.WorldCoords / MinimapDivAmount), playerSb);
                    miniShip.UseCenterAsOrigin = true;
                    miniShips.Add(miniShip);
                }
                playerSbObjects.AddRange(miniShips);
            }
        }

        void bgspr_Drawn(object sender, EventArgs e)
        {
            foreach (Bullet b in playerShip.FlyingBullets)
            {
                Sprites.SpriteBatch.Draw(b);
            }
            foreach(Bullet b in enemy.FlyingBullets)
            {
                Sprites.SpriteBatch.Draw(b);
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
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                StateManager.ScreenState = ScreenState.Pause;
                MediaPlayer.Pause();
            }
            Vector2 camMove = Vector2.Zero;
            if (keyboard.IsKeyDown((StateManager.Options.ArrowKeysEnabled ? Keys.Up : Keys.W)))
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
            else if (keyboard.IsKeyDown((StateManager.Options.ArrowKeysEnabled ? Keys.Down : Keys.S)))
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

            if (keyboard.IsKeyDown((StateManager.Options.ArrowKeysEnabled ? Keys.Right : Keys.D)))
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
            else if (keyboard.IsKeyDown((StateManager.Options.ArrowKeysEnabled ? Keys.Left : Keys.A)))
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
