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

using Glib.XNA.SpriteLib;
using Glib.XNA;
using Glib;

namespace GlenTestProgram
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager allScreens;

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
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            base.Initialize();
        }

        Texture2D bullet;
        Sprite ship;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextSprite playButton = new TextSprite(spriteBatch, 
                                                    new Vector2(GraphicsDevice.Viewport.Width / 2, 20), 
                                                    Content.Load<SpriteFont>("SpriteFont1"), 
                                                    "Play");
            playButton.IsHoverable = true;
            playButton.HoverColor = Color.Red;
            playButton.NonHoverColor = Color.Black;
            playButton.Clicked += new EventHandler(playButton_Clicked);

            bullet = Content.Load<Texture2D>("Bullet");

            target = new Sprite(Content.Load<Texture2D>("WhiteRing"), 
                                Vector2.Zero, 
                                spriteBatch);
            //target.YSpeed = 1;
            target.Speed = new Vector2(1, 1);
            target.UpdateParams.FixEdgeOff = true;
            target.Position = new Vector2(GraphicsDevice.Viewport.Width - target.Width - 1, 0);

            ship = new Sprite(Content.Load<Texture2D>("Bomber2B"), Vector2.Zero, spriteBatch);
            ship.Updated += new EventHandler(ship_Updated);
            titleScreen = new Screen(new SpriteManager(spriteBatch), Color.BlanchedAlmond);
            titleScreen.Visible = true;
            titleScreen.AdditionalSprites.Add(playButton);
            gameScreen = new Screen(new SpriteManager(spriteBatch, ship, target), Color.CornflowerBlue);
            gameScreen.Name = "MainGame";

            Screen gameOver = new Screen(new SpriteManager(spriteBatch), Color.Black);
            gameOver.Name = "gameOver";
            TextSprite gameOverTxt = new TextSprite(spriteBatch, new Vector2(GraphicsDevice.Viewport.Width / 2, 20), Content.Load<SpriteFont>("SpriteFont1"), "Game Over");
            gameOverTxt.NonHoverColor = Color.White;
            gameOverTxt.HoverColor = Color.CornflowerBlue;
            gameOverTxt.IsHoverable = true;
            gameOverTxt.Clicked += new EventHandler(gameOverTxt_Clicked);
            gameOver.AdditionalSprites.Add(gameOverTxt);

            allScreens = new ScreenManager(spriteBatch, Color.Black, titleScreen, gameScreen, gameOver);

            KeyboardManager.KeyDown += new SingleKeyEventHandler(potentialPress);
            
        }

        void gameOverTxt_Clicked(object sender, EventArgs e)
        {
            Exit();
        }

        Sprite target;

        void potentialPress(object source, SingleKeyEventArgs e)
        {
            if (e.Key == Keys.Space && gameScreen.Visible)
            {
                //Fire bullet
                Sprite newBullet = new Sprite(bullet, new Vector2(ship.X+ship.Width, ship.Y + (ship.Height / 2)), spriteBatch);
                newBullet.XSpeed = 2;

                //Inefficient intersection check
                newBullet.Updated += new EventHandler(newBullet_Updated);
                newBullet.Rotation = new SpriteRotation(270);
                gameScreen.Sprites.Add(newBullet);
            }
        }

        void newBullet_Updated(object sender, EventArgs e)
        {
            if (sender.Cast<Sprite>().Intersects(target))
            {
                //Got a point!
                gameScreen.Sprites.Remove(sender.Cast<Sprite>());
                if (target.Color == Color.White)
                {
                    target.Color = Color.Red;
                }
                else
                {
                    allScreens["gameOver"].Visible = true;
                    gameScreen.Visible = false;
                }
            }
        }

        static int MoveSpeed = 2;

        void ship_Updated(object sender, EventArgs e)
        {
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Right) && (ship.X + MoveSpeed) < (GraphicsDevice.Viewport.Width - ship.Width - target.Width))
            {
                ship.X += MoveSpeed;
            }
            if (keys.IsKeyDown(Keys.Left) && (ship.X - MoveSpeed) > 0)
            {
                ship.X -= MoveSpeed;
            }
            if (keys.IsKeyDown(Keys.Up) && (ship.Y - MoveSpeed) > 0)
            {
                ship.Y -= MoveSpeed;
            }
            if (keys.IsKeyDown(Keys.Down) && (ship.Y + MoveSpeed) < (GraphicsDevice.Viewport.Height - ship.Height))
            {
                ship.Y+=MoveSpeed;
            }
            
        }
        Screen gameScreen;
        Screen titleScreen;

        void playButton_Clicked(object sender, EventArgs e)
        {
            // TODO: Play game
            titleScreen.Visible = false;
            gameScreen.Visible = true;
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

            // TODO: Add your update logic here
            //manager.Update(gameTime);
            allScreens.Update();
            KeyboardManager.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            //manager.Draw();
            allScreens.Draw();
            base.Draw(gameTime);
        }
    }
}
