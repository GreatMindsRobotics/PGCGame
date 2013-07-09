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
using Glib.XNA.InputLib;
using Microsoft.Xna.Framework.Media;

namespace PGCGame.Screens
{
    public class MainMenu : BaseScreen
    {

        private Delegates.QuitFunction _exit;

        public MainMenu(SpriteBatch spriteBatch, Delegates.QuitFunction exit)
            : base(spriteBatch, Color.Black)
        {
            _exit = exit;
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

        Song _gameSong;




        public override void InitScreen(ScreenType screnType)
        {
            base.InitScreen(screnType);

            _gameSong = GameContent.GameAssets.Music[ScreenMusic.Level1];
            StateManager.Options.MusicStateChanged += new EventHandler(Options_MusicStateChanged);
            StateManager.ScreenStateChanged += new EventHandler(delegate(object src, EventArgs arg)
            {
                if (Visible)
                {
                    if (StateManager.Options.MusicEnabled)
                    {
                        if (MediaPlayer.State == MediaState.Paused)
                        {
                            MediaPlayer.Resume();
                        }
                        else
                        {
                            MediaPlayer.Play(_gameSong);
                        }
                    }
                    else
                    {
                        MediaPlayer.Stop();
                    }
                }
            });
            Texture2D planetTexture = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
            Texture2D altPlanetTexture = GameContent.GameAssets.Images.NonPlayingObjects.AltPlanet;
            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;

            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            TitleSprite = new Sprite(GameContent.GameAssets.Images.Controls.Title, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .05f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .07f), Sprites.SpriteBatch);
            Sprites.Add(TitleSprite);

            planet = new Sprite(altPlanetTexture, Vector2.Zero, Sprites.SpriteBatch);
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

#if WINDOWS
            SinglePlayerButton.MouseEnter += new EventHandler(SinglePlayerButton_MouseEnter);
            SinglePlayerButton.MouseLeave += new EventHandler(SinglePlayerButton_MouseLeave);
#endif

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

#if WINDOWS
            MultiPlayerButton.MouseEnter += new EventHandler(MultiPlayerButton_MouseEnter);
            MultiPlayerButton.MouseLeave += new EventHandler(MultiPlayerButton_MouseLeave);
#endif
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

#if WINDOWS
            BackButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave += new EventHandler(BackButton_MouseLeave);
#endif

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

#if WINDOWS
            OptionsButton.MouseEnter += new EventHandler(OptionsButton_MouseEnter);
            OptionsButton.MouseLeave += new EventHandler(OptionsButton_MouseLeave);
#endif

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

#if WINDOWS
            CreditsButton.MouseEnter += new EventHandler(CreditsButton_MouseEnter);
            CreditsButton.MouseLeave += new EventHandler(CreditsButton_MouseLeave);
#endif

            Sprites.Add(CreditsButton);


            CreditsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Credits");
            CreditsLabel.Position = new Vector2(CreditsButton.X + (CreditsButton.Width / 2 - CreditsLabel.Width / 2), CreditsButton.Y + (CreditsButton.Height / 2 - CreditsLabel.Height / 2));
            CreditsLabel.IsHoverable = true;
            CreditsLabel.IsManuallySelectable = true;
            CreditsLabel.NonHoverColor = Color.White;
            CreditsLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(CreditsLabel);
#if XBOX
            _screenButtons.Add(Vector2.Zero, SinglePlayerLabel);
            _screenButtons.Add(new Vector2(0, 1), MultiPlayerLabel);
            _screenButtons.Add(new Vector2(0, 2), BackLabel);
            _screenButtons.Add(new Vector2(1, 0), OptionsLabel);
            _screenButtons.Add(Vector2.One, CreditsLabel);

            SinglePlayerLabel.IsSelected = true;
#endif
        }

        void Options_MusicStateChanged(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
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

#if XBOX
        TimeSpan elapsedControllerDelayTime = TimeSpan.Zero;
        TimeSpan totalControllerDelayTime = TimeSpan.FromMilliseconds(250);
#endif

        public override void Update(GameTime gameTime)
        {
#if WINDOWS
            MouseState currentMouseState = MouseManager.CurrentMouseState;

            if (BackLabel.IsSelected && BackButton.ClickCheck(currentMouseState) && !BackButton.ClickCheck(lastMouseState))
            {
                //StateManager.GoBack();
                _exit();
            }
            else if (CreditsLabel.IsSelected && CreditsButton.ClickCheck(currentMouseState) && !CreditsButton.ClickCheck(lastMouseState))
            {
                StateManager.ScreenState = ScreenType.Credits;
            }
            else if (OptionsLabel.IsSelected && OptionsButton.ClickCheck(currentMouseState) && !OptionsButton.ClickCheck(lastMouseState))
            {
                StateManager.ScreenState = ScreenType.Options;
            }
            else if (SinglePlayerLabel.IsSelected && SinglePlayerButton.ClickCheck(currentMouseState) && !SinglePlayerButton.ClickCheck(lastMouseState))
            {
                StateManager.ScreenState = ScreenType.ShipSelect;
            }

            lastMouseState = currentMouseState;
#elif XBOX 
            Vector2 lJoystick = GamePadManager.One.Current.ThumbSticks.Left;

            elapsedControllerDelayTime += gameTime.ElapsedGameTime;

            //If enough time passed... 
            if (elapsedControllerDelayTime > totalControllerDelayTime)
            {
                elapsedControllerDelayTime = TimeSpan.Zero;

                //Check Y position
                if (lJoystick.Y <= -0.6f)
                {
                    _currentlySelectedButton.Y--;
                    if (_currentlySelectedButton.Y < 0)
                    {
                        _currentlySelectedButton.Y = _screenButtons.Count - 1;
                    }
                }
                else if (lJoystick.Y >= 0.6f)
                {
                    _currentlySelectedButton.Y++;
                    if (_currentlySelectedButton.Y == _screenButtons.Count)
                    {
                        _currentlySelectedButton.Y = 0;
                    }
                }

                //Check X position
                if (lJoystick.X <= -0.6f)
                {
                    _currentlySelectedButton.X--;
                    if (_currentlySelectedButton.X < 0)
                    {
                        _currentlySelectedButton.X = _screenButtons.Count - 1;
                    }
                }
                else if (lJoystick.X >= 0.6f)
                {
                    _currentlySelectedButton.X++;
                    if (_currentlySelectedButton.X == _screenButtons.Count)
                    {
                        _currentlySelectedButton.X = 0;
                    }
                }
            }


            //De-select all buttons
            foreach (var button in _screenButtons)
            {
                button.Value.IsSelected = false;
            }

            //Select the right button
            _screenButtons[_currentlySelectedButton].IsSelected = true;


            if (GamePadManager.One.Current.IsButtonDown(Buttons.A))
            {
                if (BackLabel.IsSelected)
                {
                    _exit();
                }
                else if (SinglePlayerLabel.IsSelected)
                {
                    StateManager.ScreenState = ScreenType.ShipSelect;
                }
                else if (MultiPlayerLabel.IsSelected)
                { 
                    //TODO: Multiplayer
                }
                else if (OptionsLabel.IsSelected)
                {
                    StateManager.ScreenState = ScreenType.Options;
                }
                else if (CreditsLabel.IsSelected)
                {
                    StateManager.ScreenState = ScreenType.Credits;
                }
            }
#endif
            base.Update(gameTime);
        }

    }
}
