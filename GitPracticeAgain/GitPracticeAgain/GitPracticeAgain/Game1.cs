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

namespace GitPracticeAgain
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ScrollingBackground bg;
        Enemy enemy;
        Asteroid asteroid;
        Ship ship;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            bg = new ScrollingBackground();
            enemy = new Enemy();
            asteroid = new Asteroid(GraphicsDevice.Viewport);
            ship = new Ship();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            bg.LoadContent(Vector2.Zero, Content.Load<Texture2D>("space_background"), Color.White, 70);
            enemy.LoadContent(new Vector2(GraphicsDevice.Viewport.Width, 100), Content.Load<Texture2D>("EnemyShip2"), Color.White);
            enemy.Speed = new Vector2(4, 0);
            ship.LoadContent(Vector2.Zero, Content.Load<Texture2D>("GoodShip"), Color.White);
         
            asteroid.LoadContent(Content.Load<Texture2D>("asteroid_use"), Color.White);
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            bg.Update();
            enemy.move();
            asteroid.Update();
            ship.Update(Keyboard.GetState(), GraphicsDevice.Viewport);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            bg.Draw(spriteBatch);
            enemy.Draw(spriteBatch);
            asteroid.Draw(spriteBatch);
            ship.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
