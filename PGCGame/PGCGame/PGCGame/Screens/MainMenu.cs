using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;

namespace PGCGame.Screens
{
    public class MainMenu : Screen
    {
        public const bool DebugBackground = false;

        public MainMenu(SpriteBatch spriteBatch)
            : base(spriteBatch, DebugBackground ? Color.Red : Color.Black)
        {
            
        }

        TextSprite SinglePlayerLabel;
        TextSprite MultiPlayerLabel;
        TextSprite BackLabel;
        TextSprite OptionsLabel;
        TextSprite CreditsLabel;
        TextSprite ShopLabel;

        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT

            //use Sprites to load your sprites
            //EX: Sprites.Add(new Sprite(content.Load<Texture2D>("assetName"), new Vector2(0, 0), Sprites.SpriteBatch));
            //OR
            //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
            Texture2D buttonImage = content.Load<Texture2D>("Images\\Controls\\Button");
            SpriteFont SegoeUIMono = content.Load<SpriteFont>("Fonts\\SegoeUIMono");

            Sprite Title = new Sprite(content.Load<Texture2D>("Images\\Controls\\Gametitle"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width*.05f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height*.07f), Sprites.SpriteBatch);

            this.BackgroundSprite = new HorizontalMenuBGSprite(content.Load<Texture2D>("Images\\Background\\1920by1080SkyStar"), Sprites.SpriteBatch);

