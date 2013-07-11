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

using PGCGame.CoreTypes;
using Glib.XNA.InputLib;

namespace PGCGame.Screens
{
    public class Title : BaseScreen
    {
        private Delegates.QuitFunction _exit;

        public Title(SpriteBatch spriteBatch, Delegates.QuitFunction exitableFunction)
            : base(spriteBatch, Color.Black)
        {
            _exit = exitableFunction;
        }

        Sprite TitleImage;

        Sprite planet;
        Sprite planettwo;

        Sprite PlayButton;
        TextSprite PlayLabel;

        Sprite ExitButton;
        TextSprite ExitLabel;

#if XBOX
        GamePadButtonEnumerator ButtonManagement;
#endif


        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);

            Viewport viewPort = Sprites.SpriteBatch.GraphicsDevice.Viewport;

            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            Texture2D planetTexture = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
            Texture2D buttonTexture = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;
            Texture2D altPlanetTexture = GameContent.GameAssets.Images.NonPlayingObjects.AltPlanet;

            planet = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planet.Scale = new Vector2(.7f);
            planet.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .16f);
            Sprites.Add(planet);

            planettwo = new Sprite(altPlanetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planettwo.Scale = new Vector2(1f);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.8f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .75f);
            Sprites.Add(planettwo);

            //title image
            TitleImage = new Sprite(GameContent.GameAssets.Images.Controls.Title, Vector2.Zero, Sprites.SpriteBatch);
            TitleImage.Position = new Vector2(viewPort.Width / 2 - TitleImage.Texture.Width / 2, viewPort.Height * 0.2f);
            Sprites.Add(TitleImage);

            PlayButton = new Sprite(buttonTexture, new Vector2(viewPort.Width * 0.375f, viewPort.Height * 0.4f), Sprites.SpriteBatch);
            Sprites.Add(PlayButton);

            PlayLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Play");
            
            PlayLabel.IsHoverable = true;
            

            PlayLabel.NonHoverColor = Color.White;
            PlayLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(PlayLabel);


            ExitButton = new Sprite(buttonTexture, new Vector2(PlayButton.X, PlayButton.Y + (PlayButton.Height * 1.6f)), Sprites.SpriteBatch);
            
            Sprites.Add(ExitButton);

            ExitLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Exit");
            
            ExitLabel.IsHoverable = true;
            
            ExitLabel.NonHoverColor = Color.White;
            ExitLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(ExitLabel);

#if WINDOWS
            ExitLabel.ParentSprite = ExitButton;
            ExitLabel.Clicked += new EventHandler(ExitLabel_Clicked);
            PlayLabel.Clicked += new EventHandler(PlayLabel_Clicked);
            PlayLabel.ParentSprite = PlayButton;
#elif XBOX
            PlayLabel.Position = new Vector2(PlayButton.X + (PlayButton.Width / 2 - PlayLabel.Width / 2), PlayButton.Y + (PlayButton.Height / 2 - PlayLabel.Height / 2));
            ExitLabel.Position = new Vector2(ExitButton.X + (ExitButton.Width / 2 - ExitLabel.Width / 2), ExitButton.Y + (ExitButton.Height / 2 - ExitLabel.Height / 2));


            ButtonManagement = new GamePadButtonEnumerator(new TextSprite[,] { { PlayLabel }, { ExitLabel } }, InputType.LeftJoystick);
            ButtonManagement.ButtonPress += new EventHandler(ButtonManagement_ButtonPress);
#endif
        }

        void ExitLabel_Clicked(object sender, EventArgs e)
        {
            _exit();
        }

        void PlayLabel_Clicked(object sender, EventArgs e)
        {
            StateManager.ScreenState = CoreTypes.ScreenType.MainMenu;
        }

#if XBOX
        void ButtonManagement_ButtonPress(object sender, EventArgs e)
        {
            if (ExitLabel.IsSelected)
            {
                _exit();
            }
            else if (PlayLabel.IsSelected)
            {
                StateManager.ScreenState = ScreenType.MainMenu;
            }
        }

        TimeSpan elapsedControllerDelayTime = TimeSpan.Zero;
        TimeSpan totalControllerDelayTime = TimeSpan.FromMilliseconds(250);
#endif

        public override void Update(GameTime gameTime)
        {
            if (!StateManager.IsWindowFocused())
            {
                //Not active window
                return;
            }
            
#if XBOX
            ButtonManagement.Update(gameTime);
#endif
            base.Update(gameTime);
        }
    }
}
