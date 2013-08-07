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
        TextSprite winText;

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
            Continue.Pressed +=new EventHandler(Continue_Pressed);
            Continue.IsHoverable = true;
            Continue.NonHoverColor = Color.White;
            Continue.HoverColor = Color.MediumAquamarine;
            Button.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .40f, 200);
           
            
            winText = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, string.Format("{2} Completed!\n\nYou earned {1} Points\n\nYou Have {0} earned SpaceBucks ",StateManager.AmountOfSpaceBucksRecievedInCurrentLevel, StateManager.AmountOfPointsRecievedInCurrentLevel, StateManager.CurrentLevel));
            winText.Color = Color.Beige;
            AdditionalSprites.Add(winText);
             winText.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .30f, 50);
           
            base.InitScreen(screenName);

        }

        void Continue_Pressed(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                StateManager.SpacePoints += StateManager.AmountOfPointsRecievedInCurrentLevel;
                StateManager.SpaceBucks += StateManager.AmountOfSpaceBucksRecievedInCurrentLevel;
                StateManager.AmountOfPointsRecievedInCurrentLevel = 0;
                StateManager.AmountOfSpaceBucksRecievedInCurrentLevel = 0;
                StateManager.ScreenState = CoreTypes.ScreenType.LevelSelect;
            }
        }

       
        public override void Update(GameTime game)
        {
            winText.Text = string.Format("{2} Completed!\n\nYou earned {1} Points\n\nYou Have {0} earned SpaceBucks ", StateManager.AmountOfSpaceBucksRecievedInCurrentLevel, StateManager.AmountOfPointsRecievedInCurrentLevel, StateManager.CurrentLevel);
            base.Update(game);
        }
    }
}
