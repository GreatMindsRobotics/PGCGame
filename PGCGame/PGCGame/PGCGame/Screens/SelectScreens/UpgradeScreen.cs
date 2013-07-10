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

       List<KeyValuePair<Sprite, string>> itemsShown = new List<KeyValuePair<Sprite, string>>();

       public override void InitScreen(ScreenType screenType)
       {




           Texture2D tempImage = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
           Texture2D ScannerImage = GameContent.GameAssets.Images.Equipment[EquipmentType.Scanner, TextureDisplayType.ShopDisplay];
           SpriteFont font = GameContent.GameAssets.Fonts.NormalText;

          

           Sprite image = new Sprite(tempImage, Vector2.Zero, Sprites.SpriteBatch);
           //Sprite equipment1 = new Sprite(tempImage, Vector2.Zero, Sprites.SpriteBatch);
           TextSprite text = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "TODO");
           text.Color = Color.White;
           image.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, 50); 
           text.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, 30);
           items.Add(new KeyValuePair<Sprite, TextSprite>(image, text));



           //scanner

           Sprite Scanner = new Sprite(ScannerImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), Sprites.SpriteBatch);
            TextSprite text4 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 1.5f), font, "Scanner", Color.White);
            text4.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.01f); 
           Scanner.Scale = new Vector2(0.5f, 0.5f);
            itemsShown.Add(new KeyValuePair<Sprite, string>(Scanner, text4.ToString()));
            items.Add(new KeyValuePair<Sprite, TextSprite>(Scanner, text4));


           Sprite equipment1 = new Sprite(tempImage, Vector2.Zero, Sprites.SpriteBatch);
           TextSprite text1 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "Todo");
           equipment1.Scale = new Vector2(0.5f, 0.5f);
           equipment1.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.66f, 150);
           text1.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, 30);
           items.Add(new KeyValuePair<Sprite, TextSprite>(equipment1, text1));
           
           Sprite image3 = new Sprite(tempImage, Vector2.Zero, Sprites.SpriteBatch);
           TextSprite text3 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "todo too");
           text3.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, 30);
           image3.Scale = new Vector2(0.75f,0.75f);
           image3.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.63f, 100);
           items.Add(new KeyValuePair<Sprite, TextSprite>(image3, text3));


           ChangeItem += new EventHandler(UpgradeScreen_ChangeItem);

           base.InitScreen(screenType);

           acceptLabel.Text = "Buy";
       }

       void UpgradeScreen_ChangeItem(object sender, EventArgs e)
       {
           foreach (KeyValuePair<Sprite, TextSprite> item in items)
           {
               if (item.Key == items[selected].Key)
               {
                   nameLabel.Text = item.Value.Text;
                   
                   break;
               }



           }
       }

       public override void Update(GameTime gameTime)
       {
           
           base.Update(gameTime);
       }
    }
}
