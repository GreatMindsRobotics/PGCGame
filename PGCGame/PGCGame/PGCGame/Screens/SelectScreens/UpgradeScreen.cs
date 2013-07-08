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
using PGCGame.CoreTypes;

namespace PGCGame.Screens.SelectScreens
{
    class UpgradeScreen : BaseSelectScreen
    {
       public UpgradeScreen(SpriteBatch spriteBatch)
       : base(spriteBatch)
        {
            
        }

       TextSprite text;
       TextSprite text2;
       TextSprite text3;

       public override void InitScreen(ScreenType screenType)
       {
           base.InitScreen(screenType);

           Texture2D tempImage = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
           SpriteFont font = GameContent.GameAssets.Fonts.NormalText;

           Sprite image = new Sprite(tempImage, Vector2.Zero, Sprites.SpriteBatch);
           text = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "TODO");
           text.Color = Color.White;
           image.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, 50);
           text.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, 30);
           items.Add(new KeyValuePair<Sprite, TextSprite>(image, text));
           
           Sprite image2 = new Sprite(tempImage, Vector2.Zero, Sprites.SpriteBatch);
           text2 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "Todo");
           image2.Scale = new Vector2(0.5f, 0.5f);
           image2.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.66f, 150);
           text2.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, 30);
           text2.Color = Color.White;
           items.Add(new KeyValuePair<Sprite, TextSprite>(image2, text2));

           Sprite image3 = new Sprite(tempImage, Vector2.Zero, Sprites.SpriteBatch);
           text3 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "todo too");
           text3.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, 30);
           image3.Scale = new Vector2(0.75f,0.75f);
           image3.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.63f, 100);
           items.Add(new KeyValuePair<Sprite, TextSprite>(image3, text3));

           acceptLabel.Text = "Buy";
       }

       public override void Update(GameTime gameTime)
       {
           base.Update(gameTime);
       }
    }
}
