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
            Sprite Button;
            Texture2D ButtonImage = GameContent.GameAssets.Images.Controls.Button;
            Vector2 ButtonPosition = new Vector2(155);
            Color tintColor;
            Button = new Sprite(ButtonImage, ButtonPosition, Sprites.SpriteBatch);
            AdditionalSprites.Add(Button);
            

            TextSprite Continue = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, string.Format("Continue"));
            Continue.Color = Color.Beige;
            Continue.ParentSprite = Button;
            AdditionalSprites.Add(Continue);
            Continue.Pressed += new EventHandler(Continue_Pressed);
            
            
            TextSprite winText = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, string.Format("{2} Completed!\nYou earned {1} Points\nYou Have {0} earned SpaceBucks ",StateManager.SpaceBucks, StateManager.SpacePoints, StateManager.CurrentLevel));
            winText.Color = Color.Beige;
            AdditionalSprites.Add(winText);

            base.InitScreen(screenName);

        }

        void Continue_Pressed(object sender, EventArgs e)
        {
            StateManager.ScreenState = CoreTypes.ScreenType.MainMenu;
        }

       
        public override void Update(GameTime game)
        {
            base.Update(game);
        }
    }
}
