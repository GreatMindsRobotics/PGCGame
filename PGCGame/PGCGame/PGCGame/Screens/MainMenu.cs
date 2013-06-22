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
        Sprite planet;
        Sprite planettwo;
        TextSprite CreditsLabel;

        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT

            //use Sprites to load your sprites
            //EX: Sprites.Add(new Sprite(content.Load<Texture2D>("assetName"), new Vector2(0, 0), Sprites.SpriteBatch));
            //OR
            //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
            Texture2D buttonImage = content.Load<Texture2D>("Images\\Controls\\Button");
            SpriteFont SegoeUIMono = content.Load<SpriteFont>("Fonts\\SegoeUIMono");

            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

            TitleSprite = new Sprite(content.Load<Texture2D>("Images\\Controls\\Gametitle"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width*.05f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height*.07f), Sprites.SpriteBatch);

            this.BackgroundSprite = new HorizontalMenuBGSprite(content.Load<Texture2D>("Images\\Background\\1920by1080SkyStar"), Sprites.SpriteBatch);

            Texture2D planetTexture = content.Load<Texture2D>("Images\\NonPlayingObject\\Planet");
            planet = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planet.Scale = new Vector2(.7f);
            planet.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1515f);
            planettwo = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planettwo.Scale = new Vector2(1f);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.8f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .75f);
            Sprites.Add(planet);
            Sprites.Add(planettwo);

            Sprites.Add(TitleSprite);
            SinglePlayerButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f), Sprites.SpriteBatch);
            SinglePlayerLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Singleplayer");
            SinglePlayerButton.Moved += new EventHandler(delegate(object src, EventArgs v) { SinglePlayerLabel.Position = new Vector2((SinglePlayerButton.X + SinglePlayerButton.Width / 2) - SinglePlayerLabel.Width / 2, (SinglePlayerButton.Y + SinglePlayerButton.Height / 2) - SinglePlayerLabel.Height / 2); });
            SinglePlayerLabel.Position = new Vector2((SinglePlayerButton.X + SinglePlayerButton.Width / 2) - SinglePlayerLabel.Width/2, (SinglePlayerButton.Y + SinglePlayerButton.Height / 2) - SinglePlayerLabel.Height/2);
            SinglePlayerLabel.IsHoverable = true;
            SinglePlayerLabel.IsManuallySelectable = true;
            SinglePlayerLabel.NonHoverColor = Color.White;
            SinglePlayerLabel.HoverColor = Color.MediumAquamarine;
            SinglePlayerButton.MouseEnter += new EventHandler(SinglePlayerButton_MouseEnter);
            SinglePlayerButton.MouseLeave += new EventHandler(SinglePlayerButton_MouseLeave);

            Sprites.Add(SinglePlayerButton);
            AdditionalSprites.Add(SinglePlayerLabel);

            MultiPlayerButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);
            MultiPlayerLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Multiplayer");
            MultiPlayerButton.Moved += new EventHandler(delegate(object src, EventArgs v) {
                new Vector2((MultiPlayerButton.X + MultiPlayerButton.Width / 2) - MultiPlayerLabel.Width / 2, (MultiPlayerButton.Y + MultiPlayerButton.Height / 2) - MultiPlayerLabel.Height / 2);
            });
            MultiPlayerLabel.Position = new Vector2((MultiPlayerButton.X + MultiPlayerButton.Width / 2) - MultiPlayerLabel.Width / 2, (MultiPlayerButton.Y + MultiPlayerButton.Height / 2) - MultiPlayerLabel.Height / 2);
            MultiPlayerLabel.IsHoverable = true;
            MultiPlayerLabel.IsManuallySelectable = true;
            MultiPlayerLabel.NonHoverColor = Color.White;
            MultiPlayerLabel.HoverColor = Color.MediumAquamarine;

            MultiPlayerButton.MouseEnter += new EventHandler(MultiPlayerButton_MouseEnter);
            MultiPlayerButton.MouseLeave += new EventHandler(MultiPlayerButton_MouseLeave);

            Sprites.Add(MultiPlayerButton);
            AdditionalSprites.Add(MultiPlayerLabel);

            BackButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            BackLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Back");
            BackButton.Moved += new EventHandler(delegate(object src, EventArgs v) { BackLabel.Position = new Vector2((BackButton.X + BackButton.Width / 2) - BackLabel.Width / 2, (BackButton.Y + BackButton.Height / 2) - BackLabel.Height / 2); });
            BackLabel.Position = new Vector2((BackButton.X + BackButton.Width / 2) - BackLabel.Width / 2, (BackButton.Y + BackButton.Height / 2) - BackLabel.Height / 2);
            BackLabel.Color = Color.White;
            BackButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave += new EventHandler(BackButton_MouseLeave);

            Sprites.Add(BackButton);
            AdditionalSprites.Add(BackLabel);

            OptionsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f), Sprites.SpriteBatch);
            OptionsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Options");
            OptionsButton.Moved += new EventHandler(delegate(object src, EventArgs ea) { new Vector2((OptionsButton.X + OptionsButton.Width / 2) - OptionsLabel.Width / 2, (OptionsButton.Y + OptionsButton.Height / 2) - OptionsLabel.Height / 2); });
            OptionsLabel.Position = new Vector2((OptionsButton.X + OptionsButton.Width / 2) - OptionsLabel.Width / 2, (OptionsButton.Y + OptionsButton.Height / 2) - OptionsLabel.Height / 2);
            OptionsLabel.IsHoverable = true;
            OptionsLabel.IsManuallySelectable = true;
            OptionsLabel.NonHoverColor = Color.White;
            OptionsLabel.HoverColor = Color.MediumAquamarine;
            OptionsButton.MouseEnter += new EventHandler(OptionsButton_MouseEnter);
            OptionsButton.MouseLeave += new EventHandler(OptionsButton_MouseLeave);

            Sprites.Add(OptionsButton);
            AdditionalSprites.Add(OptionsLabel);

            CreditsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);
            CreditsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Credits");
            CreditsButton.Moved += new EventHandler(delegate(object src, EventArgs ea)
            {
                CreditsLabel.Position = new Vector2((CreditsButton.X + CreditsButton.Width / 2) - CreditsLabel.Width / 2, (CreditsButton.Y + CreditsButton.Height / 2) - CreditsLabel.Height / 2);
            });
            CreditsLabel.Position = new Vector2((CreditsButton.X + CreditsButton.Width / 2) - CreditsLabel.Width / 2, (CreditsButton.Y + CreditsButton.Height / 2) - CreditsLabel.Height / 2);
            CreditsLabel.IsHoverable = true;
            CreditsLabel.IsManuallySelectable = true;
            CreditsLabel.NonHoverColor = Color.White;
            CreditsLabel.HoverColor = Color.MediumAquamarine;
            CreditsButton.MouseEnter += new EventHandler(CreditsButton_MouseEnter);
            CreditsButton.MouseLeave += new EventHandler(CreditsButton_MouseLeave);

            Sprites.Add(CreditsButton);
            AdditionalSprites.Add(CreditsLabel);
        }

        Sprite CreditsButton;
        Sprite BackButton;
        Sprite TitleSprite;
        Sprite OptionsButton;
        Sprite SinglePlayerButton;
        Sprite MultiPlayerButton;

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            planet.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1515f);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.8f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .75f);
            TitleSprite.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .05f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .07f);
            CreditsButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f);
            BackButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f);
            SinglePlayerButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f);
            MultiPlayerButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f);
            OptionsButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f);
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
                    //StateManager.InitializeSingleplayerGameScreen<BattleCruiser>(ShipTier.Tier4);
                    StateManager.ScreenState = ScreenState.ShipSelect;
                }
                if (mouseInCreditsButton)
                {
                    StateManager.ScreenState = ScreenState.Credits;
                }
                if (mouseInOptionButton)
                {
                    StateManager.ScreenState = ScreenState.Option;
                }
            }
            lastMs = currentMs;
        }

    }
}
