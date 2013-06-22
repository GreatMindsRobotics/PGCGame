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
    class UpgradeScreen : BaseSelectScreen
    {
       public UpgradeScreen(SpriteBatch spriteBatch)
       : base(spriteBatch)
        {
            
        }

       public override void LoadContent(ContentManager content)
       {
           //TODO: Load upgrade sprites and description of upgrades.

           base.LoadContent(content);
       }


       public override void Update(GameTime gameTime)
       {
           
           base.Update(gameTime);
       }
    }
}
