using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PGCGame.Screens
{
    public class Options : Screen
    {
        TextSprite MoveControlLabel;
        TextSprite GFXLabel;
        TextSprite SFXLabel;
        TextSprite MusicVolumeLabel;
        TextSprite OptionsLabel;
        TextSprite BackLabel;
        bool mouseInBackButton = false;
        

        public Options(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Pink)
        {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        }
        public void LoadContent(ContentManager content)
        {
            //Add background to this screen     
            this.BackgroundSprite = new HorizontalMenuBGSprite(content.Load<Texture2D>("Images\\Background\\1920by1080SkyStar"), Sprites.SpriteBatch);

            //Move Controls (aka Controls)
            TextSprite MoveControlLabel;
            Sprite MoveControlButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f), Sprites.SpriteBatch);
            MoveControlLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Move");
            MoveControlLabel.Position = new Vector2(MoveControlButton.Position.X + (MoveControlButton.Width / 2 - MoveControlLabel.Width / 2), MoveControlButton.Position.Y + (MoveControlButton.Height / 2 - MoveControlLabel.Height / 2));
            MoveControlLabel.Color = Color.White;


          
            //Fire Controls (aka Temporary)

            TextSprite FireControlLabel = new TextSprite(Sprites.SpriteBatch, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "FireControlLabel:");
            Sprite FireControlButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .50f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .10f), Sprites.SpriteBatch);
            FireControlLabel.Position = new Vector2((FireControlButton.X + FireControlButton.Width / 2) - FireControlLabel.Width / 2, (FireControlButton.Y + FireControlButton.Height / 2) - FireControlLabel.Height / 2);
            


            //GFX
            Sprite GraphicsButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);
            GraphicsButton.MouseEnter +=new EventHandler(GraphicsButton_MouseEnter);
            GraphicsButton.MouseLeave +=new EventHandler(GraphicsButton_MouseLeave);

            GFXLabel = new TextSprite(Sprites.SpriteBatch, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), String.Format("GFX: {0}", StateManager.Options.HDEnabled ? "High Def" : "Standard"));
            GFXLabel.Position = new Vector2((GraphicsButton.X + GraphicsButton.Width / 2) - GFXLabel.Width / 2, (GraphicsButton.Y + GraphicsButton.Height / 2) - GFXLabel.Height / 2);
            GFXLabel.Color = Color.White;
            GFXLabel.IsManuallySelectable = true;
            GFXLabel.IsHoverable = true;
            GFXLabel.HoverColor = Color.MediumAquamarine;
            GFXLabel.NonHoverColor = Color.White;


            //SFX
            Sprite SFXButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);
            SFXButton.MouseEnter += new EventHandler(SFXButton_MouseEnter);
            SFXButton.MouseLeave += new EventHandler(SFXButton_MouseLeave);

            SFXLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "SFX: " + (StateManager.Options.SFXEnabled ? "On" : "Off"));
            SFXLabel.Position = new Vector2((SFXButton.X + SFXButton.Width / 2) - SFXLabel.Width / 2, (SFXButton.Y + SFXButton.Height / 2) - SFXLabel.Height / 2);
            SFXLabel.Color = Color.White;
            SFXLabel.IsHoverable = true;
            SFXLabel.IsManuallySelectable = true;
            SFXLabel.HoverColor = Color.MediumAquamarine;
            SFXLabel.NonHoverColor = Color.White;


            //Back button
            Sprite BackButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            BackLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .139f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .62f), content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Back");
            BackLabel.Color = Color.White;
            BackButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave += new EventHandler(BackButton_MouseLeave);


            //Music (volume; currently on/off)

            Sprite MusicButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            MusicButton.Color = Color.White;
            MusicButton.MouseEnter += new EventHandler(MusicButton_MouseEnter);
            MusicButton.MouseLeave += new EventHandler(MusicButton_MouseLeave);

            MusicVolumeLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Music: " + (StateManager.Options.MusicEnabled ? "On" : "Off"));
            MusicVolumeLabel.Position = new Vector2((MusicButton.X + MusicButton.Width / 2) - MusicVolumeLabel.Width / 2, (MusicButton.Y + MusicButton.Height / 2) - MusicVolumeLabel.Height / 2);
            MusicVolumeLabel.Color = Color.White;
            MusicVolumeLabel.IsHoverable = true;
            MusicVolumeLabel.IsManuallySelectable = true;
            MusicVolumeLabel.HoverColor = Color.MediumAquamarine;
            MusicVolumeLabel.NonHoverColor = Color.White;


            //Add all buttons
            Sprites.Add(MoveControlButton);
            Sprites.Add(FireControlButton);
            Sprites.Add(GraphicsButton);
            Sprites.Add(SFXButton);
            Sprites.Add(BackButton);
            Sprites.Add(MusicButton);

            //Add all text sprites
            AdditionalSprites.Add(MoveControlLabel);
            AdditionalSprites.Add(FireControlLabel);
            AdditionalSprites.Add(GFXLabel);
            AdditionalSprites.Add(SFXLabel);
            AdditionalSprites.Add(BackLabel);
            AdditionalSprites.Add(MusicVolumeLabel);
       
        }

        void SFXButton_MouseLeave(object sender, EventArgs e)
        {
            SFXLabel.IsSelected = false;
        }

        void SFXButton_MouseEnter(object sender, EventArgs e)
        {
            SFXLabel.IsSelected = true;
        }

        //standard
        //full hd

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

        

        void MusicButton_MouseLeave(object sender, EventArgs e)
        {
            MusicVolumeLabel.IsSelected = false;
        }

        void MusicButton_MouseEnter(object sender, EventArgs e)
        {
            MusicVolumeLabel.IsSelected = true;
        }


        public bool mouseInOptionButton
        {
            get
            {
                return OptionsLabel.IsSelected;

            }
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
            base.Update(gameTime);
            MouseState currentMs = Mouse.GetState();
            if (lastMs.LeftButton == ButtonState.Released && currentMs.LeftButton == ButtonState.Pressed)
            {
                if (mouseInBackButton)
                {
                    StateManager.ScreenState = ScreenState.MainMenu;
                }
                if (MusicVolumeLabel.IsSelected)
                {
                    StateManager.Options.MusicEnabled = !StateManager.Options.MusicEnabled;
                    MusicVolumeLabel.Text = String.Format("Music: {0}", StateManager.Options.MusicEnabled ? "On" : "Off");
                }
                if (SFXLabel.IsSelected)
                {
                    StateManager.Options.SFXEnabled = !StateManager.Options.SFXEnabled;
                    SFXLabel.Text = String.Format("SFX: {0}", StateManager.Options.SFXEnabled ? "On" : "Off");
                }
                if (mouseOnGraphicButton)
                {
                    StateManager.Options.HDEnabled = !StateManager.Options.HDEnabled;
                    GFXLabel.Text = String.Format("GFX: {0}", StateManager.Options.HDEnabled ? "High Def" : "Standard");
                    //System.Diagnostics.Debug.WriteLine("Pre-fullscreen: {0} wide by {1} high, mode {2}", StateManager.GraphicsManager.PreferredBackBufferWidth, StateManager.GraphicsManager.PreferredBackBufferHeight, StateManager.GraphicsManager.GraphicsDevice.DisplayMode);
                    //StateManager.GraphicsManager.ToggleFullScreen();

                    PresentationParameters screenParams = new PresentationParameters();

                    screenParams.IsFullScreen = StateManager.Options.HDEnabled;
                    screenParams.BackBufferFormat = StateManager.GraphicsManager.GraphicsDevice.DisplayMode.Format;
                    screenParams.BackBufferWidth = StateManager.Options.HDEnabled ? StateManager.GraphicsManager.GraphicsDevice.DisplayMode.Width : StateManager.Resolution.X;
                    screenParams.BackBufferHeight = StateManager.Options.HDEnabled ? StateManager.GraphicsManager.GraphicsDevice.DisplayMode.Height : StateManager.Resolution.Y;
                    screenParams.DeviceWindowHandle = StateManager.GraphicsManager.GraphicsDevice.PresentationParameters.DeviceWindowHandle;

                    
                    StateManager.GraphicsManager.GraphicsDevice.Reset(screenParams);
                    StateManager.GraphicsManager.IsFullScreen = StateManager.Options.HDEnabled;
                    
                    StateManager.GraphicsManager.GraphicsDevice.Viewport = new Viewport(0, 0, screenParams.BackBufferWidth, screenParams.BackBufferHeight) { MinDepth = 0.0f, MaxDepth = 1.0f };
                    StateManager.GraphicsManager.PreferredBackBufferWidth = screenParams.BackBufferWidth;
                    StateManager.GraphicsManager.PreferredBackBufferHeight = screenParams.BackBufferHeight;
                    foreach (Screen s in StateManager.AllScreens)
                    {
                        //if (!screenParams.IsFullScreen)
                        //{
                        //    System.Diagnostics.Debugger.Break();
                        //}
                        s.Target = new RenderTarget2D(s.Graphics, screenParams.BackBufferWidth, screenParams.BackBufferHeight);
                    }
                    StateManager.Options.callResChangeEvent();
                    //System.Diagnostics.Debug.WriteLine("Fullscreen: {0} wide by {1} high, mode {2}", StateManager.GraphicsManager.PreferredBackBufferWidth, StateManager.GraphicsManager.PreferredBackBufferHeight, StateManager.GraphicsManager.GraphicsDevice.DisplayMode);
                    
                }
            }
            lastMs = currentMs;
            
        }
        
        
    }
}
