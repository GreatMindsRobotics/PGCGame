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
        Texture2D tempImage = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
        Texture2D ScannerImage = GameContent.GameAssets.Images.Equipment[EquipmentType.Scanner, TextureDisplayType.ShopDisplay];
        SpriteFont font = GameContent.GameAssets.Fonts.NormalText;
        List<KeyValuePair<Sprite, string>> itemsShown = new List<KeyValuePair<Sprite, string>>();

        public override void InitScreen(ScreenType screenType)
        {

            Texture2D tempImage = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
            Texture2D ScannerImage = GameContent.GameAssets.Images.Equipment[EquipmentType.Scanner, TextureDisplayType.ShopDisplay];
            SpriteFont font = GameContent.GameAssets.Fonts.NormalText;


            //scanner

            Sprite Scanner = new Sprite(ScannerImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), Sprites.SpriteBatch);
            TextSprite text4 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 1.5f), font, "Scanner", Color.White);
            text4.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.01f);
            Scanner.Scale = new Vector2(0.5f, 0.5f);
            itemsShown.Add(new KeyValuePair<Sprite, string>(Scanner, StateManager.SpaceBucks.ToString()));
            items.Add(new KeyValuePair<Sprite, TextSprite>(Scanner, text4));

            ChangeItem += new EventHandler(UpgradeScreen_ChangeItem);

            nextButtonClicked += new EventHandler(UpgradeScreen_nextButtonClicked);

            base.InitScreen(screenType);
            acceptLabel.Text = "Buy";
        }


        void UpgradeScreen_nextButtonClicked(object sender, EventArgs e)
        {
            foreach (KeyValuePair<Sprite, string> item in itemsShown)
            {
                if (item.Key == items[selected].Key)
                {
                    StateManager.HasBoughtScanner = true;
                    break;
                }
                else if (item.Key == items[selected].Key)
                {
                    StateManager.HealthPacks++;
                    break;
                }
            }
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
