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
        UpgradeScreen upgradeScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            Texture2D greenLaser = Content.Load<Texture2D>("Images\\Bullets\\Laser");
            //Ship.BattleCruiserBullet = new PlainTexture2D(GraphicsDevice, 5, 1, Color.Red);
            Ship.BattleCruiserBullet = greenLaser;
            //Ship.DroneBullet = new PlainTexture2D(GraphicsDevice, 5, 1, Color.Red);
            Ship.DroneBullet = greenLaser;
            //Ship.FighterCarrierBullet = new PlainTexture2D(GraphicsDevice, 5, 1, Color.Red);
            Ship.FighterCarrierBullet = greenLaser;

            //TODO: Torpedo!
            Ship.Torpedo = new PlainTexture2D(GraphicsDevice, 5, 3, Color.Red);

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

            upgradeScreen = new UpgradeScreen(spriteBatch);
            upgradeScreen.LoadContent(Content);
            upgradeScreen.Name = "upgradeScreen";

            screenManager = new ScreenManager(spriteBatch, Color.White, titleScreen, mainMenuScreen, creditsScreen, gameScreen, optionScreen, shopScreen, pauseScreen, shipSelectScreen, weaponSelectScreen, upgradeScreen);
            StateManager.AllScreens = screenManager;
            
            StateManager.GraphicsManager = graphics;
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
