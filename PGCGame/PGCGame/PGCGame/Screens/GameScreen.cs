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

        SpriteBatch playerSb;

        List<ISprite> playerSbObjects = new List<ISprite>();

        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT
            BackgroundSprite bs = new BackgroundSprite(content.Load<Texture2D>("Images\\Background\\NebulaSky"), Sprites.SpriteBatch, 10, 10);
            worldCam.Pos = new Vector2(bs.Width/2, bs.Height/2);
            BackgroundSprite = bs;
            FighterCarrier ship = new FighterCarrier(content.Load<Texture2D>("Images\\Fighter Carrier\\Tier1"),Vector2.Zero, playerSb);
            ship.Position = ship.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport, true);
            playerSbObjects.Add(ship);
            //Sprites.Add(new Drone(content.Load<Texture2D>("aTexture"), Vector2.Zero, this.Sprites.SpriteBatch, ship)); 
            //use Sprites to load your sprites
            //EX: Sprites.Add(new Sprite(content.Load<Texture2D>("assetName"), Vector2.Zero, Sprites.SpriteBatch));
            //OR
            //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //TODO: UPDATE SPRITES
            KeyboardState keyboard = Keyboard.GetState();
            Vector2 camMove = Vector2.Zero;
            if (keyboard.IsKeyDown(Keys.W))
            {
                camMove.Y = -1;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                camMove.Y = 1;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                camMove.X = 1;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                camMove.X = -1;
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
