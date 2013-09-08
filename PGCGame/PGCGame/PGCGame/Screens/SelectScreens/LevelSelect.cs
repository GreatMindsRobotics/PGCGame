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
            ButtonClick = GameContent.Assets.Sound[SoundEffectType.ButtonPressed];
        }

        TextSprite level1Label;
        TextSprite level2Label;
        TextSprite level3Label;
        TextSprite level4Label;

        Color lockedColor = new Color(10, 10, 10, 255);

        bool canPlayLevel = false;
        

        GameLevel selectedLevel = GameLevel.Level1;


        public TextSprite Playlabel;

         Sprite level1;
         Sprite level2;
         Sprite level3;
         Sprite level4;

        public override void InitScreen(ScreenType screenType)
        {
            ChangeItem +=new EventHandler(LevelSelect_ChangeItem);
        Texture2D planet1 = GameContent.Assets.Images.NonPlayingObjects.Planet;
        Texture2D planet2 = GameContent.Assets.Images.NonPlayingObjects.AltPlanet;
        Texture2D planet3 = GameContent.Assets.Images.NonPlayingObjects.Planet3;
        Texture2D planet4 = GameContent.Assets.Images.NonPlayingObjects.Planet4;

            //level1
            level1 = new Sprite(planet1, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.68f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.22f), Sprites.SpriteBatch);
            level1Label = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.Assets.Fonts.NormalText, "  Level 1");
            level1.Scale = new Vector2(0.7f);
            level1Label.Color = Color.Transparent;
            level1.Color = Color.White;

           
            
            //level2
            level2 = new Sprite(planet2, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.60f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), Sprites.SpriteBatch);
            level2Label = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.Assets.Fonts.NormalText, "  Level 2");
            level2.Scale = new Vector2(0.7f);




            //level3
            level3 = new Sprite(planet3, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.58f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), Sprites.SpriteBatch);
            level3Label = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.Assets.Fonts.NormalText, "  Level 3");
            level3Label.Color = Color.Transparent;
            level3.Scale = new Vector2(0.6f);




            //level4
            level4 = new Sprite(planet4, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.63f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), Sprites.SpriteBatch);
            level4Label = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.Assets.Fonts.NormalText, "  Level 4");
            level4.Scale = new Vector2(0.4f);



            items.Add(new KeyValuePair<Sprite, TextSprite>(level1, level1Label));
            items.Add(new KeyValuePair<Sprite, TextSprite>(level2, level2Label));
            items.Add(new KeyValuePair<Sprite, TextSprite>(level3, level3Label));
            items.Add(new KeyValuePair<Sprite, TextSprite>(level4, level4Label));

            nextButtonClicked += new EventHandler(LevelSelect_nextButtonClicked);
            ChangeItem += new System.EventHandler(LevelSelect_ChangeItem);

            base.InitScreen(screenType);
            acceptLabel.Text = "Shop";

            level1Label.Position = nameLabel.Position;
            level2Label.Position = nameLabel.Position;
            level3Label.Position = nameLabel.Position;
            level4Label.Position = nameLabel.Position;
        }

        void LevelSelect_ChangeItem(object sender, System.EventArgs e)
        {
            int counter = 0;
            foreach (KeyValuePair<Sprite, TextSprite> item in items)
            {
                if (item.Key == items[selected].Key)
                {
                    TransitionScreen.planetTexture = item.Key.Texture;
                    nameLabel.Text = item.Value.Text;
                    selectedLevel = (GameLevel)(counter + 1);
                    break;
                }
                counter++;
            }

            if (selectedLevel > StateManager.HighestUnlockedLevel)
            {
                acceptLabel.Text = "Locked";
                canPlayLevel = false;
 
                switch (selectedLevel)
                {
                    case GameLevel.Level2:
                        {
                            if (selectedLevel > StateManager.HighestUnlockedLevel)
                            {
                                level2.Color = lockedColor;
                            }
                            break;
                        }

                    case GameLevel.Level3:
                        {
                            if (selectedLevel > StateManager.HighestUnlockedLevel)
                            {
                                level3.Color = lockedColor;
                            }

                            break;
                        }
                    case GameLevel.Level4:
                        {
                            if (selectedLevel > StateManager.HighestUnlockedLevel)
                            {
                                level4.Color = lockedColor;
                            }
                            break;
                        }
                }

                return;
            }
            else
            {
                acceptLabel.Text = "Shop";
                StateManager.CurrentLevel = selectedLevel;
                canPlayLevel = true;
            }

            StateManager.levelCompleted += new EventHandler(StateManager_levelCompleted);

            
        }

        void StateManager_levelCompleted(object sender, EventArgs e)
        {
            if (StateManager.CurrentLevel != GameLevel.Level4)
            {
                selected = StateManager.HighestUnlockedLevel.ToInt() - 1;
                for (int i = 0; i < StateManager.HighestUnlockedLevel.ToInt(); i++)
                {
                    LevelSelect_ChangeItem(null, null);
                }
            }

        }

        void LevelSelect_nextButtonClicked(object sender, EventArgs e)
        {
            //StateManager.InitializeSingleplayerGameScreen(StateManager.SelectedShip, StateManager.SelectedTier);
            if (!canPlayLevel)
            {
                return;
            }

            if (StateManager.Options.SFXEnabled)
            {
                ButtonClick.Play();
            }

            StateManager.ScreenState = ScreenType.Shop;
            StateManager.CurrentLevel = (GameLevel)(selected + 1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
