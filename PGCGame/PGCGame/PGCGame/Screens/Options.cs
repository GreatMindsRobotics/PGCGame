using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PGCGame.CoreTypes;

namespace PGCGame.Screens
{
    public class Options : Screen
    {


        public Options(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Pink)
        {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        }

        Sprite MoveControlButton;
        TextSprite MoveControlLabel;

        TextSprite GFXLabel;
        TextSprite SFXLabel;
        TextSprite MusicVolumeLabel;
        TextSprite BackLabel;
        TextSprite FireControlLabel;

        bool mouseInBackButton = false;
        

        public void LoadContent(ContentManager content)
        {
            //Add background to this screen     
            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            Texture2D button = content.Load<Texture2D>("Images\\Controls\\Button");
            SpriteFont font = content.Load<SpriteFont>("Fonts\\SegoeUIMono");

            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

            //Move Controls (aka Controls)
            MoveControlButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f), Sprites.SpriteBatch);
            MoveControlButton.MouseEnter += new EventHandler(MoveControlButton_MouseEnter);
            MoveControlButton.MouseLeave += new EventHandler(MoveControlButton_MouseLeave); 

            MoveControlLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, String.Format("Move: {0}", StateManager.Options.ArrowKeysEnabled ? "Arrow" : "WASD"));
            MoveControlLabel.Position = new Vector2(MoveControlButton.Position.X + (MoveControlButton.Width / 2 - MoveControlLabel.Width / 2), MoveControlButton.Position.Y + (MoveControlButton.Height / 2 - MoveControlLabel.Height / 2));
            MoveControlLabel.Color = Color.White;
            MoveControlLabel.IsManuallySelectable = true;
            MoveControlLabel.IsHoverable = true;
            MoveControlLabel.HoverColor = Color.MediumAquamarine;
            MoveControlLabel.NonHoverColor = Color.White;

                                                                                                                                                                              

          
            //Fire Controls (aka Temporary)
            FireControlLabel = new TextSprite(Sprites.SpriteBatch, font, String.Format("Fire:{0}", StateManager.Options.LeftButtonEnabled ? "LClick" : "Space"));
            Sprite FireControlButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .50f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .10f), Sprites.SpriteBatch);
            FireControlLabel.Position = new Vector2((FireControlButton.X + FireControlButton.Width / 2) - FireControlLabel.Width / 2, (FireControlButton.Y + FireControlButton.Height / 2) - FireControlLabel.Height / 2);
            FireControlLabel.Color = Color.White;
            FireControlLabel.IsManuallySelectable = true;
            FireControlLabel.IsHoverable = true;
            FireControlLabel.HoverColor = Color.MediumAquamarine;
            FireControlLabel.NonHoverColor = Color.White;
            FireControlButton.MouseEnter += new EventHandler(FireControlButton_MouseEnter);
            FireControlButton.MouseLeave += new EventHandler(FireControlButton_MouseLeave);                                                                                                                   


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


            //SFX
            Sprite SFXButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);
            SFXButton.MouseEnter += new EventHandler(SFXButton_MouseEnter);
            SFXButton.MouseLeave += new EventHandler(SFXButton_MouseLeave);

            SFXLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "SFX: " + (StateManager.Options.SFXEnabled ? "On" : "Off"));
            SFXLabel.Position = new Vector2((SFXButton.X + SFXButton.Width / 2) - SFXLabel.Width / 2, (SFXButton.Y + SFXButton.Height / 2) - SFXLabel.Height / 2);
            SFXLabel.Color = Color.White;
            SFXLabel.IsHoverable = true;
            SFXLabel.IsManuallySelectable = true;
            SFXLabel.HoverColor = Color.MediumAquamarine;
            SFXLabel.NonHoverColor = Color.White;


            //Back button
            Sprite BackButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            BackLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .139f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .62f), content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Back");
            BackLabel.Color = Color.White;
            BackButton.MouseEnter += new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave += new EventHandler(BackButton_MouseLeave);


            //Music (volume; currently on/off)

            Sprite MusicButton = new Sprite(button, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            MusicButton.Color = Color.White;
            MusicButton.MouseEnter += new EventHandler(MusicButton_MouseEnter);
            MusicButton.MouseLeave += new EventHandler(MusicButton_MouseLeave);

            MusicVolumeLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, font, "Music: " + (StateManager.Options.MusicEnabled ? "On" : "Off"));
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
       
            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            //RESET THE LOCATION OF EVERY SPRITE ON THE SCREEN!
            GFXLabel.Text = String.Format("GFX: {0}", StateManager.GraphicsManager.IsFullScreen ? "Full" : "Standard");

        }
        //Fire Controls
        void FireControlButton_MouseLeave(object sender, EventArgs e)
        {
            FireControlLabel.IsSelected = false;
        }
        void FireControlButton_MouseEnter(object sender, EventArgs e)
        {
            FireControlLabel.IsSelected = true;
        }

        //Controls
        void MoveControlButton_MouseLeave(object sender, EventArgs e)
        {
            MoveControlLabel.IsSelected = false;
        }
        void MoveControlButton_MouseEnter(object sender, EventArgs e)
        {
            MoveControlLabel.IsSelected = true;
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
        //Full

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
                    StateManager.GoBack();
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
                    StateManager.Options.ToggleFullscreen();

                    
                }
                if (MoveControlLabel.IsSelected)
                {
                    StateManager.Options.ArrowKeysEnabled = !StateManager.Options.ArrowKeysEnabled;
                    MoveControlLabel.Text = String.Format("Move: {0}", StateManager.Options.ArrowKeysEnabled ? "Arrow" : "WASD");
                }
                if (FireControlLabel.IsSelected)
                {
                    StateManager.Options.LeftButtonEnabled = !StateManager.Options.LeftButtonEnabled;
                    FireControlLabel.Text = String.Format("Fire:{0}", StateManager.Options.LeftButtonEnabled ? "LClick" : "Space");
                    
                                    }
            }
            lastMs = currentMs;
        } 
    }
}
