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

namespace PGCGame.Screens.SelectScreens
{
    public class WeaponSelectScreen : BaseSelectScreen
    {
        public WeaponSelectScreen(SpriteBatch spriteBatch)
            : base(spriteBatch)
        {

        }

        List<KeyValuePair<Sprite, string>> itemsShown = new List<KeyValuePair<Sprite, string>>();

        public override void LoadContent(ContentManager content)
        {
            Texture2D image = content.Load<Texture2D>("Images\\NonPlayingObject\\Planet");
            Texture2D EMP = content.Load<Texture2D>("Images\\Secondary Weapons\\EMP\\EMP");
            Texture2D RayGun = content.Load<Texture2D>("Images\\Secondary Weapons\\Ray Gun\\RayGun");
            Texture2D Bomb = content.Load<Texture2D>("Images\\Secondary Weapons\\Spacemine\\bombas3");
            SpriteFont font = content.Load<SpriteFont>("Fonts\\SegoeUIMono");

            Sprite weapon1 = new Sprite(EMP, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), Sprites.SpriteBatch);
            TextSprite text1 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "\n\nAn electro magnetic pulse. \nThis device disables all \nnearby enemy ships \nwithin your ship's range. \n\ncost: 1000 credits", Color.White);
            weapon1.Scale = new Vector2(0.5f, 0.5f);

            itemsShown.Add(new KeyValuePair<Sprite, string>(weapon1, "EMP"));

            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon1, text1));

            Sprite weapon2 = new Sprite(RayGun, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.60f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.2f), Sprites.SpriteBatch);
            weapon2.Scale = new Vector2(0.5f, 0.5f);

            itemsShown.Add(new KeyValuePair<Sprite, string>(weapon2, "Ray Gun"));

            Sprite weapon3 = new Sprite(Bomb, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.69f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.3f), Sprites.SpriteBatch);
            weapon3.Scale = new Vector2(2f, 2f);

            itemsShown.Add(new KeyValuePair<Sprite, string>(weapon3, "Space Mine"));

            TextSprite text2 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "\n\nThis weapon causes the targeted enemy\n to shrink losing 33% of their health. \nThis does not affect bosses.\n\ncost: 2000 credits", Color.White);
            TextSprite text3 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "\n\nYou place this mine anywhere in space \nand when an enemy crashes into it the mine \nexplodes \n\ncost: 500 credits.", Color.White);

            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon2, text2));
            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon3, text3));

            ChangeItem += new EventHandler(WeaponSelectScreen_ChangeItem);

            base.LoadContent(content);
            acceptLabel.Text = "Buy";
        }

        void WeaponSelectScreen_ChangeItem(object sender, EventArgs e)
        {
            foreach (KeyValuePair<Sprite, string> item in itemsShown)
            {
                if (item.Key == items[selected].Key)
                {
                    nameLabel.Text = item.Value;
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
