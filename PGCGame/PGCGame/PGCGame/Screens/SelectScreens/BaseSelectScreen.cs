using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;

namespace PGCGame.Screens.SelectScreens
{
    public abstract class BaseSelectScreen : Screen
    {
        public BaseSelectScreen(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            
        }

        public void LoadContent(ContentManager content)
        {

        }

        public override void Update(GameTime gameTime)
        {

            
        }
    }
}
