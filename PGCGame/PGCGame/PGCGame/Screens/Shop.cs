using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PGCGame.Screens
{
    public class Shop : Screen
    {
        public Shop(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            
        }


        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT

            //use Sprites to load your sprites
            //EX: Sprites.Add(new Sprite(content.Load<Texture2D>("assetName"), new Vector2(0, 0), Sprites.SpriteBatch));
            //OR
            //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
            
        }
    }
}
