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
        TextSprite BackLabel;
        TextSprite PauseLabel;
        TextSprite ResumeLabel;
        TextSprite ShopLabel;

        public PauseScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)

        {
            
        }

        public void LoadContent(ContentManager content)
        {
            Texture2D button = content.Load<Texture2D>("Images\\Controls\\Button");

            Sprite BackButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            BackButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave += new EventHandler(BackButton_MouseLeave);
            Sprites.Add(BackButton);

            BackLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .139f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .62f), content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Back");
            BackLabel.Color = Color.White;
            BackLabel.IsHoverable = true;
            BackLabel.IsManuallySelectable = true;
            BackLabel.HoverColor = Color.MediumAquamarine;
            BackLabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(BackLabel);

            PauseLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMonoBold"), "Paused");
            PauseLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width / 2 - PauseLabel.Width / 2, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f);
            PauseLabel.Color = Color.White;
            AdditionalSprites.Add(PauseLabel);

            Sprite ResumeButton = new Sprite(button, Vector2.Zero, Sprites.SpriteBatch);
            ResumeButton.Position = new Vector2(ResumeButton.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .2f);
            ResumeButton.MouseEnter += new EventHandler(ResumeButton_MouseEnter);
            ResumeButton.MouseLeave += new EventHandler(ResumeButton_MouseLeave);
            Sprites.Add(ResumeButton);

            ResumeLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Resume");
            ResumeLabel.Position = new Vector2(ResumeLabel.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, ResumeButton.Y+(ResumeButton.Height/2-ResumeLabel.Height/2));
            ResumeLabel.Color = Color.White;
            ResumeLabel.IsHoverable = true;
            ResumeLabel.IsManuallySelectable = true;
            ResumeLabel.HoverColor = Color.MediumAquamarine;
            ResumeLabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(ResumeLabel);

            Sprite ShopButton = new Sprite(button, Vector2.Zero, Sprites.SpriteBatch);
            ShopButton.Position = new Vector2(ResumeButton.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .5f);
            ShopButton.MouseEnter += new EventHandler(ShopButton_MouseEnter);
            ShopButton.MouseLeave += new EventHandler(ShopButton_MouseLeave);
            Sprites.Add(ShopButton);
             
            ShopLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Shop");
            ShopLabel.Position = new Vector2(ShopLabel.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, ShopButton.Y + (ShopButton.Height / 2 - ShopLabel.Height / 2));
            ShopLabel.Color = Color.White;
            ShopLabel.IsHoverable = true;
            ShopLabel.IsManuallySelectable = true;
            ShopLabel.HoverColor = Color.MediumAquamarine;
            ShopLabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(ShopLabel);
        }

        void ShopButton_MouseLeave(object sender, EventArgs e)
        {
            ShopLabel.IsSelected = false;
        }

        void ShopButton_MouseEnter(object sender, EventArgs e)
        {
            ShopLabel.IsSelected = true;
        }

        void ResumeButton_MouseLeave(object sender, EventArgs e)
        {
            
        }

        void ResumeButton_MouseEnter(object sender, EventArgs e)
        {
            
        }   

        public bool mouseInBackButton
        {
            get
            {
                return BackLabel.IsSelected;
            }
        }

        void BackButton_MouseLeave(object sender, EventArgs e)
        {
            BackLabel.IsSelected = false;
        }

        void BackButton_MouseEnter(object sender, EventArgs e)
        {
            BackLabel.IsSelected = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);



            if (mouseInBackButton)
            {
                MouseState ms = Mouse.GetState();
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    if (mouseInBackButton)
                    {
                        StateManager.ScreenState = ScreenState.Game;
                    }
                }
            }

        }
    }
}
