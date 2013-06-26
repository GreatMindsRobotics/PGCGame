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

namespace PGCGame.Screens
{
    public class PauseScreen : Screen
    {
        TextSprite ExitLabel;
        TextSprite PauseLabel;
        TextSprite ResumeLabel;
        TextSprite ShopLabel;
        TextSprite OptionsLabel;

        public PauseScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {

        }

        public void LoadContent(ContentManager content)
        {
            Texture2D button = content.Load<Texture2D>("Images\\Controls\\Button");


            PauseLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMonoBold"), "Paused");
            PauseLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width / 2 - PauseLabel.Width / 2, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f);
            PauseLabel.Color = Color.White;
            AdditionalSprites.Add(PauseLabel);

            Sprite ResumeButton = new Sprite(button, Vector2.Zero, Sprites.SpriteBatch);
            ResumeButton.Position = new Vector2(ResumeButton.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .2f);
            ResumeButton.MouseEnter += new EventHandler(ResumeButton_MouseEnter);
            ResumeButton.MouseLeave += new EventHandler(ResumeButton_MouseLeave);
            Sprites.Add(ResumeButton);



            Sprite ExitButton = new Sprite(button, new Vector2(ResumeButton.X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .8f), Sprites.SpriteBatch);
            ExitButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            ExitButton.MouseLeave += new EventHandler(BackButton_MouseLeave);
            Sprites.Add(ExitButton);

            ResumeLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Resume");
            ResumeLabel.Position = new Vector2(ResumeLabel.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, ResumeButton.Y + (ResumeButton.Height / 2 - ResumeLabel.Height / 2));
            ResumeLabel.Color = Color.White;
            ResumeLabel.IsHoverable = true;
            ResumeLabel.IsManuallySelectable = true;
            ResumeLabel.HoverColor = Color.MediumAquamarine;
            ResumeLabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(ResumeLabel);

            ExitLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Exit");
            ExitLabel.Position = new Vector2(ExitLabel.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, ExitButton.Y + (ExitButton.Height / 2 - ExitLabel.Height / 2));
            ExitLabel.Color = Color.White;
            ExitLabel.IsHoverable = true;
            ExitLabel.IsManuallySelectable = true;
            ExitLabel.HoverColor = Color.MediumAquamarine;
            ExitLabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(ExitLabel);

            Sprite ShopButton = new Sprite(button, Vector2.Zero, Sprites.SpriteBatch);
            ShopButton.Position = new Vector2(ResumeButton.X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .53f);
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

            Sprite OptionButton = new Sprite(button, Vector2.Zero, Sprites.SpriteBatch);
            OptionButton.Position = new Vector2(ResumeButton.X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .37f);
            OptionButton.MouseEnter += new EventHandler(OptionButton_MouseEnter);
            OptionButton.MouseLeave += new EventHandler(OptionButton_MouseLeave);
            Sprites.Add(OptionButton);

            OptionsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Options");
            OptionsLabel.Position = new Vector2(OptionsLabel.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, OptionButton.Y + (OptionButton.Height / 2 - OptionsLabel.Height / 2));
            OptionsLabel.Color = Color.White;
            OptionsLabel.IsHoverable = true;
            OptionsLabel.IsManuallySelectable = true;
            OptionsLabel.HoverColor = Color.MediumAquamarine;
            OptionsLabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(OptionsLabel);

        }

        void OptionButton_MouseLeave(object sender, EventArgs e)
        {
            OptionsLabel.IsSelected = false;
        }

        void OptionButton_MouseEnter(object sender, EventArgs e)
        {
            OptionsLabel.IsSelected = true;   
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
            ResumeLabel.IsSelected = false;
        }

        void ResumeButton_MouseEnter(object sender, EventArgs e)
        {
            ResumeLabel.IsSelected = true;
        }

        public bool mouseInExitButton
        {
            get
            {
                return ExitLabel.IsSelected;
            }
        }

        public bool mouseInResumeButton
        {
            get
            {
                return ResumeLabel.IsSelected;
            }
        }

        public bool mouseInShopButton
        {
            get
            {
                return ShopLabel.IsSelected;
            }
        }

        public bool mouseInOptionsButton
        {
            get
            {
                return OptionsLabel.IsSelected;
            }
        }

        void BackButton_MouseLeave(object sender, EventArgs e)
        {
            ExitLabel.IsSelected = false;
        }

        void BackButton_MouseEnter(object sender, EventArgs e)
        {
            ExitLabel.IsSelected = true;
        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (mouseInResumeButton || mouseInExitButton || mouseInShopButton || mouseInOptionsButton)
            {
                MouseState ms = Mouse.GetState();
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    if (mouseInResumeButton)
                    {
                        StateManager.GoBack();
                    }

                    if (mouseInExitButton)
                    {
                        StateManager.ScreenState = ScreenState.MainMenu;

                        //TODO: Abe: Reset stacks; force "Title Screen" onto ScreenState stack
                        StateManager.ActiveShips.Clear();
                    }
                    if (mouseInShopButton)
                    {
                        StateManager.ScreenState = ScreenState.Shop;
                    }

                    if (mouseInOptionsButton)
                    {
                        StateManager.ScreenState = ScreenState.Option;
                    }

                }

            }

        }
    }
}
