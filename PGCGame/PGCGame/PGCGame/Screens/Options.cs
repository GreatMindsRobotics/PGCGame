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
    public class Options : BaseScreen
    {


        public Options(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        }

        Sprite ControlButton;
        TextSprite ControlLabel;

#if WINDOWS
        TextSprite GFXLabel;
#endif
        TextSprite SFXLabel;
        TextSprite MusicVolumeLabel;
        TextSprite BackLabel;
        

        bool mouseInBackButton = false;
        

        public override void InitScreen(ScreenType screenType)
        {
            base.InitScreen(screenType);

            //Add background to this screen     
            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            Texture2D button = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont font = GameContent.GameAssets.Fonts.NormalText;

#if WINDOWS
            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);
#endif

            //Move Controls (aka Controls)
            ControlButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f), Sprites.SpriteBatch);
            
#if WINDOWS
            ControlButton.MouseEnter += new EventHandler(ControlButton_MouseEnter);
            ControlButton.MouseLeave += new EventHandler(ControlButton_MouseLeave); 
#endif
            ControlLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, ("Controls"));
            ControlLabel.Position = new Vector2(ControlButton.Position.X + (ControlButton.Width / 2 - ControlLabel.Width / 2), ControlButton.Position.Y + (ControlButton.Height / 2 - ControlLabel.Height / 2));
            ControlLabel.Color = Color.White;
            ControlLabel.IsManuallySelectable = true;
            ControlLabel.IsHoverable = true;
            ControlLabel.HoverColor = Color.MediumAquamarine;
            ControlLabel.NonHoverColor = Color.White;

                                                                                                                                                                             

            
#if WINDOWS
            //GFX
            Sprite GraphicsButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);

            GraphicsButton.MouseEnter +=new EventHandler(GraphicsButton_MouseEnter);
            GraphicsButton.MouseLeave +=new EventHandler(GraphicsButton_MouseLeave);


            GFXLabel = new TextSprite(Sprites.SpriteBatch, font, String.Format("GFX: {0}", StateManager.GraphicsManager.IsFullScreen ? "Full" : "Standard"));
            GFXLabel.Position = new Vector2((GraphicsButton.X + GraphicsButton.Width / 2) - GFXLabel.Width / 2, (GraphicsButton.Y + GraphicsButton.Height / 2) - GFXLabel.Height / 2);
            GFXLabel.Color = Color.White;
            GFXLabel.IsManuallySelectable = true;
            GFXLabel.IsHoverable = true;
            GFXLabel.HoverColor = Color.MediumAquamarine;
            GFXLabel.NonHoverColor = Color.White;
#endif

            //SFX
            Sprite SFXButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .10f), Sprites.SpriteBatch);
            
#if WINDOWS
            SFXButton.MouseEnter += new EventHandler(SFXButton_MouseEnter);
            SFXButton.MouseLeave += new EventHandler(SFXButton_MouseLeave);
#endif
            SFXLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "SFX: " + (StateManager.Options.SFXEnabled ? "On" : "Off"));
            SFXLabel.Position = new Vector2((SFXButton.X + SFXButton.Width / 2) - SFXLabel.Width / 2, (SFXButton.Y + SFXButton.Height / 2) - SFXLabel.Height / 2);
            SFXLabel.Color = Color.White;
            SFXLabel.IsHoverable = true;
            SFXLabel.IsManuallySelectable = true;
            SFXLabel.HoverColor = Color.MediumAquamarine;
            SFXLabel.NonHoverColor = Color.White;


            //Back button
#if WINDOWS
            Sprite BackButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
#elif XBOX
            Sprite BackButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);
#endif
            BackLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "Back");
            BackLabel.Position = new Vector2((BackButton.X + BackButton.Width/2) - BackLabel.Width/2, (BackButton.Y + BackButton.Height/2) - BackLabel.Height/2);
            BackLabel.Color = Color.White;

#if WINDOWS
            BackButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave += new EventHandler(BackButton_MouseLeave);
#endif

            //Music (volume; currently on/off)

            Sprite MusicButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);
            MusicButton.Color = Color.White;

#if WINDOWS
            MusicButton.MouseEnter += new EventHandler(MusicButton_MouseEnter);
            MusicButton.MouseLeave += new EventHandler(MusicButton_MouseLeave);
#endif

            MusicVolumeLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "Music: " + (StateManager.Options.MusicEnabled ? "On" : "Off"));
            MusicVolumeLabel.Position = new Vector2((MusicButton.X + MusicButton.Width / 2) - MusicVolumeLabel.Width / 2, (MusicButton.Y + MusicButton.Height / 2) - MusicVolumeLabel.Height / 2);
            MusicVolumeLabel.Color = Color.White;
            MusicVolumeLabel.IsHoverable = true;
            MusicVolumeLabel.IsManuallySelectable = true;
            MusicVolumeLabel.HoverColor = Color.MediumAquamarine;
            MusicVolumeLabel.NonHoverColor = Color.White;


            //Add all buttons
            Sprites.Add(ControlButton);
            Sprites.Add(SFXButton);
            Sprites.Add(BackButton);
            Sprites.Add(MusicButton);

            //Add all text sprites
            AdditionalSprites.Add(ControlLabel);
