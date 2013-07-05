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
using Glib.XNA.InputLib;
using PGCGame.CoreTypes;

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
        UpgradeScreen upgradeScreen;
        TierSelect tierSelectScreen;
        LevelSelect levelSelectScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Plequarius: Galactic Commanders";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            StateManager.GraphicsManager = graphics;
            IsMouseVisible = true;
            Components.Add(new InputManagerComponent(this));
            StateManager.IsWindowFocused = new Delegates.CheckIfWindowFocused(isFocused);
            base.Initialize();
        }



        GameScreen gameScreen;
        Shop shopScreen;

        bool isFocused()
        {
            return IsActive;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Instantiate the singleton class GameContent and load all game assets
            GameContent gameContent = new GameContent(Content);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            shopScreen = new Shop(spriteBatch);
            shopScreen.InitScreen();
            shopScreen.Name = "shopScreen";

            titleScreen = new Title(spriteBatch, new Delegates.QuitFunction(Exit));
            titleScreen.InitScreen();
            titleScreen.Name = "titleScreen";
            titleScreen.Visible = true;

            mainMenuScreen = new MainMenu(spriteBatch, new Delegates.QuitFunction(Exit));
            mainMenuScreen.InitScreen();
            mainMenuScreen.Name = "mainMenuScreen";

            creditsScreen = new Credits(spriteBatch);
            creditsScreen.InitScreen();
            creditsScreen.Name = "creditsScreen";

            gameScreen = new GameScreen(spriteBatch);
            gameScreen.LoadContent();
            gameScreen.Name = "gameScreen";

            optionScreen = new Options(spriteBatch);
            optionScreen.InitScreen();
            optionScreen.Name = "optionScreen";

            pauseScreen = new PauseScreen(spriteBatch);
            pauseScreen.InitScreen();
            pauseScreen.Name = "pauseScreen";

            shipSelectScreen = new ShipSelect(spriteBatch);
            shipSelectScreen.InitScreen();
            shipSelectScreen.Name = "shipSelectScreen";

            weaponSelectScreen = new WeaponSelectScreen(spriteBatch);
            weaponSelectScreen.InitScreen();
            weaponSelectScreen.Name = "weaponSelectScreen";

            upgradeScreen = new UpgradeScreen(spriteBatch);
            upgradeScreen.InitScreen();
            upgradeScreen.Name = "upgradeScreen";

            tierSelectScreen = new TierSelect(spriteBatch);
            tierSelectScreen.InitScreen();
            tierSelectScreen.Name = "tierSelectScreen";

            levelSelectScreen = new LevelSelect(spriteBatch);
            levelSelectScreen.InitScreen();
            levelSelectScreen.Name = "levelSelectScreen";

            screenManager = new ScreenManager(spriteBatch, Color.White, titleScreen, mainMenuScreen, creditsScreen, gameScreen, optionScreen, shopScreen, pauseScreen, shipSelectScreen, weaponSelectScreen, upgradeScreen, tierSelectScreen, levelSelectScreen);
            StateManager.AllScreens = screenManager;
            StateManager.ScreenState = CoreTypes.ScreenState.Title;
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
            base.Update(gameTime);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            screenManager.Update(gameTime);

            
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
