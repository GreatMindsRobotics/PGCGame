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

namespace PGCGame.Screens.Multiplayer
{
    public class AvailableSessionsScreen : BaseScreen
    {
        public AvailableSessionsScreen(SpriteBatch sb)
            : base(sb, Color.Black)
        {

        }

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            TextSprite title = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.BoldText, "Available LAN Sectors");
            title.Color = Color.White;
            title.Y = 5;
            title.X = title.GetCenterPosition(Graphics.Viewport).X;
            
            AdditionalSprites.Add(title);
        }
    }
}
