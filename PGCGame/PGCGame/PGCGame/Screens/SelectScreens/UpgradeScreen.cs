﻿using System;
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
    class UpgradeScreen : BaseSelectScreen
    {
       public UpgradeScreen(SpriteBatch spriteBatch)
       : base(spriteBatch)
        {
            
        }

       public override void LoadContent(ContentManager content)
       {
           Texture2D tempImage = content.Load<Texture2D>("Images\\NonPlayingObject\\Planet");
           SpriteFont font = content.Load<SpriteFont>("Fonts\\SegoeUIMono");

           Sprite image = new Sprite(tempImage, Vector2.Zero, Sprites.SpriteBatch);
           TextSprite text = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "TODO");
           text.Color = Color.White;
           image.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, 50);
           text.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, 30);
           items.Add(new KeyValuePair<Sprite, TextSprite>(image, text));
           
           Sprite image2 = new Sprite(tempImage, Vector2.Zero, Sprites.SpriteBatch);
           TextSprite text2 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "todo");
           image2.Scale = new Vector2(0.5f, 0.5f);
           image2.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.66f, 100);
           text2.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, 30);
           items.Add(new KeyValuePair<Sprite, TextSprite>(image2, text2));
           Sprite image3 = new Sprite(tempImage, Vector2.Zero, Sprites.SpriteBatch);
           TextSprite text3 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "todo");
           text2.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, 30);
           image3.Scale = new Vector2(0.75f,0.75f);
           image3.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.66f, 100);
        
           base.LoadContent(content);
       }

       public override void Update(GameTime gameTime)
       {
           
           base.Update(gameTime);
       }
    }
}
