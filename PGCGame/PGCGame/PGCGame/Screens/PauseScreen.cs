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
using Glib.XNA.InputLib;

namespace PGCGame.Screens
{
    public class PauseScreen : BaseScreen
    {
#if XBOX
        GamePadButtonEnumerator AllButtons;
#endif

        TextSprite ExitLabel;
        TextSprite PauseLabel;
        TextSprite ResumeLabel;
        TextSprite ShopLabel;
        Sprite ResumeButton;
        TextSprite OptionsLabel;
        Sprite ExitButton;
        Sprite ShopButton;

        public PauseScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {

        }

        public override void InitScreen(ScreenType screenType)
        {
            base.InitScreen(screenType);

            Texture2D button = GameContent.GameAssets.Images.Controls.Button;

            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

            PauseLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.BoldText, "Paused");
            PauseLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width / 2 - PauseLabel.Width / 2, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f);
            PauseLabel.Color = Color.White;
            AdditionalSprites.Add(PauseLabel);


            ResumeButton = new Sprite(button, Vector2.Zero, Sprites.SpriteBatch);
            ResumeButton.Position = new Vector2(ResumeButton.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .2f);
            Sprites.Add(ResumeButton);

            ResumeLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "Resume");
            ResumeLabel.ParentSprite = ResumeButton;
            ResumeLabel.Color = Color.White;
            ResumeLabel.IsHoverable = true;
            ResumeLabel.HoverColor = Color.MediumAquamarine;
            ResumeLabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(ResumeLabel);

            ExitButton = new Sprite(button, new Vector2(ResumeButton.X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .8f), Sprites.SpriteBatch);
            Sprites.Add(ExitButton);

            

            ExitLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "Exit");
            ExitLabel.Color = Color.White;
            ExitLabel.IsHoverable = true;
            ExitLabel.ParentSprite = ExitButton;
            ExitLabel.HoverColor = Color.MediumAquamarine;
            ExitLabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(ExitLabel);

            ShopButton = new Sprite(button, Vector2.Zero, Sprites.SpriteBatch);
            ShopButton.Position = new Vector2(ResumeButton.X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .53f);
            Sprites.Add(ShopButton);

            ShopLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "Shop");
            ShopLabel.Color = Color.White;
            ShopLabel.IsHoverable = true;
            ShopLabel.ParentSprite = ShopButton;
            ShopLabel.HoverColor = Color.MediumAquamarine;
            ShopLabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(ShopLabel);

            OptionButton = new Sprite(button, Vector2.Zero, Sprites.SpriteBatch);
            OptionButton.Position = new Vector2(ResumeButton.X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .37f);
            Sprites.Add(OptionButton);

            OptionsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "Options");
            OptionsLabel.ParentSprite = OptionButton;
            OptionsLabel.Color = Color.White;
            OptionsLabel.IsHoverable = true;
            OptionsLabel.HoverColor = Color.MediumAquamarine;
            OptionsLabel.NonHoverColor = Color.White;
            AdditionalSprites.Add(OptionsLabel);
#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,]
                {
                    {ResumeLabel},
                    {OptionsLabel},
                    {ShopLabel},
                    {ExitLabel}
                }, InputType.LeftJoystick);
            AllButtons.FireTextSpritePressed = true;

            ResumeLabel.Pressed += new EventHandler(ResumeLabel_Pressed);
            ExitLabel.Pressed += new EventHandler(ExitLabel_Pressed);
            ShopLabel.Pressed += new EventHandler(ShopLabel_Pressed);
            OptionsLabel.Pressed += new EventHandler(OptionsLabel_Pressed);
#elif WINDOWS
            ResumeLabel.Clicked += new EventHandler(ResumeLabel_Pressed);
            ExitLabel.Clicked += new EventHandler(ExitLabel_Pressed);
            ShopLabel.Clicked += new EventHandler(ShopLabel_Pressed);
            OptionsLabel.Clicked += new EventHandler(OptionsLabel_Pressed);
#endif
        }

        void OptionsLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.ScreenState = ScreenType.Options;
            OptionsLabel.IsSelected = false;
        }

        void ShopLabel_Pressed(object sender, EventArgs e)
        {
            //If we previously upgraded the ship, refresh shop screen to reflect the upgrade
            if (StateManager.IsShipUpgraded)
            {
                StateManager.AllScreens[ScreenType.TierSelect.ToString()].Cast<BaseSelectScreen>().InitScreen(CoreTypes.ScreenType.TierSelect);
                StateManager.IsShipUpgraded = false;
            }

            //Go to shop
            StateManager.ScreenState = ScreenType.Shop;
            ShopLabel.IsSelected = false;
        }

        void ExitLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.ScreenState = ScreenType.MainMenu;
            StateManager.Reset();
            StateManager.ActiveShips.Clear();
        }

        void ResumeLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.GoBack();
        }

        public KeyboardState lastState;
        Sprite OptionButton;

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            //relocate buttons and labels on the screen!

            PauseLabel.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width / 2 - PauseLabel.Width / 2, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f);

            ResumeButton.Position = new Vector2(ResumeButton.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .2f);
            ExitButton.Position = new Vector2(ResumeButton.X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .8f);
            ExitLabel.Position = new Vector2(ExitLabel.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, ExitButton.Y + (ExitButton.Height / 2 - ExitLabel.Height / 2));
            ShopButton.Position = new Vector2(ResumeButton.X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .53f);
            ResumeLabel.Position = new Vector2(ResumeLabel.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, ResumeButton.Y + (ResumeButton.Height / 2 - ResumeLabel.Height / 2));
            OptionButton.Position = new Vector2(ResumeButton.X, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .37f);
            ShopLabel.Position = new Vector2(ShopLabel.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, ShopButton.Y + (ShopButton.Height / 2 - ShopLabel.Height / 2));
            OptionsLabel.Position = new Vector2(OptionsLabel.GetCenterPosition(Sprites.SpriteBatch.GraphicsDevice.Viewport).X, OptionButton.Y + (OptionButton.Height / 2 - OptionsLabel.Height / 2));
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState current = Keyboard.GetState();
            if (lastState.IsKeyUp(Keys.Escape) && current.IsKeyDown(Keys.Escape))
            {
                StateManager.GoBack();
                return;
            }
#if XBOX
            AllButtons.Update(gameTime);
#endif
            lastState = current;
        }
    }
}
