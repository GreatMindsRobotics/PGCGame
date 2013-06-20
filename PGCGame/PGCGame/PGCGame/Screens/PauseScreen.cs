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
    public class PauseScreen : Screen
    {
        TextSprite PauseLabel;
        bool mouseInBackButton = false;
        public PauseScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)

        {
            
        }

        public void LoadContent(ContentManager content)
        {
            Sprite BackButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            PauseLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .139f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .62f), content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Back");
            PauseLabel.Color = Color.White;
            Sprite PauseButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            PauseLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .139f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .62f), content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Back");
            PauseButton.MouseEnter += new EventHandler(PauseButton_MouseEnter);
            PauseButton.MouseLeave += new EventHandler(PauseButton_MouseLeave);



        }

        void PauseButton_MouseLeave(object sender, EventArgs e)
        {
            
        }

        void PauseButton_MouseEnter(object sender, EventArgs e)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
