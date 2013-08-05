using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PGCGame.CoreTypes;
using Glib.XNA.SpriteLib;

namespace PGCGame
{
    public class LevelCompleteScreen : CoreTypes.BaseScreen
    {
        public LevelCompleteScreen(SpriteBatch spriteBatch)
        : base(spriteBatch, Color.White)
        {
            
        }

        public override void InitScreen(ScreenType screenName)
        {
            this.BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            
            TextSprite Continue = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, string.Format("Continue"));
            Continue.Color = Color.Beige;
            AdditionalSprites.Add(Continue);
            
            TextSprite winText = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, string.Format("{0} Completed!\nYou earned {1} Points\nYou Have {2} earned SpaceBucks ",StateManager.SpaceBucks, StateManager.SpacePoints, StateManager.CurrentLevel));
            winText.Color = Color.Beige;
            AdditionalSprites.Add(winText);

            base.InitScreen(screenName);

        }

        public override void Update(GameTime game)
        {
            base.Update(game);
        }
    }
}
