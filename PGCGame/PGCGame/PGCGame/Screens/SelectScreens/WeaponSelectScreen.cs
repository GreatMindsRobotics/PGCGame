using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;

namespace PGCGame.Screens.SelectScreens
{
    public class WeaponSelectScreen : BaseSelectScreen
    {
        public WeaponSelectScreen(SpriteBatch spriteBatch)
            : base(spriteBatch)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            Texture2D image = content.Load<Texture2D>("Images\\NonPlayingObject\\Planet");
            SpriteFont font = content.Load<SpriteFont>("Fonts\\SegoeUIMono");

                                                                                                                                                                                                

            items.Add(new KeyValuePair<Sprite, TextSprite>(new Sprite(image, new Vector2(400, 400), Sprites.SpriteBatch), new TextSprite(Sprites.SpriteBatch, new Vector2(.8f,.8f), font, "TODO", Color.White)));



            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
