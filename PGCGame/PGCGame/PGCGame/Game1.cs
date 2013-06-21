using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Glib.XNA;
using Glib;
using Glib.XNA.SpriteLib;

using PGCGame.Screens;
using PGCGame.Screens.SelectScreens;

namespace PGCGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager screenManager;

        Title titleScreen;
        MainMenu mainMenuScreen;
        Credits creditsScreen;
        Options optionScreen;
        PauseScreen pauseScreen;
        ShipSelect shipSelectScreen;
        WeaponSelectScreen weaponSelectScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            graphics.DeviceReset += new EventHandler<EventArgs>(graphics_DeviceReset);
            Content.RootDirectory = "Content";
        }

        void graphics_DeviceReset(object sender, EventArgs e)
        {

            foreach (Screen s in StateManager.AllScreens)
            {
                s.Target = new RenderTarget2D(s.Graphics, graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height);
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {   
            base.Initialize();
            IsMouseVisible = true;
        }

        GameScreen gameScreen;
        Shop shopScreen;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //TODO: Load in bullet textures
            Ship.BattleCruiserBullet = new PlainTexture2D(GraphicsDevice, 5, 1, Color.Red);
            Ship.DroneBullet = new PlainTexture2D(GraphicsDevice, 5, 1, Color.Red);
            Ship.FighterCarrierBullet = new PlainTexture2D(GraphicsDevice, 5, 1, Color.Red);
            Ship.Torpedo = new PlainTexture2D(GraphicsDevice, 5, 1, Color.Red);

            shopScreen = new Shop(spriteBatch);
            shopScreen.LoadContent(Content);
            shopScreen.Name = "shopScreen";

            titleScreen = new Title(spriteBatch, new Title.quitFunction(Exit));
            titleScreen.LoadContent(Content);
            titleScreen.Name = "titleScreen";
            titleScreen.Visible = true;

            mainMenuScreen = new MainMenu(spriteBatch);
            mainMenuScreen.LoadContent(Content);
            mainMenuScreen.Name = "mainMenuScreen";

            creditsScreen = new Credits(spriteBatch);
            creditsScreen.LoadContent(Content);
            creditsScreen.Name = "creditsScreen";

            gameScreen = new GameScreen(spriteBatch);
            gameScreen.LoadContent(Content);
            gameScreen.Name = "gameScreen";

            optionScreen = new Options(spriteBatch);
            optionScreen.LoadContent(Content);
            optionScreen.Name = "optionScreen";

            pauseScreen = new PauseScreen(spriteBatch);
            pauseScreen.LoadContent(Content);
            pauseScreen.Name = "pauseScreen";

            shipSelectScreen = new ShipSelect(spriteBatch);
            shipSelectScreen.LoadContent(Content);
            shipSelectScreen.Name = "shipSelectScreen";

            weaponSelectScreen = new WeaponSelectScreen(spriteBatch);
            weaponSelectScreen.LoadContent(Content);
            weaponSelectScreen.Name = "weaponSelectScreen";

            screenManager = new ScreenManager(spriteBatch, Color.White, titleScreen, mainMenuScreen, creditsScreen, gameScreen, optionScreen, shopScreen, pauseScreen, shipSelectScreen, weaponSelectScreen);
            StateManager.AllScreens = screenManager;
            
            StateManager.GraphicsManager = graphics;
        }

        /// <summary>
        /// Modifies the display mode for the graphics device 
        /// when it is reset or recreated.
        /// </summary>
        void graphics_PreparingDeviceSettings(object sender,
            PreparingDeviceSettingsEventArgs e)
        {
            //First call - rely on defaults
            if (graphics.GraphicsDevice == null)
            {
                StateManager.Resolution = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                return;
            }



            //Reset PreferredBackBuffer to the new DisplayMode size
            PresentationParameters screenParams = new PresentationParameters();

            screenParams.BackBufferFormat = graphics.GraphicsDevice.DisplayMode.Format;
            screenParams.BackBufferWidth = StateManager.Options.HDEnabled ? graphics.GraphicsDevice.DisplayMode.Width : StateManager.Resolution.X;
            screenParams.BackBufferHeight = StateManager.Options.HDEnabled ? graphics.GraphicsDevice.DisplayMode.Height : StateManager.Resolution.Y;
            screenParams.DeviceWindowHandle = Window.Handle;

            GraphicsDevice.Reset(screenParams);
            GraphicsDevice.Viewport = new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight) { MinDepth = 0.0f, MaxDepth = 1.0f };
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            screenManager.Draw();

            base.Draw(gameTime);
        }
    }
}
