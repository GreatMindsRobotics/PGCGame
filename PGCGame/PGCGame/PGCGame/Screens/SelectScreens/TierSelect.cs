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
    class TierSelect  : BaseSelectScreen
    {
        public TierSelect(SpriteBatch spriteBatch)
            : base(spriteBatch)
        {
            
        }

        //This screen needs sprites for each tier of ship and descriptions for each tier.

        public override void LoadContent(ContentManager content)
        {

            

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
