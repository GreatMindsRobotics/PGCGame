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
using PGCGame.Screens.Multiplayer;

namespace PGCGame
{
    /// <summary>
    /// Main type for PGCGame
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
        GameScreen gameScreen;
        Shop shopScreen;
        ControlScreen controlScreen;
        NetworkSelectScreen networkScreen;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Plequarius: Galactic Commanders";
        }

        /// <summary>
        /// Initialize StateManager and InputManager
        /// </summary>
        protected override void Initialize()
        {
            StateManager.GraphicsManager = graphics;
            StateManager.IsWindowFocused = new Delegates.CheckIfWindowFocused(() => IsActive);
            StateManager.Exit = new Delegates.QuitFunction(() => Exit());

            IsMouseVisible = true;
            Components.Add(new InputManagerComponent(this));
            
            base.Initialize();
        }

        /// <summary>
        /// Load Game Assets and initialize all screens
        /// </summary>
        protected override void LoadContent()
        {
            //Instantiate the singleton class GameContent and load all game assets
            GameContent gameContent = new GameContent(Content);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Instantiate the singleton class HorizontalMenuBGSprite
            HorizontalMenuBGSprite scrollingBG = new HorizontalMenuBGSprite(GameContent.GameAssets.Images.Backgrounds.Screens[ScreenBackgrounds.GlobalScrollingBg], spriteBatch);

            
            //Initialize screens

            titleScreen = new Title(spriteBatch, new Delegates.QuitFunction(Exit));
            titleScreen.InitScreen(ScreenType.Title);
            titleScreen.Visible = true;

            shopScreen = new Shop(spriteBatch);
            shopScreen.InitScreen(ScreenType.Shop);

            controlScreen = new ControlScreen(spriteBatch);
            controlScreen.InitScreen(ScreenType.ControlScreen);

            mainMenuScreen = new MainMenu(spriteBatch);
            mainMenuScreen.InitScreen(ScreenType.MainMenu);

            creditsScreen = new Credits(spriteBatch);
            creditsScreen.InitScreen(ScreenType.Credits);

            gameScreen = new GameScreen(spriteBatch);
            gameScreen.InitScreen(ScreenType.Game);

            optionScreen = new Options(spriteBatch);
            optionScreen.InitScreen(ScreenType.Options);

            pauseScreen = new PauseScreen(spriteBatch);
            pauseScreen.InitScreen(ScreenType.Pause);

            shipSelectScreen = new ShipSelect(spriteBatch);
            shipSelectScreen.InitScreen(ScreenType.ShipSelect);

            weaponSelectScreen = new WeaponSelectScreen(spriteBatch);
            weaponSelectScreen.InitScreen(ScreenType.WeaponSelect);

            upgradeScreen = new UpgradeScreen(spriteBatch);
            upgradeScreen.InitScreen(ScreenType.UpgradeScreen);

            tierSelectScreen = new TierSelect(spriteBatch);
            tierSelectScreen.InitScreen(ScreenType.TierSelect);

            levelSelectScreen = new LevelSelect(spriteBatch);
            levelSelectScreen.InitScreen(ScreenType.LevelSelect);

            networkScreen = new NetworkSelectScreen(spriteBatch);
            networkScreen.InitScreen(ScreenType.NetworkSelectScreen);

            screenManager = new ScreenManager(spriteBatch, Color.White, titleScreen, mainMenuScreen, creditsScreen, gameScreen, optionScreen, shopScreen, pauseScreen, shipSelectScreen, weaponSelectScreen, upgradeScreen, tierSelectScreen, levelSelectScreen, controlScreen, networkScreen);
            StateManager.AllScreens = screenManager;
            StateManager.ScreenState = CoreTypes.ScreenType.Title;
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
