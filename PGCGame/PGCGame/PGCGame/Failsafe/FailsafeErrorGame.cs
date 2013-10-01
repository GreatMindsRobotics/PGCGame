using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Glib.XNA.SpriteLib;
using Glib.XNA;
using Glib;
using Glib.XNA.InputLib;

namespace PGCGame.Failsafe
{
    /// <summary>
    /// Failsafe game error screen class.
    /// </summary>
    public class FailsafeErrorGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ScreenManager screenManager;
        Exception error;

        public FailsafeErrorGame(Exception ex) : base() 
        {
            error = ex;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Error During PGC Runtime";
        }

        Screen mainScreen;

        protected override void Initialize()
        {
            IsMouseVisible = true;
            Components.Add(new InputManagerComponent(this));
            base.Initialize();
        }

        /// <summary>
        /// Load Game Assets and initialize all screens
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainScreen = new Screen(spriteBatch, Color.Black);
            mainScreen.Name = "ErrorDisplay";
            mainScreen.Visible = true;
            Sprite parentButton = mainScreen.Sprites.AddNewSprite(Vector2.Zero, Content.Load<Texture2D>("Images\\Controls\\Button"));
            parentButton.Position = parentButton.GetCenterPosition(GraphicsDevice.Viewport);

            TextSprite title = new TextSprite(spriteBatch, Content.Load<SpriteFont>("Fonts\\SegoeUIMonoBold"), "Runtime Error in PGCGame", Color.Red);

            title.Text += ": " + this.error.GetType().FullName;

            title.X = title.GetCenterPosition(GraphicsDevice.Viewport).X;
            title.Y = 5;
            mainScreen.AdditionalSprites.Add(title);

            TextSprite details = new TextSprite(spriteBatch, Content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Please report this error at: \nhttp://pgcgame.com/ReportError", Color.White);
            details.Y = title.Y + title.Font.LineSpacing;
            details.X = details.GetCenterPosition(GraphicsDevice.Viewport).X;
            mainScreen.AdditionalSprites.Add(details);

            TextSprite exceptMsg = new TextSprite(spriteBatch, Content.Load<SpriteFont>("Fonts\\SegoeUIMono"), error.Message, Color.White);
            exceptMsg.X = exceptMsg.GetCenterPosition(GraphicsDevice.Viewport).X;
            exceptMsg.Y = details.Y + 1 + details.Height;
            mainScreen.AdditionalSprites.Add(exceptMsg);

            TextSprite reportError = new TextSprite(spriteBatch, Content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "Report Error", Color.White);
            reportError.IsHoverable = true;
            reportError.ParentSprite = parentButton;
            reportError.HoverColor = Color.MediumAquamarine;
            reportError.NonHoverColor = Color.White;
            reportError.Pressed += new EventHandler(reportError_Pressed);
            mainScreen.AdditionalSprites.Add(reportError);
            screenManager = new ScreenManager(spriteBatch, Color.DarkRed, mainScreen);
            base.LoadContent();
        }

        private void ErrLinkOpen(TextSprite clicked)
        {
            clicked.IsHoverable = false;
            clicked.Text = "Error opening website, please report manually";
            clicked.ParentSprite.Visible = false;
            clicked.X = clicked.GetCenterPosition(GraphicsDevice.Viewport).X;
        }

        void reportError_Pressed(object sender, EventArgs e)
        {
#if WINDOWS
            try
            {
                
                System.Diagnostics.Process.Start(string.Format("http://pgcgame.com/ReportError?except={0}&msg={1}&innerexcept={2}&stack={3}", System.Uri.EscapeDataString(error.GetType().FullName), Uri.EscapeDataString(error.Message), Uri.EscapeDataString(error.InnerException == null ? "No Inner Exception" : error.InnerException.GetType().FullName), Uri.EscapeDataString(error.StackTrace)));
            }
            catch
            {
                ErrLinkOpen(sender as TextSprite);
            }
#else
            ErrLinkOpen(sender as TextSprite);
#endif
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            screenManager.Update(gameTime);
            
        }

        protected override bool BeginDraw()
        {
            //Fixes windows key crash in full screen
            try
            {
                screenManager.BeginDraw();
            }  
            catch(InvalidOperationException)
            {
                //Don't draw frame
                return false;
            }
            return base.BeginDraw();
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            screenManager.Draw();

            base.Draw(gameTime);
        }

        protected override void EndDraw()
        {
            screenManager.EndDraw();
            base.EndDraw();
        }
    }
}
