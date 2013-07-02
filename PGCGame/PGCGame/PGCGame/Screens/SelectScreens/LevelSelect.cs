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
using PGCGame.CoreTypes;
namespace PGCGame.Screens.SelectScreens
{
    public class LevelSelect : BaseSelectScreen
    {
        public LevelSelect(SpriteBatch spriteBatch)
            : base(spriteBatch)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            nextButtonClicked += new EventHandler(LevelSelect_nextButtonClicked);
        }

        void LevelSelect_nextButtonClicked(object sender, EventArgs e)
        {
            StateManager.ScreenState = ScreenState.Game;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
