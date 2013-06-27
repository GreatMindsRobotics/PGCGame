﻿using System;
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

namespace PGCGame.Screens
{
    public class Title : Screen
    {
        public delegate void quitFunction();

        quitFunction exit;

        public Title(SpriteBatch spriteBatch, quitFunction exitableFunction)
            : base(spriteBatch, Color.Black)
        {
            exit = exitableFunction;
        }

        private const float labelYOffset = 0.895f;

        Sprite TitleImage;

        Sprite planet;
        Sprite planettwo;

        Sprite PlayButton;
        TextSprite PlayLabel;

        Sprite ExitButton;
        TextSprite ExitLabel;


        public void LoadContent(ContentManager content)
        {
            Viewport viewPort = Sprites.SpriteBatch.GraphicsDevice.Viewport;
            
            this.BackgroundSprite = new HorizontalMenuBGSprite(content.Load<Texture2D>("Images\\Background\\1920by1080SkyStar"), Sprites.SpriteBatch);

            //loading the content ONCE
            Texture2D planetTexture = content.Load<Texture2D>("Images\\NonPlayingObject\\Planet");
            Texture2D buttonTexture = content.Load<Texture2D>("Images\\Controls\\Button");
            SpriteFont SegoeUIMono = content.Load<SpriteFont>("Fonts\\SegoeUIMono");

            planet = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planet.Scale = new Vector2(.7f);
            planet.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.1f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .16f);
            Sprites.Add(planet);

            planettwo = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planettwo.Scale = new Vector2(1f);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.8f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .75f);
            Sprites.Add(planettwo);

            //title image
            TitleImage = new Sprite(content.Load<Texture2D>("Images\\Controls\\Gametitle"), Vector2.Zero, Sprites.SpriteBatch);
            TitleImage.Position = new Vector2(viewPort.Width / 2 - TitleImage.Texture.Width / 2, viewPort.Height * 0.2f);
            Sprites.Add(TitleImage);

            PlayButton = new Sprite(buttonTexture, new Vector2(viewPort.Width * 0.375f, viewPort.Height * 0.4f), Sprites.SpriteBatch);
            PlayButton.MouseEnter += new EventHandler(PlayButton_MouseEnter);
            PlayButton.MouseLeave += new EventHandler(PlayButton_MouseLeave);
            Sprites.Add(PlayButton);

            PlayLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Play");
            PlayLabel.Position = new Vector2(PlayButton.X + (PlayButton.Width / 2 - PlayLabel.Width / 2), PlayButton.Y + (PlayButton.Height / 2 - PlayLabel.Height / 2));
            PlayLabel.IsHoverable = true;
            PlayLabel.IsManuallySelectable = true;
            PlayLabel.NonHoverColor = Color.White;
            PlayLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(PlayLabel);


            ExitButton = new Sprite(buttonTexture, new Vector2(PlayButton.X, PlayButton.Y + (PlayButton.Height * 1.6f)), Sprites.SpriteBatch);
            ExitButton.MouseEnter += new EventHandler(ExitButton_MouseEnter);
            ExitButton.MouseLeave += new EventHandler(ExitButton_MouseLeave);
            Sprites.Add(ExitButton);

            ExitLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Exit");
            ExitLabel.Position = new Vector2(ExitButton.X + (ExitButton.Width / 2 - ExitLabel.Width / 2), ExitButton.Y + (ExitButton.Height / 2 - ExitLabel.Height / 2));
            ExitLabel.IsHoverable = true;
            ExitLabel.IsManuallySelectable = true;
            ExitLabel.NonHoverColor = Color.White;
            ExitLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(ExitLabel);
        }

        void PlayButton_MouseLeave(object sender, EventArgs e)
        {
            PlayLabel.IsSelected = false;
        }

        void PlayButton_MouseEnter(object sender, EventArgs e)
        {
            PlayLabel.IsSelected = true;
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
            if (PlayLabel.IsSelected && PlayButton.ClickCheck())
            {
                StateManager.ScreenState = ScreenState.MainMenu;
            }
            else if (ExitLabel.IsSelected && ExitButton.ClickCheck())
            {
                exit();
            }

            base.Update(gameTime);
        }
    }
}
