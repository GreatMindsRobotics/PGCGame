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

        List<BaseEnemy> enemyList = new List<BaseEnemy>();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;
        Vector2 scorePosition;
        String text;

        TimeSpan elapsedGameTime;
        Random random;

        int scoreNumber;
        String scoreText;
        Vector2 number;

        Ship spaceShip;

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
            elapsedGameTime = new TimeSpan();
            random = new Random();

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

            scoreText = scoreNumber.ToString();

            spaceShip.LoadContent(Content, "GalagaShip");
            spaceShip.Speed = new Vector2(7, 7);

            font = Content.Load<SpriteFont>("SpriteFont1");
            scorePosition = Vector2.Zero;
            text = "Score: ";
            scoreNumber = 0;
            number = new Vector2(85, 0);

            /*
            VerticalEnemy enemy = new VerticalEnemy();
            enemy.LoadContent(Content, "coin");
            enemy.Speed = Vector2.Zero;
            enemy.MoveDirection = MoveDirection.Down;

            HorizontalEnemy horizontalEnemy = new HorizontalEnemy();
            horizontalEnemy.LoadContent(Content, "coin");
            horizontalEnemy.Speed = new Vector2(0, 0);
            horizontalEnemy.MoveDirection = MoveDirection.Right;
             * */

            Coin3 coin3 = new Coin3();
            coin3.LoadContent(Content, "coin");
            coin3.Speed = Vector2.Zero;
            coin3.MoveDirection = MoveDirection.Right;

            Coin4 coin4 = new Coin4();
            coin4.LoadContent(Content, "coin");
            coin4.Speed = Vector2.Zero;
            coin4.MoveDirection = MoveDirection.Right;

            /*
            enemyList.Add(enemy);
            enemyList.Add(horizontalEnemy);
            */
            enemyList.Add(coin3);
            enemyList.Add(coin4);


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

            elapsedGameTime += gameTime.ElapsedGameTime;
            if (elapsedGameTime >= new TimeSpan(0, 0, 1))
            {
                if (random.Next(2) == 1)
                {
                    Coin3 coin = new Coin3();
                    coin.LoadContent(Content, "coin");
                    coin.Speed = Vector2.Zero;
                    coin.MoveDirection = MoveDirection.Right;

                    enemyList.Add(coin);
                }
                else
                {
                    Coin4 coin = new Coin4();
                    coin.LoadContent(Content, "coin");
                    coin.Speed = Vector2.Zero;
                    coin.MoveDirection = MoveDirection.Right;

                    enemyList.Add(coin);
                }

                elapsedGameTime = TimeSpan.Zero;
            }

            spaceShip.Update(Keyboard.GetState(), gameTime, GameState.Playing, graphics.GraphicsDevice.Viewport);

            foreach (BaseEnemy enemy in enemyList)
            {
                enemy.Update(gameTime, GameState.Playing, MoveDirection.Right, graphics.GraphicsDevice.Viewport);
            }

            foreach (Bullet bullet in spaceShip.FlyingBullets)
            {
                for (int e = 0; e < enemyList.Count; e++)
                {
                    BaseEnemy enemy = enemyList[e];

                    if (bullet.Rect.Intersects(enemy.Rect))
                    {
                        scoreNumber++;
                        bullet.IsDead = true;

                        enemyList.Remove(enemy);
                        e--;
                    }
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    BaseEnemy coin = enemyList[i];

                    if (coin.GetType() == typeof(Coin3) || coin.GetType() == typeof(Coin4))
                    {
                        scoreNumber++;
                        enemyList.Remove(coin);
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aqua);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundImage, backgroundLocation, backgroundTint);


            spriteBatch.DrawString(font, text, scorePosition, Color.Green);
            spriteBatch.DrawString(font, scoreNumber.ToString(), number, Color.Green);

            foreach (BaseEnemy enemy in enemyList)
            {
                enemy.Draw(spriteBatch);
            }

            spaceShip.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
