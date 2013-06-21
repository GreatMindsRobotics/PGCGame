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
    public class ShipSelect : BaseSelectScreen
    {
        public ShipSelect(SpriteBatch spriteBatch)
            : base(spriteBatch)
        {
            
        }

        
        public override void LoadContent(ContentManager content)
        {
            Sprite ship1 = new Sprite(content.Load<Texture2D>("Images\\Battle Cruiser\\Tier1"), Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text1 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "TODO");
            ship1.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.82f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            ship1.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(ship1, text1));

            Sprite ship2 = new Sprite(content.Load<Texture2D>("Images\\Fighter Carrier\\Tier1"), Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text2 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "TODO");

            items.Add(new KeyValuePair<Sprite, TextSprite>(ship2, text2));

            Sprite ship3 = new Sprite(content.Load<Texture2D>("Images\\Torpedo Ship\\Tier1"), Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text3 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "TODO");

            items.Add(new KeyValuePair<Sprite, TextSprite>(ship3, text3));

            base.LoadContent(content);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
