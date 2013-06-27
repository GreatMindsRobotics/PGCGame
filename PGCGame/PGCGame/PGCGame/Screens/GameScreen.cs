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
        TorpedoShip enemy;
        public GameScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            worldCam = new Camera2DMatrix();
            playerSb = new SpriteBatch(spriteBatch.GraphicsDevice);
        }

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

            _gameSong = content.Load<Song>("Songs\\Movement Proposition");

            bgImg = content.Load<Texture2D>("Images\\Background\\NebulaSky");

            //enemy = new EnemyDrone(content.Load<Texture2D>("Images\\Drones\\Drone1"), Vector2.Zero, Sprites.SpriteBatch);

            enemy = new TorpedoShip(content.Load<Texture2D>("Images\\Fighter Carrier\\Tier1"), Vector2.Zero, Sprites.SpriteBatch);
            enemy.WorldCoords = new Vector2(1920, 10260);
            Sprites.Add(enemy);
        }

        public void InitializeScreen<TShip>(ShipTier tier) where TShip : Ship
        {
            playerSbObjects.Clear();
            BackgroundSprite bgspr = new BackgroundSprite(bgImg, Sprites.SpriteBatch, 10, 2);
            bgspr.Drawn += new EventHandler(bgspr_Drawn);
            worldCam.Pos = new Vector2(bgspr.TotalWidth / 2, bgspr.TotalHeight - (bgspr.Height / 2));
            BackgroundSprite = bgspr;

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
            playerSbObjects.Add(ship);

            //TEST CODE: Start with one mine; TODO: purchase mines!!!
            //SpaceMine spaceMine = new SpaceMine(Ship.SpaceMine, Vector2.Zero, playerShip.WorldSb);
            //spaceMine.ParentShip = playerShip;
            //playerShip.SpaceMines.Push(spaceMine);
            
        }

        void bgspr_Drawn(object sender, EventArgs e)
        {
            foreach (Bullet b in playerShip.FlyingBullets)
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (StateManager.Options.MusicEnabled == true && MediaPlayer.State != MediaState.Playing && MediaPlayer.State != MediaState.Paused)
            {
                MediaPlayer.Play(_gameSong);
            }
            else if ((StateManager.Options.MusicEnabled == true && MediaPlayer.State == MediaState.Paused))
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
                if (worldCam.Pos.X - playerShip.MovementSpeed.X >= bg.Width / 2)
                {
                    camMove.X = -playerShip.MovementSpeed.X;
                }
                else
                {
                    camMove.X = bg.Width / 2 - worldCam.Pos.X;
                }
            }

            
            worldCam.Move(camMove);
            playerShip.WorldCoords = worldCam.Pos;

            foreach (ISprite s in playerSbObjects)
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
