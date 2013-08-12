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
        AvailableSessionsScreen availableNetSessions;
        NetworkMatchTypeSelectionScreen netMatchCreate;
        MainMenu mainMenuScreen;
        Credits creditsScreen;
        Options optionScreen;
        PauseScreen pauseScreen;
        WeaponSelectScreen weaponSelectScreen;
        UpgradeScreen upgradeScreen;
        TierSelect tierSelectScreen;
        LevelSelect levelSelectScreen;
        GameScreen gameScreen;
        Shop shopScreen;
        GameOver gameOver;
        ControlScreen controlScreen;
        LoadingScreen loadingScreen;
        NetworkSelectScreen networkScreen;
        TransitionScreen transitionScreen;
        LevelCompleteScreen levelCompleteScreen;
        LobbyScreen lobbyScreen;
        WarnScreen warnScreen;
        MulitplayerShipSelectScreen multiplayerShipSelectScreen;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Plequarius: Galactic Commanders";

#if XBOX
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();

            graphics.GraphicsDevice.Viewport = new Viewport(0, 0, 1920, 1080) { MinDepth = 0, MaxDepth = 1 };
#endif

        }

        /// <summary>
        /// Initialize StateManager and InputManager
        /// </summary>
        protected override void Initialize()
        {
            StateManager.GraphicsManager = graphics;
            StateManager.IsWindowFocused = new Delegates.CheckIfWindowFocused(() => IsActive);
            StateManager.Exit = new Delegates.QuitFunction(() => Exit());

            GamerServicesComponent services = new GamerServicesComponent(this);
            IsMouseVisible = true;
            Components.Add(new InputManagerComponent(this));
            Components.Add(services);

            StateManager.InitGame(this);

            TargetElapsedTime = new TimeSpan(TargetElapsedTime.Ticks / StateManager.DebugData.OverclockAmount);

            try
            {
                base.Initialize();
            }
            catch (GamerServicesNotAvailableException) { StateManager.GamerServicesAreAvailable = false; Components.Remove(services); base.Initialize(); };
        }

        /// <summary>
        /// Load Game Assets and initialize all screens
        /// </summary>
        protected override void LoadContent()
        {
            //Instantiate the singleton class GameContent and load all game assets, done in ctor
            new GameContent(Content);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Instantiate the singleton class HorizontalMenuBGSprite
            HorizontalMenuBGSprite scrollingBG = new HorizontalMenuBGSprite(GameContent.GameAssets.Images.Backgrounds.Screens[ScreenBackgrounds.GlobalScrollingBg], spriteBatch);

            
            //Initialize screens

            availableNetSessions = new AvailableSessionsScreen(spriteBatch);
            availableNetSessions.InitScreen(ScreenType.NetworkSessionsScreen);

            titleScreen = new Title(spriteBatch);
            titleScreen.InitScreen(ScreenType.Title);

            netMatchCreate = new NetworkMatchTypeSelectionScreen(spriteBatch);
            netMatchCreate.InitScreen(ScreenType.NetworkMatchSelection);

            gameOver = new GameOver(spriteBatch);
            gameOver.InitScreen(ScreenType.GameOver);

            levelCompleteScreen = new LevelCompleteScreen(spriteBatch);
            levelCompleteScreen.InitScreen(ScreenType.LevelCompleteScreen);

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

            transitionScreen = new TransitionScreen(spriteBatch);
            transitionScreen.InitScreen(ScreenType.TransitionScreen);

            loadingScreen = new LoadingScreen(spriteBatch, Color.Black);
            loadingScreen.InitScreen(ScreenType.LoadingScreen);

            lobbyScreen = new LobbyScreen(spriteBatch);
            lobbyScreen.InitScreen(ScreenType.NetworkLobbyScreen);

            warnScreen = new WarnScreen(spriteBatch);
            warnScreen.InitScreen(ScreenType.WarnHostScreen);

            multiplayerShipSelectScreen = new MulitplayerShipSelectScreen(spriteBatch);
            multiplayerShipSelectScreen.InitScreen(ScreenType.MultiPlayerShipSelect);

            screenManager = new ScreenManager(spriteBatch, Color.White, titleScreen, mainMenuScreen, creditsScreen, gameScreen, optionScreen, shopScreen, pauseScreen, weaponSelectScreen, upgradeScreen, tierSelectScreen, levelSelectScreen, controlScreen, networkScreen, gameOver, transitionScreen, levelCompleteScreen, loadingScreen, availableNetSessions, lobbyScreen, warnScreen, netMatchCreate, multiplayerShipSelectScreen);
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
            GameContent.GameAssets.Dispose();
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

            if (StateManager.DebugData.InfiniteMoney)
            {
                StateManager.SpaceBucks = int.MaxValue;
            }

            if (StateManager.NetworkData.CurrentSession != null)
            {
                StateManager.NetworkData.CurrentSession.Update();
            }
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
                //TODO: Do something here?
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
