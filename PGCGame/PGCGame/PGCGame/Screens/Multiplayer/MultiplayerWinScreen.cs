using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib.XNA.SpriteLib;

namespace PGCGame.Screens.Multiplayer
{
    public class MultiplayerWinScreen : BaseScreen
    {
        public MultiplayerWinScreen(SpriteBatch sb) : base(sb, Color.Black) { }

        TextSprite title;
        Sprite planet;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
            title = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.BoldText, "You Won!", Color.White);
            planet = Sprites.AddNewSprite(new Vector2(-3, Graphics.Viewport.Height - GameContent.Assets.Images.NonPlayingObjects.Planet.Height + 11), GameContent.Assets.Images.NonPlayingObjects.Planet);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
