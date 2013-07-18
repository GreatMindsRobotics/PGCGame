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

        List<KeyValuePair<Sprite, string>> itemsShown = new List<KeyValuePair<Sprite, string>>();

        TextSprite level1Label;
        TextSprite level2Label;
        TextSprite level3Label;
        TextSprite level4Label;
         public TextSprite Playlabel;

        public override void InitScreen(ScreenType screenType)
        {
        Texture2D planet1 = GameContent.GameAssets.Images.NonPlayingObjects.Planet;
        Texture2D planet2 = GameContent.GameAssets.Images.NonPlayingObjects.AltPlanet;
        Texture2D planet3 = GameContent.GameAssets.Images.NonPlayingObjects.Planet3;
        Texture2D planet4 = GameContent.GameAssets.Images.NonPlayingObjects.Planet4;
            

           
            //level1
            Sprite level1 = new Sprite(planet1, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.68f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.22f), Sprites.SpriteBatch);
            level1Label = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "");
            level1.Scale = new Vector2(0.7f);

            itemsShown.Add(new KeyValuePair<Sprite,string>(level1, "   Level 1"));
            items.Add(new KeyValuePair<Sprite, TextSprite>(level1, level1Label));
            
            //level2
            Sprite level2 = new Sprite(planet2, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.60f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), Sprites.SpriteBatch);
            level2Label = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "");
            level2.Scale = new Vector2(0.7f);

            itemsShown.Add(new KeyValuePair<Sprite,string>(level2, "   Level 2"));
            items.Add(new KeyValuePair<Sprite, TextSprite>(level2, level2Label));

            //level3
            Sprite level3 = new Sprite(planet3, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.58f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), Sprites.SpriteBatch);
            level3Label = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "");
            level3.Scale = new Vector2(0.6f);

          

            itemsShown.Add(new KeyValuePair<Sprite,string>(level3, "   Level 3"));
            items.Add(new KeyValuePair<Sprite, TextSprite>(level3, level3Label));

            //level4
            Sprite level4 = new Sprite(planet4, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.63f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * 0.1f), Sprites.SpriteBatch);
            level4Label = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "");
            level4.Scale = new Vector2(0.4f);
            itemsShown.Add(new KeyValuePair<Sprite, string>(level4, "   Level 4"));
            items.Add(new KeyValuePair<Sprite, TextSprite>(level4, level3Label));

            nextButtonClicked += new EventHandler(LevelSelect_nextButtonClicked);
            ChangeItem += new System.EventHandler(LevelSelect_ChangeItem);

            
            base.InitScreen(screenType);
            acceptLabel.Text = "Shop";

        }

        void LevelSelect_ChangeItem(object sender, System.EventArgs e)
        {
            foreach (KeyValuePair<Sprite, string> item in itemsShown)
            {
                if (item.Key == items[selected].Key)
                {
                    nameLabel.Text = item.Value;
                    break;
                }
            }
        }

        void LevelSelect_nextButtonClicked(object sender, EventArgs e)
        {
            StateManager.InitializeSingleplayerGameScreen(StateManager.SelectedShip, StateManager.SelectedTier);
            StateManager.ScreenState = ScreenType.Shop;         
        }

        

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
