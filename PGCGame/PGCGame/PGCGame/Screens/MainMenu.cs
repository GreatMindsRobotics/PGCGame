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

using PGCGame.CoreTypes;

namespace PGCGame.Screens
{
    public class MainMenu : Screen
    {
        private static bool _debugBackground = false;
        public static bool DebugBackground
        {
            get 
            { 
                return _debugBackground; 
            }
        }

        PGCGame.Screens.Title.quitFunction exit;

        public MainMenu(SpriteBatch spriteBatch, PGCGame.Screens.Title.quitFunction exit)
            : base(spriteBatch, _debugBackground ? Color.Red : Color.Black)
        {
            this.exit = exit;
        }

        Sprite TitleSprite;

        

        Sprite planet;
        Sprite planettwo;
        
        Sprite SinglePlayerButton;
        TextSprite SinglePlayerLabel;

        Sprite MultiPlayerButton;
        TextSprite MultiPlayerLabel;

        Sprite BackButton;
        TextSprite BackLabel;

        Sprite OptionsButton;
        TextSprite OptionsLabel;

        Sprite CreditsButton;
        TextSprite CreditsLabel;


        public void LoadContent(ContentManager content)
        {
            Texture2D planetTexture = content.Load<Texture2D>("Images\\NonPlayingObject\\Planet");
            Texture2D buttonImage = content.Load<Texture2D>("Images\\Controls\\Button");
            SpriteFont SegoeUIMono = content.Load<SpriteFont>("Fonts\\SegoeUIMono");

            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            TitleSprite = new Sprite(content.Load<Texture2D>("Images\\Controls\\Gametitle"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .05f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .07f), Sprites.SpriteBatch);
            Sprites.Add(TitleSprite);

            planet = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planet.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1515f);
            planet.Scale = new Vector2(.7f);
            Sprites.Add(planet);
            
