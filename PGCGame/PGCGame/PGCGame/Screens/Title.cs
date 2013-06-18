using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Glib.XNA.SpriteLib;
using Glib.XNA;
using Glib;

namespace PGCGame.Screens
{
    public class Title : Screen
    {
        

        public Title(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT
            
            //use Sprites to load your sprites

            Sprites.AddNewSprite(new Vector2(200, 100), content.Load<Texture2D>("Gametitle"));

            Sprites.AddNewSprite(new Vector2(300, 200), content.Load<Texture2D>("Button"));
            TextSprite PlayLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(360, 210), content.Load<SpriteFont>("TitleFont"), "Play");
            AdditionalSprites.Add(PlayLabel);

            Sprites.AddNewSprite(new Vector2(300, 300), content.Load<Texture2D>("Button"));
            TextSprite ExitLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(200, 300), content.Load<SpriteFont>("TitleFont"), "Exit");
            AdditionalSprites.Add(ExitLabel);

            //Sprites.Add(new Sprite(content.Load<Texture2D>("assetName"), new Vector2(0, 0), Sprites.SpriteBatch));
            TextSprite PlequariusGalaticCommandersLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(0, 0), content.Load<SpriteFont>("TitleFont"), "Title");
            AdditionalSprites.Add(PlequariusGalaticCommandersLabel);


            //OR
            //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
        }

        public override void Update(GameTime gameTime)
        {
            //TODO: UPDATE SPRITES
        }
    }
}
