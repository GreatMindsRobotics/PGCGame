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

        Ship playerShip;
        SpriteBatch playerSb;
        Texture2D bgImg;
        List<ISprite> playerSbObjects = new List<ISprite>();
        ContentManager storedCm;
        List<KeyValuePair<string, Texture2D>> shipTextures = new List<KeyValuePair<string, Texture2D>>();

        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT
            storedCm = content;

            bgImg = content.Load<Texture2D>("Images\\Background\\NebulaSky");


            
            //Sprites.Add(new Drone(content.Load<Texture2D>("aTexture"), Vector2.Zero, this.Sprites.SpriteBatch, ship)); 
            //use Sprites to load your sprites
            //EX: Sprites.Add(new Sprite(content.Load<Texture2D>("assetName"), Vector2.Zero, Sprites.SpriteBatch));
            //OR
            //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
        }

        public void InitializeScreen<TShip>(ShipTier tier) where TShip : Ship
        {
            
            playerSbObjects.Clear();
            BackgroundSprite bgspr = new BackgroundSprite(bgImg, Sprites.SpriteBatch, 10, 10);
            worldCam.Pos = new Vector2(bgspr.TotalWidth / 2, bgspr.TotalHeight / 2);
            BackgroundSprite = bgspr;
            TShip ship = (TShip)Activator.CreateInstance(typeof(TShip), null, Vector2.Zero, playerSb);
            ship.Tier = tier;
            Texture2D shipTexture = null;
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
            ship.Position = ship.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport, true);
            playerShip = ship;
            playerSbObjects.Add(ship);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            BackgroundSprite bg = BackgroundSprite.Cast<BackgroundSprite>();
            //TODO: UPDATE SPRITES
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                StateManager.ScreenState = ScreenState.Pause;
            }
            Vector2 camMove = Vector2.Zero;
            if (keyboard.IsKeyDown(Keys.W))
            {
                if (worldCam.Pos.Y > bg.Height / 2)
                {
                    camMove.Y = -playerShip.MovementSpeed.Y;
                }
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                if (worldCam.Pos.Y < bg.TotalHeight - (bg.Height / 2))
                {
                    camMove.Y = playerShip.MovementSpeed.Y;
                }
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                if (worldCam.Pos.X < bg.TotalWidth - (bg.Width / 2))
                {
                    camMove.X = playerShip.MovementSpeed.X;
                }
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                if (worldCam.Pos.X > bg.Width / 2)
                {
                    camMove.X = -playerShip.MovementSpeed.X;
                }
            }
            
            worldCam.Move(camMove);
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
