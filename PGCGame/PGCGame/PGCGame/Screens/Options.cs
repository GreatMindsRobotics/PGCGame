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
        TextSprite VolumeLabel;
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


            Sprite volumeLabel= new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            VolumeLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .139f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .62f), content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Back");
            VolumeLabel.Color = Color.White;
          


            Sprite VolumeButton = new Sprite(content.Load<Texture2D>("Images\\Controls\\Button"), new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            VolumeButton.Color = Color.White;


            Sprites.Add(VolumeButton);
            
            Sprites.Add(BackButton);

            AdditionalSprites.Add(VolumeLabel);
            AdditionalSprites.Add(BackLabel);

       
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


        //TODO: LOAD CONTENT 
        //use Sprites to load your sprites
        //EX: Sprites.Add(new Sprite(content.Load<Texture2D>("assetName"), new Vector2(0, 0), Sprites.SpriteBatch));
        //OR
        //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
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
                
            }
            lastMs = currentMs;

        }
        
        
    }
}