            planettwo = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.8f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .75f);
            planettwo.Scale = new Vector2(1f);
            Sprites.Add(planettwo);

            
            SinglePlayerButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f), Sprites.SpriteBatch);
            SinglePlayerButton.Moved += new EventHandler(delegate(object src, EventArgs v) 
                {
                    SinglePlayerLabel.Position = new Vector2(SinglePlayerButton.X + (SinglePlayerButton.Width / 2 - SinglePlayerLabel.Width / 2), SinglePlayerButton.Y + (SinglePlayerButton.Height / 2 - SinglePlayerLabel.Height / 2)); 
                });
            SinglePlayerButton.MouseEnter += new EventHandler(SinglePlayerButton_MouseEnter);
            SinglePlayerButton.MouseLeave += new EventHandler(SinglePlayerButton_MouseLeave);
            Sprites.Add(SinglePlayerButton);

            SinglePlayerLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Singleplayer");
            SinglePlayerLabel.Position = new Vector2(SinglePlayerButton.X + (SinglePlayerButton.Width / 2 - SinglePlayerLabel.Width / 2), SinglePlayerButton.Y + (SinglePlayerButton.Height / 2 - SinglePlayerLabel.Height / 2));
            SinglePlayerLabel.IsHoverable = true;
            SinglePlayerLabel.IsManuallySelectable = true;
            SinglePlayerLabel.NonHoverColor = Color.White;
            SinglePlayerLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(SinglePlayerLabel);
            

            MultiPlayerButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);
            MultiPlayerButton.Moved += new EventHandler(delegate(object src, EventArgs v)
            {
                MultiPlayerLabel.Position = new Vector2(MultiPlayerButton.X + (MultiPlayerButton.Width / 2 - MultiPlayerLabel.Width / 2), MultiPlayerButton.Y + (MultiPlayerButton.Height / 2 - MultiPlayerLabel.Height / 2));
            });
            MultiPlayerButton.MouseEnter += new EventHandler(MultiPlayerButton_MouseEnter);
            MultiPlayerButton.MouseLeave += new EventHandler(MultiPlayerButton_MouseLeave);
            Sprites.Add(MultiPlayerButton);

            MultiPlayerLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Multiplayer");
            MultiPlayerLabel.Position = new Vector2(MultiPlayerButton.X + (MultiPlayerButton.Width / 2 - MultiPlayerLabel.Width / 2), MultiPlayerButton.Y + (MultiPlayerButton.Height / 2 - MultiPlayerLabel.Height / 2));
            MultiPlayerLabel.IsHoverable = true;
            MultiPlayerLabel.IsManuallySelectable = true;
            MultiPlayerLabel.NonHoverColor = Color.White;
            MultiPlayerLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(MultiPlayerLabel);


            BackButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            BackButton.Moved += new EventHandler(delegate(object src, EventArgs v) 
                { 
                    BackLabel.Position = new Vector2((BackButton.X + BackButton.Width / 2) - BackLabel.Width / 2, (BackButton.Y + BackButton.Height / 2) - BackLabel.Height / 2); 
                });
            BackButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave += new EventHandler(BackButton_MouseLeave);
            Sprites.Add(BackButton);

            BackLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Exit");
            BackLabel.Position = new Vector2((BackButton.X + BackButton.Width / 2) - BackLabel.Width / 2, (BackButton.Y + BackButton.Height / 2) - BackLabel.Height / 2);
            BackLabel.IsHoverable = true;
            BackLabel.IsManuallySelectable = true;
            BackLabel.NonHoverColor = Color.White;
            BackLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(BackLabel);


            OptionsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .21f), Sprites.SpriteBatch);
            OptionsButton.Moved += new EventHandler(delegate(object src, EventArgs ea) 
                {
                    OptionsLabel.Position = new Vector2(OptionsButton.X + (OptionsButton.Width / 2 - OptionsLabel.Width / 2), OptionsButton.Y + (OptionsButton.Height / 2 - OptionsLabel.Height / 2));
                });
            OptionsButton.MouseEnter += new EventHandler(OptionsButton_MouseEnter);
            OptionsButton.MouseLeave += new EventHandler(OptionsButton_MouseLeave);
            Sprites.Add(OptionsButton);

            OptionsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Options");
            OptionsLabel.Position = new Vector2(OptionsButton.X + (OptionsButton.Width / 2 - OptionsLabel.Width / 2), OptionsButton.Y + (OptionsButton.Height / 2 - OptionsLabel.Height / 2));
            OptionsLabel.IsHoverable = true;
            OptionsLabel.IsManuallySelectable = true;
            OptionsLabel.NonHoverColor = Color.White;
            OptionsLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(OptionsLabel);


            CreditsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .362f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .405f), Sprites.SpriteBatch);
            CreditsButton.Moved += new EventHandler(delegate(object src, EventArgs ea)
            {
                CreditsLabel.Position = new Vector2((CreditsButton.X + CreditsButton.Width / 2) - CreditsLabel.Width / 2, (CreditsButton.Y + CreditsButton.Height / 2) - CreditsLabel.Height / 2);
            });
            CreditsButton.MouseEnter += new EventHandler(CreditsButton_MouseEnter);
            CreditsButton.MouseLeave += new EventHandler(CreditsButton_MouseLeave);
            Sprites.Add(CreditsButton);


            CreditsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Credits");
            CreditsLabel.Position = new Vector2(CreditsButton.X + (CreditsButton.Width / 2 - CreditsLabel.Width / 2), CreditsButton.Y + (CreditsButton.Height / 2 - CreditsLabel.Height / 2));
            CreditsLabel.IsHoverable = true;
            CreditsLabel.IsManuallySelectable = true;
            CreditsLabel.NonHoverColor = Color.White;
            CreditsLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(CreditsLabel);
        }

        

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

            //to unselect options label when changing to full screens and back
            foreach (ISprite s in AdditionalSprites)
            {
                if (s.GetType() == typeof(TextSprite))
                {
                    //We can deselect
                    s.Cast<TextSprite>().IsSelected = false;
                }
            }
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

        void OptionsButton_MouseLeave(object sender, EventArgs e)
        {
            OptionsLabel.IsSelected = false;
        }
        void OptionsButton_MouseEnter(object sender, EventArgs e)
        {
            OptionsLabel.IsSelected = true;
        }

        void MultiPlayerButton_MouseLeave(object sender, EventArgs e)
        {
            MultiPlayerLabel.IsSelected = false;
        }
        void MultiPlayerButton_MouseEnter(object sender, EventArgs e)
        {
            MultiPlayerLabel.IsSelected = true;
        }

        void BackButton_MouseLeave(object sender, EventArgs e)
        {
            BackLabel.IsSelected = false;
        }
        void BackButton_MouseEnter(object sender, EventArgs e)
        {
            BackLabel.IsSelected = true;
        }

        void SinglePlayerButton_MouseLeave(object sender, EventArgs e)
        {
            SinglePlayerLabel.IsSelected = false;
        }
        void SinglePlayerButton_MouseEnter(object sender, EventArgs e)
        {
            SinglePlayerLabel.IsSelected = true;
        }

        MouseState lastMouseState;

        public override void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            if (BackLabel.IsSelected && BackButton.ClickCheck(currentMouseState) && !BackButton.ClickCheck(lastMouseState))
            {
                //StateManager.GoBack();
                exit();
            }
            else if (CreditsLabel.IsSelected && CreditsButton.ClickCheck(currentMouseState) && !CreditsButton.ClickCheck(lastMouseState))
            {
                StateManager.ScreenState = ScreenState.Credits;
            }
            else if (OptionsLabel.IsSelected && OptionsButton.ClickCheck(currentMouseState) && !OptionsButton.ClickCheck(lastMouseState))
            {
                StateManager.ScreenState = ScreenState.Option;
            }
            else if (SinglePlayerLabel.IsSelected && SinglePlayerButton.ClickCheck(currentMouseState) && !SinglePlayerButton.ClickCheck(lastMouseState))
            {
                StateManager.ScreenState = ScreenState.ShipSelect;
            }

            lastMouseState = currentMouseState;

            base.Update(gameTime);
        }

    }
}
