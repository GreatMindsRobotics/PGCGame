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

namespace PGCGame.Screens
{
    public class GameScreen : Screen
    {
        public GameScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Pink)
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT
            BackgroundSprite = new BackgroundSprite(content.Load<Texture2D>("Images\\Background\\NebulaSky"), Sprites.SpriteBatch, 10, 10);
            FighterCarrier ship = new FighterCarrier(content.Load<Texture2D>("Ships\\Additional\\1"),Vector2.Zero, Sprites.SpriteBatch);
            ship.Position = ship.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport);
            Sprites.Add(ship);
            //Sprites.Add(ship);
            //Sprites.Add(new Drone(content.Load<Texture2D>("aTexture"), Vector2.Zero, this.Sprites.SpriteBatch, ship)); 
            //use Sprites to load your sprites
            //EX: Sprites.Add(new Sprite(content.Load<Texture2D>("assetName"), new Vector2(0, 0), Sprites.SpriteBatch));
            //OR
            //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //TODO: UPDATE SPRITES
        }
    }
}
