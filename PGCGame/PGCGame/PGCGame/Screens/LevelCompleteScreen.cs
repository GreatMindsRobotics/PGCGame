using System;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PGCGame.CoreTypes;

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
            Button = new Sprite(ButtonImage, ButtonPosition, Sprites.SpriteBatch);
            AdditionalSprites.Add(Button);
            StateManager.levelCompleted += new EventHandler(StateManager_levelCompleted);


            
           
            TextSprite Continue = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, string.Format("Continue"));
            Continue.Color = Color.Beige;
            Continue.ParentSprite = Button;
            AdditionalSprites.Add(Continue);
            Continue.Pressed +=new EventHandler(Continue_Pressed);
            Continue.IsHoverable = true;
            Continue.NonHoverColor = Color.White;
            Continue.HoverColor = Color.MediumAquamarine;
            Button.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .40f, 300);


            winText = new TextSprite(Sprites.SpriteBatch, GameContent.GameAssets.Fonts.NormalText, "");
            winText.Color = Color.Beige;
            AdditionalSprites.Add(winText);
             winText.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .30f, 50);
           
            base.InitScreen(screenName);

        }

        void StateManager_levelCompleted(object sender, EventArgs e)
        {
            winText.Text = string.Format("You died {3} times \n\n {2} Completed!\n\nYou earned {1} Points\n\nYou have obtained {0} spacebucks   ", StateManager.AmountOfSpaceBucksRecievedInCurrentLevel, StateManager.AmountOfPointsRecievedInCurrentLevel, StateManager.CurrentLevel, StateManager.Deaths);
        }

        void Continue_Pressed(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                StateManager.SpacePoints += StateManager.AmountOfPointsRecievedInCurrentLevel;
                StateManager.SpaceBucks += StateManager.AmountOfSpaceBucksRecievedInCurrentLevel;
                StateManager.AmountOfPointsRecievedInCurrentLevel = 0;
                StateManager.AmountOfSpaceBucksRecievedInCurrentLevel = 0;
                StateManager.Deaths = 0;
                StateManager.ScreenState = CoreTypes.ScreenType.LevelSelect;
            }
        }

       
        public override void Update(GameTime game)
        {
            
            base.Update(game);
        }
    }
}
