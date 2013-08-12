using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PGCGame.Screens.Multiplayer
{
    public class NetworkMatchTypeSelectionScreen : BaseScreen
    {
        public NetworkMatchTypeSelectionScreen(SpriteBatch sb)
            : base(sb, Color.Black)
        {

        }

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
        }
    }
}
