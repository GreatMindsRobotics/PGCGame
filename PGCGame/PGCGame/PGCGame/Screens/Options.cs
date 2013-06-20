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
        TextSprite Temporary;
        TextSprite Control;
        TextSprite Graphics;
        TextSprite SFXLabel;
        TextSprite MusicLabel;
        TextSprite Volume;
        TextSprite OptionsLabel;
        TextSprite BackLabel;
        bool mouseInBackButton = false;
        

        public Options(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Pink)
        {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        }
        public void LoadContent(ContentManager content)
        {
                                                  
            this.BackgroundSprite = new HorizontalMenuBGSprite(content.Load<Texture2D>("Images\\Background\\1920by1080SkyStar"), Sprites.SpriteBatch);


            Sprite BackButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            BackLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .139f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .62f), content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Back");
            BackLabel.Color = Color.White;
            BackButton.MouseEnter +=new EventHandler(BackButton_MouseEnter);
            BackButton.MouseLeave +=new EventHandler(BackButton_MouseLeave);



            TextSprite TemporaryLabel = new TextSprite(Sprites.SpriteBatch, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Temporary:");
            Sprite TemporaryButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .55f), Sprites.SpriteBatch);



            TextSprite SFXLabel = new TextSprite(Sprites.SpriteBatch, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "SFX:");
            Sprite SFXButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);


            TextSprite GraphicsLabel = new TextSprite(Sprites.SpriteBatch, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Graphics:");
            Sprite GraphicsButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .35f), Sprites.SpriteBatch);

            TextSprite ControlLabel;
            Sprite ControlButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .1f), Sprites.SpriteBatch);   



            Sprite MusicButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            MusicButton.Color = Color.White;
            MusicButton.MouseEnter += new EventHandler(MusicButton_MouseEnter);
            MusicButton.MouseLeave += new EventHandler(MusicButton_MouseLeave);

            MusicLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Music: "+(StateManager.Options.Volume == 100 ? "On" : "Off"));
            MusicLabel.Position = new Vector2((MusicButton.X + MusicButton.Width/2) - MusicLabel.Width/2, (MusicButton.Y + MusicButton.Height/2) - MusicLabel.Height/2);
            MusicLabel.Color = Color.White;
            MusicLabel.IsHoverable = true;
            MusicLabel.IsManuallySelectable = true;
            MusicLabel.HoverColor = Color.MediumAquamarine;
            MusicLabel.NonHoverColor = Color.White;


            SFXLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "SFX:");
            SFXLabel.Position = new Vector2((SFXButton.X + SFXButton.Width / 2) - SFXLabel.Width / 2, (SFXButton.Y + SFXButton.Height / 2) - SFXLabel.Height / 2);
            SFXLabel.Color = Color.White;

            ControlLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Controls");
            ControlLabel.Position = new Vector2((ControlButton.X + ControlButton.Width / 2)- ControlLabel.Width / 2, (ControlButton.Y + ControlButton.Height / 2) - ControlLabel.Height / 2);
            ControlLabel.Color = Color.White;

            TemporaryLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Temporary", Color.White);
            TemporaryLabel.Position = new Vector2((GraphicsButton.X + GraphicsButton.Width / 2) - GraphicsLabel.Width / 2, (GraphicsButton.Y + GraphicsButton.Height / 2) - GraphicsLabel.Height / 2);


            GraphicsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Graphics:");
            GraphicsLabel.Position = new Vector2((GraphicsButton.X + GraphicsButton.Width / 2) - GraphicsLabel.Width / 2, (GraphicsButton.Y + GraphicsButton.Height / 2) - GraphicsLabel.Height / 2);
            GraphicsLabel.Color = Color.White;


            Sprites.Add(TemporaryButton);
            Sprites.Add(GraphicsButton);
            Sprites.Add(ControlButton);
            Sprites.Add(MusicButton);
            Sprites.Add(SFXButton);


            Sprites.Add(BackButton);

            
            //AdditionalSprites.Add(
            AdditionalSprites.Add(ControlLabel);
            AdditionalSprites.Add(GraphicsLabel);
            AdditionalSprites.Add(MusicLabel);
            AdditionalSprites.Add(BackLabel);
            AdditionalSprites.Add(SFXLabel);

       
        }

        void MusicButton_MouseLeave(object sender, EventArgs e)
        {
            MusicLabel.IsSelected = false;
        }

        void MusicButton_MouseEnter(object sender, EventArgs e)
        {
            MusicLabel.IsSelected = true;
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
                if (MusicLabel.IsSelected)
                {
                    StateManager.Options.Volume = StateManager.Options.Volume == 100 ? 0 : 100;
                    MusicLabel.Text = "Music: " + (StateManager.Options.Volume == 100 ? "On" : "Off");
                }
            }
            lastMs = currentMs;
            
        }
        
        
    }
}
