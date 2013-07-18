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

namespace PGCGame.Screens.Multiplayer
{
    public class NetworkSelectScreen: BaseScreen
    {
        public NetworkSelectScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
#if WINDOWS
            StateManager.ScreenStateChanged += new EventHandler(StateManager_ScreenStateChanged);
#endif
        }
        TimeSpan elapsedButtonDelay = TimeSpan.Zero;

        void StateManager_ScreenStateChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                elapsedButtonDelay = TimeSpan.Zero;
            }
        }

        Sprite LANButton;
        TextSprite LANLabel;

        Sprite HostButton;
        TextSprite HostLabel;

        Sprite BackButton;
        TextSprite BackLabel;

        public override void InitScreen(ScreenType screnType)
        {
            base.InitScreen(screnType);

            StateManager.Options.MusicStateChanged += new EventHandler(Options_MusicStateChanged);
            
            Texture2D planetTexture = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
            Texture2D altPlanetTexture = GameContent.GameAssets.Images.NonPlayingObjects.AltPlanet;
            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;



            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            BackButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            LANButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .6f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f), Sprites.SpriteBatch);
            HostButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .06f), Sprites.SpriteBatch);

            Sprites.Add(BackButton);

            BackLabel = new TextSprite(Sprites.SpriteBatch, Vector2.One * 10, SegoeUIMono, "Back");
            BackLabel.IsHoverable = true;
            BackLabel.ParentSprite = BackButton;
            BackLabel.NonHoverColor = Color.White;
            BackLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(BackLabel);

            Sprites.Add(LANButton);
            LANButton.Scale = new Vector2(1.5f,1);
            LANLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Scan for LAN sectors");
            LANLabel.IsHoverable = true;
            LANLabel.ParentSprite = LANButton;
            LANLabel.NonHoverColor = Color.White;
            LANLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(LANLabel);
            
            Sprites.Add(HostButton);
            HostButton.Scale = new Vector2(1.1f, 1);
            HostLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Host LAN sector");
            HostLabel.IsHoverable = true;
            HostLabel.ParentSprite = HostButton;
            HostLabel.NonHoverColor = Color.White;
            HostLabel.HoverColor = Color.MediumAquamarine;
            AdditionalSprites.Add(HostLabel);








#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { { BackLabel, LANLabel }}, InputType.LeftJoystick);
            AllButtons.ButtonPress += new EventHandler(AllButtons_ButtonPress);
            BackLabel.IsSelected = true;
#elif WINDOWS
            BackLabel.Clicked += new EventHandler(delegate(object src, EventArgs e) { if (this.Visible && elapsedButtonDelay > totalButtonDelay) { StateManager.GoBack(); } });
            
#endif
        }

        void Options_MusicStateChanged(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
        }

#if XBOX
        void AllButtons_ButtonPress(object sender, EventArgs e)
        {
            if (BackLabel.IsSelected)
            {
                StateManager.Exit();
            }
            else if (LANLabel.IsSelected)
            {
                StateManager.ScreenState = ScreenType.ShipSelect;
            }

        }
#endif

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
           
            BackButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .06f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .60f);
           

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

#if XBOX
        GamePadButtonEnumerator AllButtons;
#elif WINDOWS
        //Preventing clickthrus
       // TimeSpan elapsedButtonDelay = TimeSpan.Zero;
        TimeSpan totalButtonDelay = TimeSpan.FromMilliseconds(250);
#endif


        public override void Update(GameTime gameTime)
        {
#if WINDOWS
            elapsedButtonDelay += gameTime.ElapsedGameTime;
            
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
