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
using Microsoft.Xna.Framework.Media;

namespace PGCGame.Screens
{
    public class Title : BaseScreen
    {
        public Title(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {

            ButtonClick = GameContent.GameAssets.Sound[SoundEffectType.ButtonPressed];
        }

        Sprite TitleImage;

        Sprite planet;
        Sprite planettwo;

        Sprite PlayButton;
        TextSprite PlayLabel;

        Sprite ExitButton;
        TextSprite ExitLabel;

        Sprite ship;

        Song _gameSong;

#if XBOX
        GamePadButtonEnumerator ButtonManagement;
#endif


        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);

            _gameSong = GameContent.GameAssets.Music[ScreenMusic.Level1];

            StateManager.Options.MusicStateChanged += new EventHandler(Options_MusicStateChanged);
            StateManager.ScreenStateChanged += new EventHandler(delegate(object src, EventArgs arg)
            {
                if (Visible)
                {
                    if (StateManager.Options.MusicEnabled)
                    {
                        if (MediaPlayer.State != MediaState.Playing)
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

            Viewport viewport = Sprites.SpriteBatch.GraphicsDevice.Viewport;

            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            Texture2D planetTexture = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
            Texture2D buttonTexture = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;
            Texture2D altPlanetTexture = GameContent.GameAssets.Images.NonPlayingObjects.AltPlanet;
            Texture2D fighterCarrier = GameContent.GameAssets.Images.Ships[ShipType.FighterCarrier, ShipTier.Tier1];


            planet = new Sprite(planetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planet.Scale = new Vector2(.7f);
            planet.Position = new Vector2(viewport.Width * 0.1f, viewport.Height * .16f);
            Sprites.Add(planet);

            planettwo = new Sprite(altPlanetTexture, Vector2.Zero, Sprites.SpriteBatch);
            planettwo.Scale = new Vector2(1f);
            planettwo.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.8f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .75f);
            Sprites.Add(planettwo);

            setupTitleShip();

            Sprites.Add(ship);

            //title image
            TitleImage = new Sprite(GameContent.GameAssets.Images.Controls.Title, Vector2.Zero, Sprites.SpriteBatch);
            TitleImage.Position = new Vector2(viewport.Width / 2 - TitleImage.Texture.Width / 2, viewport.Height * 0.2f);
            Sprites.Add(TitleImage);

            PlayButton = new Sprite(buttonTexture, new Vector2(viewport.Width * 0.375f, viewport.Height * 0.4f), Sprites.SpriteBatch);
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
            ExitLabel.ParentSprite = ExitButton;
            PlayLabel.ParentSprite = PlayButton;

#if WINDOWS
            
            ExitLabel.Pressed += new EventHandler(ExitLabel_Clicked);
            PlayLabel.Pressed += new EventHandler(PlayLabel_Clicked);
            
#elif XBOX
            ButtonManagement = new GamePadButtonEnumerator(new TextSprite[,] { { PlayLabel }, { ExitLabel } }, InputType.LeftJoystick);
            ButtonManagement.ButtonPress += new EventHandler(ButtonManagement_ButtonPress);
#endif
        }

#if WINDOWS
        void ExitLabel_Clicked(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }

            StateManager.Exit();
        }

        void PlayLabel_Clicked(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }

            //if(StateManager.Options.SFXEnabled)
            //{
            //   ButtonClick.Play();
            //}

            StateManager.ScreenState = CoreTypes.ScreenType.MainMenu;
        }

#elif XBOX
        void ButtonManagement_ButtonPress(object sender, EventArgs e)
        {
            if (ExitLabel.IsSelected)
            {
                if (!this.Visible)
                {
                    return;
                }


                _exit();
            }
            else if (PlayLabel.IsSelected)
            {
                if (!this.Visible)
                {
                    return;
                }

                StateManager.ScreenState = ScreenType.MainMenu;
            }
        }

        TimeSpan elapsedControllerDelayTime = TimeSpan.Zero;
        TimeSpan totalControllerDelayTime = TimeSpan.FromMilliseconds(250);
#endif
        private void setupTitleShip()
        {
            ShipType type = (ShipType)StateManager.RandomGenerator.Next(1, 4);
            ShipTier tier = StateManager.RandomGenerator.NextShipTier(ShipTier.Tier1, ShipTier.Tier4);

            if (ship == null)
            {
                ship = new Sprite(GameContent.GameAssets.Images.Ships[type, tier], Vector2.Zero, Sprites.SpriteBatch);
            }
            else
            { 
                ship.Texture = GameContent.GameAssets.Images.Ships[type, tier];
            }

            ship.Position = new Vector2(-ship.Texture.Width / 2, Graphics.Viewport.Height);
            ship.Scale = new Vector2(0.8f);
            ship.XSpeed = 1.5f;
            ship.YSpeed = -ship.XSpeed * .8f;
            ship.Rotation.Degrees = 0;
        }


        void Options_MusicStateChanged(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
        }


        public override void Update(GameTime gameTime)
        {
            if (ship.Position.X < Graphics.Viewport.Width * 3)
            {
                if (ship.Rotation.Degrees <= 90)
                {
                    ship.Rotation.Radians = (new Vector2(Graphics.Viewport.Width / 2, Graphics.Viewport.Height / 2) - ship.Position).ToAngle();
                    ship.YSpeed -= .0008f;
                    ship.Scale.X -= .001f;
                    ship.Scale.Y -= .001f;
                }
                else
                {
                    ship.XSpeed += .1f;
                    ship.YSpeed = 0;
                    if (ship.Scale.X >= .005f)
                    {
                        ship.Scale.X -= .003f;
                        ship.Scale.Y -= .003f;
                    }
                }
            }
            else
            {
                setupTitleShip(); 
            }


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
