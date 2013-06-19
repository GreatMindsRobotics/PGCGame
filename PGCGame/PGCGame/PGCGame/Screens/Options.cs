using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace PGCGame.Screens
{
    public class Options : Screen
    {
        public Options(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Pink)
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //TODO: UPDATE SPRITES
        }
    }
}
