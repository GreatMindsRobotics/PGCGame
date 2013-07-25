using System;

using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.Input;
using Glib.XNA.InputLib;

namespace PGCGame.Screens
{
    public class GameOver : BaseScreen
    {
        public GameOver(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            //TODO: BACKGROUND
        }

        TextSprite mainMenuLabel;
        TextSprite gameOverLabel;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);

            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            gameOverLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f), SegoeUIMono, string.Format("GAME OVER\n you had {0} points,\n {1} spacebucks,\n and was on {2}", StateManager.SpacePoints, StateManager.SpaceBucks, StateManager.level));
            gameOverLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f - gameOverLabel.Width / 2, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f - gameOverLabel.Height / 2);
            gameOverLabel.Color = Color.Red;

            Sprite MainMenuButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .5f), Sprites.SpriteBatch);
            MainMenuButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f - MainMenuButton.Width / 2, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .5f - MainMenuButton.Height / 2);
            mainMenuLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Main Menu");
            mainMenuLabel.Position = new Vector2((MainMenuButton.X + MainMenuButton.Width / 2) - mainMenuLabel.Width / 2, (MainMenuButton.Y + MainMenuButton.Height / 2) - mainMenuLabel.Height / 2);
#if WINDOWS
            mainMenuLabel.CallKeyboardClickEvent = false;
#endif
            mainMenuLabel.Color = Color.White;
            mainMenuLabel.IsHoverable = true;
            mainMenuLabel.IsManuallySelectable = true;
            mainMenuLabel.NonHoverColor = Color.White;
            mainMenuLabel.HoverColor = Color.MediumAquamarine;

            AdditionalSprites.Add(mainMenuLabel);
            Sprites.Add(MainMenuButton);
            AdditionalSprites.Add(gameOverLabel);

#if WINDOWS
            MainMenuButton.MouseEnter +=new EventHandler(MainMenuButton_MouseEnter);
            MainMenuButton.MouseLeave +=new EventHandler(MainMenuButton_MouseLeave);
#endif
        }

#if WINDOWS
        bool mouseInMainMenuButton = false;

        void MainMenuButton_MouseLeave(object sender, EventArgs e)
        {
            mainMenuLabel.IsSelected = false;
            mouseInMainMenuButton = false;
        }
        void MainMenuButton_MouseEnter(object sender, EventArgs e)
        {
            mainMenuLabel.IsSelected = true;
            mouseInMainMenuButton = true;

        }
        
        MouseState lastMs = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
#endif

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
#if WINDOWS
            MouseState currentMs = MouseManager.CurrentMouseState;
            if (currentMs.LeftButton == ButtonState.Pressed)
            {
                if (mouseInMainMenuButton && this.Visible)
                {
                    StateManager.Reset();
                    StateManager.ScreenState = ScreenType.MainMenu;
                }
                lastMs = currentMs;
            }
#endif
        }
    }
}