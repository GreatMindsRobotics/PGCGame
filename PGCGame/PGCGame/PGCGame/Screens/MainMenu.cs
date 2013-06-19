using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Glib.XNA.SpriteLib;
using Glib.XNA;
using Glib;
using Microsoft.Xna.Framework.Input;

namespace PGCGame.Screens
{
    public class MainMenu : Screen
    {
        public MainMenu(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            
        }

        TextSprite SinglePlayerLabel;
        TextSprite MultiPlayerLabel;
        TextSprite BackLabel;
        TextSprite OptionsLabel;
        TextSprite CreditsLabel;

        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT

            //use Sprites to load your sprites
            //EX: Sprites.Add(new Sprite(content.Load<Texture2D>("assetName"), new Vector2(0, 0), Sprites.SpriteBatch));
            //OR
            //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
            
            Sprite Title = new Sprite(content.Load<Texture2D>("Gametitle"), new Vector2(25), Sprites.SpriteBatch);
            BackgroundSprite = new HorizontalMenuBGSprite(content.Load<Texture2D>("1920by1080SkyStar"), Sprites.SpriteBatch);
            Texture2D planetTexture = content.Load<Texture2D>("Planet");
            Sprite planet = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planet.Scale = new Vector2(.0625f);
            planet.Position = new Vector2(300,321);
            Sprite planettwo = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planettwo.Scale = new Vector2(.125f);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width - 45 - planettwo.Width, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height - 13.5f - planettwo.Height);
            Sprites.Add(planet);
            Sprites.Add(planettwo);
            
            Sprites.Add(Title);
            Sprite SinglePlayerButton = new Sprite(content.Load<Texture2D>("Button"), new Vector2(50, 100), Sprites.SpriteBatch);
            SinglePlayerLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(72, 110), content.Load<SpriteFont>("TitleFont"), "Singleplayer");
            SinglePlayerLabel.IsHoverable = true;
            SinglePlayerLabel.IsManuallySelectable = true;
            SinglePlayerLabel.NonHoverColor = Color.White;
            SinglePlayerLabel.HoverColor = Color.MediumAquamarine;
            SinglePlayerButton.MouseEnter += new EventHandler(SinglePlayerButton_MouseEnter);
            SinglePlayerButton.MouseLeave += new EventHandler(SinglePlayerButton_MouseLeave);

            Sprites.Add(SinglePlayerButton);
            AdditionalSprites.Add(SinglePlayerLabel);

            Sprite MultiPlayerButton = new Sprite(content.Load<Texture2D>("Button"), new Vector2(50, 195), Sprites.SpriteBatch);
            MultiPlayerLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(78, 205), content.Load<SpriteFont>("TitleFont"), "Multiplayer");
            MultiPlayerLabel.IsHoverable = true;
            MultiPlayerLabel.IsManuallySelectable = true;
            MultiPlayerLabel.NonHoverColor = Color.White;
            MultiPlayerLabel.HoverColor = Color.MediumAquamarine;

            MultiPlayerButton.MouseEnter += new EventHandler(MultiPlayerButton_MouseEnter);
            MultiPlayerButton.MouseLeave += new EventHandler(MultiPlayerButton_MouseLeave);

            Sprites.Add(MultiPlayerButton);
            AdditionalSprites.Add(MultiPlayerLabel);  

            Sprite BackButton = new Sprite(content.Load<Texture2D>("Button"), new Vector2(50, 290), Sprites.SpriteBatch);
            BackLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(115, 299), content.Load<SpriteFont>("TitleFont"), "Back");
            BackLabel.Color = Color.White;
            BackButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave += new EventHandler(BackButton_MouseLeave);

            Sprites.Add(BackButton);
            AdditionalSprites.Add(BackLabel);

            Sprite OptionsButton = new Sprite(content.Load<Texture2D>("Button"), new Vector2(290, 100), Sprites.SpriteBatch);
            OptionsLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(340, 110), content.Load<SpriteFont>("TitleFont"), "Options");
            OptionsLabel.IsHoverable = true;
            OptionsLabel.IsManuallySelectable = true;
            OptionsLabel.NonHoverColor = Color.White;
            OptionsLabel.HoverColor = Color.MediumAquamarine;
            OptionsButton.MouseEnter += new EventHandler(OptionsButton_MouseEnter);
            OptionsButton.MouseLeave += new EventHandler(OptionsButton_MouseLeave);

            Sprites.Add(OptionsButton);
            AdditionalSprites.Add(OptionsLabel);

            Sprite CreditsButton = new Sprite(content.Load<Texture2D>("Button"), new Vector2(290, 195), Sprites.SpriteBatch);
            CreditsLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(340, 205), content.Load<SpriteFont>("TitleFont"), "Credits");
            CreditsLabel.IsHoverable = true;
            CreditsLabel.IsManuallySelectable = true;
            CreditsLabel.NonHoverColor = Color.White;
            CreditsLabel.HoverColor = Color.MediumAquamarine;
            CreditsButton.MouseEnter += new EventHandler(CreditsButton_MouseEnter);
            CreditsButton.MouseLeave += new EventHandler(CreditsButton_MouseLeave);

            Sprites.Add(CreditsButton);
            AdditionalSprites.Add(CreditsLabel);
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
                    StateManager.ScreenState = ScreenState.Game;
                }
                if (mouseInCreditsButton)
                {
                    StateManager.ScreenState = ScreenState.Credits;
                }
            }
            lastMs = currentMs;
        }

    }
}
