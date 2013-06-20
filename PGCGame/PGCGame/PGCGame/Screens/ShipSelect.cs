using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;

namespace PGCGame.Screens
{
    public class ShipSelect : Screen
    {
        public ShipSelect(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            
        }

        TextSprite playLabel;

        public void LoadContent(ContentManager content)
        {
            Texture2D buttonImage = content.Load<Texture2D>("Images\\Controls\\Button");
            SpriteFont SegoeUIMono = content.Load<SpriteFont>("Fonts\\SegoeUIMono");

            Sprite playButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .8f), Sprites.SpriteBatch);
            playLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Play");
            playLabel.Color = Color.White;

            Sprites.Add(playButton);
            AdditionalSprites.Add(playLabel);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
