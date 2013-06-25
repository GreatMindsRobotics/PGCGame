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

namespace GitPractice
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /* TODO: Kevin
         * Change the enemies image to the "snake" image.
         * Create, load, update, and draw a horizontal enemy.
         * 
         * */

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Ship spaceShip;
        VerticalEnemy enemy;
        

        Song bgMusic;

        Texture2D backgroundImage;
        Vector2 backgroundLocation;
        Color backgroundTint;

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
            spaceShip = new Ship();
            enemy = new VerticalEnemy();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spaceShip.LoadContent(Content, "ship");
            spaceShip.Speed = new Vector2(5, 5);


            enemy.LoadContent(Content, "coin");
            enemy.Speed = new Vector2(0, 5);
            enemy.MoveDirection = MoveDirection.Right;

            bgMusic = Content.Load<Song>("bgMusic");
            MediaPlayer.Play(bgMusic);
            MediaPlayer.IsRepeating = true;
            backgroundImage = Content.Load<Texture2D>("bgImage");
            backgroundLocation = Vector2.Zero;
            backgroundTint = Color.White;
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

            spaceShip.Update(Keyboard.GetState(), gameTime, GameState.Playing, graphics.GraphicsDevice.Viewport);
            enemy.Update(gameTime, GameState.Playing, MoveDirection.Down, graphics.GraphicsDevice.Viewport);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundImage, backgroundLocation, backgroundTint);

            enemy.Draw(spriteBatch);

            

            spaceShip.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
