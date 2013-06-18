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

        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT
            
            //use Sprites to load your sprites
            TitleImage = new Sprite(content.Load<Texture2D>("Gametitle"), new Vector2(150, 100), Sprites.SpriteBatch);
            Sprites.Add(TitleImage);

            PlayButton = new Sprite(content.Load<Texture2D>("Button"), new Vector2(300, 200), Sprites.SpriteBatch);
            PlayButton.MouseEnter += new EventHandler(PlayButton_MouseEnter);
            PlayButton.MouseLeave += new EventHandler(PlayButton_MouseLeave);
            Sprites.Add(PlayButton);

            PlayLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(360, 210), content.Load<SpriteFont>("TitleFont"), "Play");
            PlayLabel.IsHoverable = true;
            PlayLabel.IsManuallySelectable = true;
            PlayLabel.NonHoverColor = Color.White;
            PlayLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(PlayLabel);


            ExitButton = new Sprite(content.Load<Texture2D>("Button"), new Vector2(300, 300), Sprites.SpriteBatch);
            ExitButton.MouseEnter += new EventHandler(ExitButton_MouseEnter);
            ExitButton.MouseLeave += new EventHandler(ExitButton_MouseLeave);
            Sprites.Add(ExitButton);

            ExitLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(360, 310), content.Load<SpriteFont>("TitleFont"), "Exit");
            ExitLabel.IsHoverable = true;
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
