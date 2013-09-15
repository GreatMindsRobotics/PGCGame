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
using Glib.XNA.InputLib;


namespace blah
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public SpriteManager spriteManager;

        Screen titleScreen;
        Screen gameScreen;
        ScreenManager screenManager;

        Player player;

        KeyboardState keyboardState;
        GameTime gameTime;

        Texture2D bullet_blueTexture;
        Texture2D bullet_greenTexture;
        Texture2D bullet_yellowTexture;
        Texture2D shipTexture;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Sprite> enemies = new List<Sprite>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            IsMouseVisible = true;
            Components.Add(new InputManagerComponent(this));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bullet_blueTexture = this.Content.Load<Texture2D>("bullet_blue");
            bullet_greenTexture = this.Content.Load<Texture2D>("bullet_green");
            bullet_yellowTexture = this.Content.Load<Texture2D>("bullet_yellow");
            shipTexture = this.Content.Load<Texture2D>("spaceship");

            player = new Player(shipTexture, new Vector2(this.GraphicsDevice.Viewport.Width/2-50, this.GraphicsDevice.Viewport.Height-200), spriteBatch, 1, new Vector2(0,1), 1, new Vector2(0,1));
            player.setBulletTexture(bullet_blueTexture);
            player.Speed = new Vector2(1,0);
            player.Updated += new EventHandler(player_Updated);
            spriteManager = new SpriteManager(spriteBatch, player);

            gameScreen = new Screen(spriteManager, Color.PapayaWhip);
            gameScreen.Visible = false;

            titleScreen = new Screen(spriteBatch, Color.PapayaWhip);
            titleScreen.Visible = true;

            TextSprite blahLabel = new TextSprite(spriteBatch, Content.Load<SpriteFont>("BLAHFONT"), "PLAY THE BLAH GAME!!!!!");
            blahLabel.X = blahLabel.GetCenterPosition(GraphicsDevice.Viewport).X;

            TextSprite playLabel = new TextSprite(spriteBatch, Content.Load<SpriteFont>("BLAHFONT"), "CLICK-------->PLAY NOW<------CLICK");
            playLabel.X = playLabel.GetCenterPosition(GraphicsDevice.Viewport).X;
            playLabel.Y = 100;
            playLabel.IsHoverable = true;
            playLabel.HoverColor = Color.Red;
            playLabel.NonHoverColor = Color.Black;
            playLabel.Pressed += new EventHandler(playLabel_Clicked);

            titleScreen.AdditionalSprites.Add(blahLabel);
            titleScreen.AdditionalSprites.Add(playLabel);

            screenManager = new ScreenManager(spriteBatch, Color.White, titleScreen, gameScreen);

            // TODO: use this.Content to load your game content here
        }

        void player_Updated(object sender, EventArgs e)
        {
            player.updatePlayer(keyboardState, gameTime);
        }

        void playLabel_Clicked(object sender, EventArgs e)
        {
            titleScreen.Visible = false;
            gameScreen.Visible = true;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            gameTime = gametime;

            keyboardState = Keyboard.GetState();
         
            screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override bool BeginDraw()
        {
            screenManager.BeginDraw();
            return base.BeginDraw();
        }

        protected override void EndDraw()
        {
            screenManager.EndDraw();
            base.EndDraw();
        }

        protected override void Draw(GameTime gameTime)
        {

            screenManager.Draw();

            base.Draw(gameTime);
        }
    }
}