#if WINDOWS
            Sprites.Add(GraphicsButton);
            AdditionalSprites.Add(GFXLabel);
#endif
            AdditionalSprites.Add(SFXLabel);
            AdditionalSprites.Add(BackLabel);
            AdditionalSprites.Add(MusicVolumeLabel);

#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { { ControlLabel,  SFXLabel}, { BackLabel,  MusicVolumeLabel}}, InputType.LeftJoystick);
            AllButtons.ButtonPress += new EventHandler(AllButtons_ButtonPress);
            ControlLabel.IsSelected = true;
#endif

#if WINDOWS
            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);
#endif

        }

#if XBOX
        void AllButtons_ButtonPress(object sender, EventArgs e)
        { 
          if (ControlLabel.IsSelected)
          {
              StateManager.ScreenState = ScreenType.ControlScreen;
          }
          else if (SFXLabel.IsSelected)
          {
              StateManager.Options.SFXEnabled = !StateManager.Options.SFXEnabled;
              SFXLabel.Text = String.Format("SFX: {0}", StateManager.Options.SFXEnabled ? "On" : "Off");
          }
#if WINDOWS
          else if (GFXLabel.IsSelected)
          {
              StateManager.Options.ToggleFullscreen();
          }
#endif
          else if (MusicVolumeLabel.IsSelected)
          {
              StateManager.Options.MusicEnabled = !StateManager.Options.MusicEnabled;
              MusicVolumeLabel.Text = String.Format("Music: {0}", StateManager.Options.MusicEnabled ? "On" : "Off");
          }
          else if (BackLabel.IsSelected)
          {
             
          }
        }
#endif
#if XBOX
        GamePadButtonEnumerator AllButtons;
#endif

#if WINDOWS
        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            //RESET THE LOCATION OF EVERY SPRITE ON THE SCREEN!
            GFXLabel.Text = String.Format("GFX: {0}", StateManager.GraphicsManager.IsFullScreen ? "Full" : "Standard");

        }
#endif

        //Controls
        void ControlButton_MouseLeave(object sender, EventArgs e)
        {
            ControlLabel.IsSelected = false;
        }
        void ControlButton_MouseEnter(object sender, EventArgs e)
        {
            ControlLabel.IsSelected = true;
        }

        //Sound Effects
        void SFXButton_MouseLeave(object sender, EventArgs e)
        {
            SFXLabel.IsSelected = false;
        }

        void SFXButton_MouseEnter(object sender, EventArgs e)
        {
            SFXLabel.IsSelected = true;
        }

        //standard
        //Full
#if WINDOWS
        void GraphicsButton_MouseLeave(object sender, EventArgs e)
        {
            GFXLabel.IsSelected = false;
        }
        void GraphicsButton_MouseEnter(object sender, EventArgs e)
        {
            GFXLabel.IsSelected = true;
        }

        public bool mouseOnGraphicButton
        {
            get
            {
                return GFXLabel.IsSelected;
            }
        }
#endif





        void MusicButton_MouseLeave(object sender, EventArgs e)
        {
            MusicVolumeLabel.IsSelected = false;
        }

        void MusicButton_MouseEnter(object sender, EventArgs e)
        {
            MusicVolumeLabel.IsSelected = true;
        }
     

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

        
                                                    
        MouseState lastMs = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);

        public override void Update(GameTime gameTime)
        {
#if WINDOWS
            MouseState currentMs = MouseManager.CurrentMouseState;
            if (lastMs.LeftButton == ButtonState.Released && currentMs.LeftButton == ButtonState.Pressed)
            {
                if (mouseInBackButton)
                {
                    StateManager.GoBack();
                }
                if (MusicVolumeLabel.IsSelected)
                {
                    StateManager.Options.MusicEnabled = !StateManager.Options.MusicEnabled;
                    MusicVolumeLabel.Text = String.Format("Music: {0}", StateManager.Options.MusicEnabled ? "On" : "Off");
                    if (MusicVolumeLabel.Text == "Music: On")
                    {
                        MediaPlayer.Resume();
                    }
                }
                if (SFXLabel.IsSelected)
                {
                    StateManager.Options.SFXEnabled = !StateManager.Options.SFXEnabled;
                    SFXLabel.Text = String.Format("SFX: {0}", StateManager.Options.SFXEnabled ? "On" : "Off");

                }
                if (mouseOnGraphicButton)
                {
                    StateManager.Options.ToggleFullscreen();
                }
                if (ControlLabel.IsSelected)
                {
                    StateManager.ScreenState = ScreenType.ControlScreen;
                }
            }
            lastMs = currentMs;
#elif XBOX
            AllButtons.Update(gameTime);

            currentGamePad = GamePad.GetState(PlayerIndex.One);

            lastGamePad = currentGamePad;

#endif
            base.Update(gameTime);
        }
#if XBOX
        GamePadState currentGamePad;
        GamePadState lastGamePad = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, Buttons.A);
#endif
    }
}