            Texture2D planetTexture = content.Load<Texture2D>("Images\\NonPlayingObject\\Planet");
            Sprite planet = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planet.Scale = new Vector2(.7f);
            planet.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1515f);
            Sprite planettwo = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planettwo.Scale = new Vector2(1f);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.8f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .75f);
            Sprites.Add(planet);
            Sprites.Add(planettwo);
            
            Sprites.Add(Title);
            Sprite SinglePlayerButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f), Sprites.SpriteBatch);
            SinglePlayerLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Singleplayer");
            SinglePlayerLabel.Position = new Vector2((SinglePlayerButton.X + SinglePlayerButton.Width / 2) - SinglePlayerLabel.Width/2, (SinglePlayerButton.Y + SinglePlayerButton.Height / 2) - SinglePlayerLabel.Height/2);
            SinglePlayerLabel.IsHoverable = true;
            SinglePlayerLabel.IsManuallySelectable = true;
            SinglePlayerLabel.NonHoverColor = Color.White;
            SinglePlayerLabel.HoverColor = Color.MediumAquamarine;
            SinglePlayerButton.MouseEnter += new EventHandler(SinglePlayerButton_MouseEnter);
            SinglePlayerButton.MouseLeave += new EventHandler(SinglePlayerButton_MouseLeave);

            Sprites.Add(SinglePlayerButton);
            AdditionalSprites.Add(SinglePlayerLabel);

            Sprite MultiPlayerButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);
            MultiPlayerLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Multiplayer");
            MultiPlayerLabel.Position = new Vector2((MultiPlayerButton.X + MultiPlayerButton.Width / 2) - MultiPlayerLabel.Width / 2, (MultiPlayerButton.Y + MultiPlayerButton.Height / 2) - MultiPlayerLabel.Height / 2);
            MultiPlayerLabel.IsHoverable = true;
            MultiPlayerLabel.IsManuallySelectable = true;
            MultiPlayerLabel.NonHoverColor = Color.White;
            MultiPlayerLabel.HoverColor = Color.MediumAquamarine;

            MultiPlayerButton.MouseEnter += new EventHandler(MultiPlayerButton_MouseEnter);
            MultiPlayerButton.MouseLeave += new EventHandler(MultiPlayerButton_MouseLeave);

            Sprites.Add(MultiPlayerButton);
            AdditionalSprites.Add(MultiPlayerLabel);

            Sprite BackButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            BackLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Back");
            BackLabel.Position = new Vector2((BackButton.X + BackButton.Width / 2) - BackLabel.Width / 2, (BackButton.Y + BackButton.Height / 2) - BackLabel.Height / 2);
            BackLabel.Color = Color.White;
            BackButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave += new EventHandler(BackButton_MouseLeave);

            Sprites.Add(BackButton);
            AdditionalSprites.Add(BackLabel);

            Sprite OptionsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f), Sprites.SpriteBatch);
            OptionsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Options");
            OptionsLabel.Position = new Vector2((OptionsButton.X + OptionsButton.Width / 2) - OptionsLabel.Width / 2, (OptionsButton.Y + OptionsButton.Height / 2) - OptionsLabel.Height / 2);
            OptionsLabel.IsHoverable = true;
            OptionsLabel.IsManuallySelectable = true;
            OptionsLabel.NonHoverColor = Color.White;
            OptionsLabel.HoverColor = Color.MediumAquamarine;
            OptionsButton.MouseEnter += new EventHandler(OptionsButton_MouseEnter);
            OptionsButton.MouseLeave += new EventHandler(OptionsButton_MouseLeave);

            Sprites.Add(OptionsButton);
            AdditionalSprites.Add(OptionsLabel);

            Sprite CreditsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);
            CreditsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Credits");
            CreditsLabel.Position = new Vector2((CreditsButton.X + CreditsButton.Width / 2) - CreditsLabel.Width / 2, (CreditsButton.Y + CreditsButton.Height / 2) - CreditsLabel.Height / 2);
            CreditsLabel.IsHoverable = true;
            CreditsLabel.IsManuallySelectable = true;
            CreditsLabel.NonHoverColor = Color.White;
            CreditsLabel.HoverColor = Color.MediumAquamarine;
            CreditsButton.MouseEnter += new EventHandler(CreditsButton_MouseEnter);
            CreditsButton.MouseLeave += new EventHandler(CreditsButton_MouseLeave);

            Sprites.Add(CreditsButton);
            AdditionalSprites.Add(CreditsLabel);

            //temperary
            Sprite ShopButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .598f), Sprites.SpriteBatch);
            ShopLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Shop");
            ShopLabel.Position = new Vector2((ShopButton.X + ShopButton.Width / 2) - ShopLabel.Width / 2, (ShopButton.Y + ShopButton.Height / 2) - ShopLabel.Height / 2);
            ShopLabel.Color = Color.White;
            ShopLabel.IsHoverable = true;
            ShopLabel.IsManuallySelectable = true;
            ShopLabel.NonHoverColor = Color.White;
            ShopLabel.HoverColor = Color.MediumAquamarine;
            ShopButton.MouseEnter += new EventHandler(ShopButton_MouseEnter);
            ShopButton.MouseLeave += new EventHandler(ShopButton_MouseLeave);

            Sprites.Add(ShopButton);
            AdditionalSprites.Add(ShopLabel);
        }

        //shop button
        void ShopButton_MouseLeave(object sender, EventArgs e)
        {
            ShopLabel.IsSelected = false;
        }
        void ShopButton_MouseEnter(object sender, EventArgs e)
        {
            ShopLabel.IsSelected = true;
        }

        //credits button
        void CreditsButton_MouseLeave(object sender, EventArgs e)
        {
            CreditsLabel.IsSelected = false;
        }
        void CreditsButton_MouseEnter(object sender, EventArgs e)
        {
            CreditsLabel.IsSelected = true;
        }


        //options button
        void OptionsButton_MouseLeave(object sender, EventArgs e)
        {
            OptionsLabel.IsSelected = false;
        }
        void OptionsButton_MouseEnter(object sender, EventArgs e)
        {
            OptionsLabel.IsSelected = true;
        }

        public bool mouseInOptionButton
        {
            get
            {
                return OptionsLabel.IsSelected;
            }
        }


        //multiplayer button
        void MultiPlayerButton_MouseLeave(object sender, EventArgs e)
        {
            MultiPlayerLabel.IsSelected = false;
        }
        void MultiPlayerButton_MouseEnter(object sender, EventArgs e)
        {
            MultiPlayerLabel.IsSelected = true;
        }

        bool mouseInBackButton = false;

        //back button
        void BackButton_MouseLeave(object sender, EventArgs e)
        {
            BackLabel.Color = Color.White;
            mouseInBackButton = false;
        }
        void BackButton_MouseEnter(object sender, EventArgs e)
        {
            BackLabel.Color = Color.MediumAquamarine;
            mouseInBackButton = true;
        }

        bool mouseInSingleplayerButton
        {
            get
            {
                return SinglePlayerLabel.IsSelected;
            }
        }

        bool mouseInCreditsButton
        {
            get
            {
                return CreditsLabel.IsSelected;
            }
        }

        //singleplayer button
        void SinglePlayerButton_MouseLeave(object sender, EventArgs e)
        {
            SinglePlayerLabel.IsSelected = false;
        }
        void SinglePlayerButton_MouseEnter(object sender, EventArgs e)
        {
            SinglePlayerLabel.IsSelected = true;
        }

        MouseState lastMs = new MouseState(0,0,0,ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);

        public override void Update(GameTime gameTime)
        {
            //TODO: UPDATE SPRITES
            base.Update(gameTime);
            MouseState currentMs = Mouse.GetState();
            if (lastMs.LeftButton == ButtonState.Released && currentMs.LeftButton == ButtonState.Pressed)
            {
                if (mouseInBackButton)
                {
                    StateManager.ScreenState = ScreenState.Title;
                }
                if (mouseInSingleplayerButton)
                {
                    //TODO: Ship selection screen will choose ship
                    StateManager.InitializeSingleplayerGameScreen<BattleCruiser>(ShipTier.Tier4);
                    StateManager.ScreenState = ScreenState.Game;
                }
                if (mouseInCreditsButton)
                {
                    StateManager.ScreenState = ScreenState.Credits;
                }
                if (mouseInOptionButton)
                {
                    StateManager.ScreenState = ScreenState.Option;
                }
                if (ShopLabel.IsSelected)
                {
                    StateManager.ScreenState = ScreenState.Shop;
                }
            }
            lastMs = currentMs;
        }

    }
}
