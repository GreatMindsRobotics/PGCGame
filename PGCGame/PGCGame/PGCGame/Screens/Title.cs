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
    public class Title : Screen
    {
        Sprite TitleImage;

        public delegate void quitFunction();

        Sprite PlayButton;
        TextSprite PlayLabel;

        Sprite ExitButton;
        TextSprite ExitLabel;
        quitFunction exit;


        public Title(SpriteBatch spriteBatch, quitFunction exitableFunction)
            : base(spriteBatch, Color.Black)
        {
            exit = exitableFunction;
        }

        float labelYOffset = 0.895f;

        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT
            
            //use Sprites to load your sprites
            this.BackgroundSprite = new HorizontalMenuBGSprite(content.Load<Texture2D>("Images\\Background\\1920by1080SkyStar"), Sprites.SpriteBatch);
            Texture2D planetTexture = content.Load<Texture2D>("Images\\NonPlayingObject\\Planet");
            Sprite planet = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planet.Scale = new Vector2(.0625f);
            planet.Position = new Vector2(300, 321);
            Sprite planettwo = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planettwo.Scale = new Vector2(.125f);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width - 45 - planettwo.Width, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height - 13.5f - planettwo.Height);
            Sprites.Add(planet);
            Sprites.Add(planettwo);

            Viewport viewPort = Sprites.SpriteBatch.GraphicsDevice.Viewport;

            TitleImage = new Sprite(content.Load<Texture2D>("Images\\Controls\\Gametitle"), Vector2.Zero, Sprites.SpriteBatch);
            TitleImage.Position = new Vector2(viewPort.Width / 2 - TitleImage.Texture.Width / 2, viewPort.Height * .2f);
            Sprites.Add(TitleImage);

            PlayButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(viewPort.Width * 0.375f, viewPort.Height * 0.4f), Sprites.SpriteBatch);
            PlayButton.MouseEnter += new EventHandler(PlayButton_MouseEnter);
            PlayButton.MouseLeave += new EventHandler(PlayButton_MouseLeave);
            Sprites.Add(PlayButton);

            PlayLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Play");
            PlayLabel.Position = new Vector2(PlayButton.X + (PlayButton.Width / 2 - PlayLabel.Width / 2), PlayButton.Y + (PlayButton.Height / 2 - PlayLabel.Height / 2 - (labelYOffset * PlayButton.Scale.Y)));
            PlayLabel.IsHoverable = true;
            PlayLabel.IsManuallySelectable = true;
            PlayLabel.NonHoverColor = Color.White;
            PlayLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(PlayLabel);


            ExitButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(300, 300), Sprites.SpriteBatch);
            ExitButton.MouseEnter += new EventHandler(ExitButton_MouseEnter);
            ExitButton.MouseLeave += new EventHandler(ExitButton_MouseLeave);
            Sprites.Add(ExitButton);

            ExitLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(360, 310), content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Exit");
            ExitLabel.IsHoverable = true;
            ExitLabel.Position = new Vector2(ExitButton.X + (ExitButton.Width / 2 - ExitLabel.Width / 2), ExitButton.Y + (ExitButton.Height / 2 - ExitLabel.Height / 2 - (labelYOffset * ExitButton.Scale.Y)));
            ExitLabel.IsManuallySelectable = true;
            ExitLabel.NonHoverColor = Color.White;
            ExitLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(ExitLabel);

            
            //OR
            //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
        }

        void PlayButton_MouseLeave(object sender, EventArgs e)
        {
            PlayLabel.IsSelected = false;
        }

        void PlayButton_MouseEnter(object sender, EventArgs e)
        {
            PlayLabel.IsSelected = true;
        }

        bool mouseInExitButton
        {
            get
            {
                return ExitLabel.IsSelected;
            }
        }

        bool mouseInPlayButton
        {
            get
            {
                return PlayLabel.IsSelected;
            }
        }

        void ExitButton_MouseLeave(object sender, EventArgs e)
        {
            ExitLabel.IsSelected = false;
        }

        void ExitButton_MouseEnter(object sender, EventArgs e)
        {
            ExitLabel.IsSelected = true;
        }

        public override void Update(GameTime gameTime)
        {
            //TODO: UPDATE SPRITES
            base.Update(gameTime);

            if (mouseInExitButton || mouseInPlayButton)
            {
                MouseState ms = Mouse.GetState();
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    if (mouseInExitButton)
                    {
                        exit();
                    }
                    if (mouseInPlayButton)
                    {
                        StateManager.ScreenState = ScreenState.MainMenu;
                    }
                }
            }
        }
    }
}
