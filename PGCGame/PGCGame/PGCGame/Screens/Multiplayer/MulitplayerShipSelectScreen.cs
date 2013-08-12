using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace PGCGame.Screens.Multiplayer
{
    public class MulitplayerShipSelectScreen : BaseSelectScreen
    {
        public MulitplayerShipSelectScreen(SpriteBatch spriteBatch)
            : base(spriteBatch)
        {
           
        }

        public override void InitScreen(ScreenType screenType)
        {
            base.InitScreen(screenType);
        
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
