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

        public override void LoadContent(ContentManager content)
        {
            Texture2D image = content.Load<Texture2D>("Images\\NonPlayingObject\\Planet");
            SpriteFont font = content.Load<SpriteFont>("Fonts\\SegoeUIMono");



            Sprite weapon1 = new Sprite(image, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), Sprites.SpriteBatch);
            TextSprite text1 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "TODO", Color.White);                                                                                                                                                                            

            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon1, text1));

            Sprite weapon2 = new Sprite(image, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.65f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.2f), Sprites.SpriteBatch);
            weapon2.Scale = new Vector2(0.5f, 0.5f);

            Sprite weapon3 = new Sprite(image, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.69f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.3f), Sprites.SpriteBatch);
            weapon3.Scale = new Vector2(0.3f, 0.3f);




            TextSprite text2 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "TODO", Color.White);
            TextSprite text3 = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.01f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), font, "TODO", Color.White);

            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon2, text2));
            items.Add(new KeyValuePair<Sprite, TextSprite>(weapon3, text3));

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
