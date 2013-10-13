using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib.XNA.SpriteLib;
using Glib.XNA;
using Glib;


namespace PGCGame.Screens.Multiplayer
{
    public class MultiplayerLoseScreen : BaseScreen
    {
        public override MusicBehaviour Music
        {
            get { return MusicBehaviour.NoMusic; }
        }

        public MultiplayerLoseScreen(SpriteBatch sb) : base(sb, Color.Black) { }

        TextSprite Lose;

        public override void InitScreen(ScreenType screenName)
        {
            Lose = new TextSprite(Sprites.SpriteBatch, GameContent.Assets.Fonts.BoldText, "You Lose =(", Color.White);

            base.InitScreen(screenName);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
