using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Glib.XNA.SpriteLib;

using PGCGame.CoreTypes;
using Glib.XNA.InputLib;

namespace PGCGame.Screens
{
    public class Shop : BaseScreen
    {
        public Shop(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            //TODO: BACKGROUND
        }

        TextSprite upgradeEquipmentLabel;
        TextSprite shipLabel;
        TextSprite weaponsLabel;
        TextSprite nextLevelLabel;

        public static Boolean firstShop = true;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);
                        
            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);
            
            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;

            //Configure backgrounds
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;
            Sprites.AddNewSprite(Vector2.Zero, GameContent.GameAssets.Images.NonPlayingObjects.ShopBackground);
            Sprites[0].Scale = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width / (float)Sprites[0].Texture.Width, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / (float)Sprites[0].Texture.Height);


            Sprite upgradeEquipmentButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .1f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            upgradeEquipmentLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Equipment");
            upgradeEquipmentLabel.Color = Color.White;
            upgradeEquipmentLabel.IsHoverable = true;
            upgradeEquipmentLabel.ParentSprite = upgradeEquipmentButton;
            upgradeEquipmentLabel.NonHoverColor = Color.White;
            upgradeEquipmentLabel.HoverColor = Color.MediumAquamarine;

            Sprites.Add(upgradeEquipmentButton);
            AdditionalSprites.Add(upgradeEquipmentLabel);

            Sprite shipButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .400f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            shipLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Ship");
            shipLabel.Color = Color.White;
            shipLabel.IsHoverable = true;
            shipLabel.ParentSprite = shipButton;
            shipLabel.NonHoverColor = Color.White;
            shipLabel.HoverColor = Color.MediumAquamarine;


            Sprites.Add(shipButton);
            AdditionalSprites.Add(shipLabel);

            Sprite weaponsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .7f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            weaponsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Weapons");
            weaponsLabel.ParentSprite = weaponsButton;
            weaponsLabel.Color = Color.White;
            weaponsLabel.IsHoverable = true;
            weaponsLabel.NonHoverColor = Color.White;
            weaponsLabel.HoverColor = Color.MediumAquamarine;

            Sprites.Add(weaponsButton);
            AdditionalSprites.Add(weaponsLabel);

            Sprite nextLevelButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .65f), Sprites.SpriteBatch);
            nextLevelLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Next Level");
            nextLevelLabel.Color = Color.White;
            nextLevelLabel.IsHoverable = true;
            nextLevelLabel.ParentSprite = nextLevelButton;
            nextLevelLabel.NonHoverColor = Color.White;
            nextLevelLabel.HoverColor = Color.MediumAquamarine;

            Sprites.Add(nextLevelButton);
            AdditionalSprites.Add(nextLevelLabel);


            nextLevelLabel.Pressed += new EventHandler(nextLevelLabel_Pressed);
            weaponsLabel.Pressed += new EventHandler(weaponsLabel_Pressed);
            upgradeEquipmentLabel.Pressed += new EventHandler(upgradeEquipmentLabel_Pressed);
            shipLabel.Pressed += new EventHandler(shipLabel_Pressed);

#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { 
            {upgradeEquipmentLabel, shipLabel, weaponsLabel},
            {null, nextLevelLabel, null}
            }, InputType.LeftJoystick);
            AllButtons.FireTextSpritePressed = true;
#endif
        }

#if XBOX
        GamePadButtonEnumerator AllButtons;
#endif

        void shipLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.ScreenState = ScreenType.TierSelect;
        }

        void upgradeEquipmentLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.ScreenState = ScreenType.UpgradeScreen;
        }

        void weaponsLabel_Pressed(object sender, EventArgs e)
        {
            StateManager.ScreenState = ScreenType.WeaponSelect;
        }

        void nextLevelLabel_Pressed(object sender, EventArgs e)
        {
            if (!firstShop)
            {
                if (StateManager.level != GameLevel.Level4)
                {
                    StateManager.level++;
                }
                else
                {
                    //TODO: Win Code;
                }
                StateManager.nextLevel = true;
                StateManager.ScreenState = ScreenType.Game;
            }
            else
            {
                StateManager.level = GameLevel.Level1;
                firstShop = false;
                StateManager.ScreenState = ScreenType.Game;
            }
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            //relocate all the sprites and labels to the correct position
        }



        //MouseState lastMs = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
#if XBOX
            AllButtons.Update(gameTime);
#endif
        }
    }
}
