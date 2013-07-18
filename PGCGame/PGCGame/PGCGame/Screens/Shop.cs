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

        Boolean firstShop = true;

        public override void InitScreen(ScreenType screenName)
        {
            base.InitScreen(screenName);

            StateManager.Options.ScreenResolutionChanged += new EventHandler(Options_ScreenResolutionChanged);

            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;
            BackgroundSprite = HorizontalMenuBGSprite.CurrentBG;

            Sprite upgradeEquipmentButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .1f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            upgradeEquipmentLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Equipment");
            upgradeEquipmentLabel.Position = new Vector2((upgradeEquipmentButton.X + upgradeEquipmentButton.Width / 2) - upgradeEquipmentLabel.Width / 2, (upgradeEquipmentButton.Y + upgradeEquipmentButton.Height / 2) - upgradeEquipmentLabel.Height / 2);
            upgradeEquipmentLabel.Color = Color.White;
            upgradeEquipmentLabel.IsHoverable = true;
            upgradeEquipmentLabel.IsManuallySelectable = true;
            upgradeEquipmentLabel.NonHoverColor = Color.White;
            upgradeEquipmentLabel.HoverColor = Color.MediumAquamarine;

#if WINDOWS
            upgradeEquipmentButton.MouseEnter += new EventHandler(upgradeEButton_MouseEnter);
            upgradeEquipmentButton.MouseLeave += new EventHandler(upgradeEButton_MouseLeave);
#endif

            Sprites.Add(upgradeEquipmentButton);
            AdditionalSprites.Add(upgradeEquipmentLabel);

            Sprite shipButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .400f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            shipLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Ship");
            shipLabel.Position = new Vector2((shipButton.X + shipButton.Width / 2) - shipLabel.Width / 2, (shipButton.Y + shipButton.Height / 2) - shipLabel.Height / 2);
            shipLabel.Color = Color.White;
            shipLabel.IsHoverable = true;
            shipLabel.IsManuallySelectable = true;
            shipLabel.NonHoverColor = Color.White;
            shipLabel.HoverColor = Color.MediumAquamarine;

#if WINDOWS
            shipButton.MouseEnter += new EventHandler(shipButton_MouseEnter);
            shipButton.MouseLeave += new EventHandler(shipButton_MouseLeave);
#endif


            Sprites.Add(shipButton);
            AdditionalSprites.Add(shipLabel);

            Sprite weaponsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .7f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            weaponsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Weapons");
            weaponsLabel.Position = new Vector2((weaponsButton.X + weaponsButton.Width / 2) - weaponsLabel.Width / 2, (weaponsButton.Y + weaponsButton.Height / 2) - weaponsLabel.Height / 2);
            weaponsLabel.Color = Color.White;
            weaponsLabel.IsHoverable = true;
            weaponsLabel.IsManuallySelectable = true;
            weaponsLabel.NonHoverColor = Color.White;
            weaponsLabel.HoverColor = Color.MediumAquamarine;

#if WINDOWS
            weaponsButton.MouseEnter += new EventHandler(weaponsButton_MouseEnter);
            weaponsButton.MouseLeave += new EventHandler(weaponsButton_MouseLeave);
#endif

            Sprites.Add(weaponsButton);
            AdditionalSprites.Add(weaponsLabel);

            Sprite nextLevelButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .65f), Sprites.SpriteBatch);
            nextLevelLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Next Level");
            nextLevelLabel.Position = new Vector2((nextLevelButton.X + nextLevelButton.Width / 2) - nextLevelLabel.Width / 2, (nextLevelButton.Y + nextLevelButton.Height / 2) - nextLevelLabel.Height / 2);
            nextLevelLabel.Color = Color.White;
            nextLevelLabel.IsHoverable = true;
            nextLevelLabel.IsManuallySelectable = true;
            nextLevelLabel.NonHoverColor = Color.White;
            nextLevelLabel.HoverColor = Color.MediumAquamarine;

#if WINDOWS
            nextLevelButton.MouseEnter += new EventHandler(nextLevelButton_MouseEnter);
            nextLevelButton.MouseLeave += new EventHandler(nextLevelButton_MouseLeave);
#endif

            Sprites.Add(nextLevelButton);
            AdditionalSprites.Add(nextLevelLabel);
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            //relocate all the sprites and labels to the correct position
        }

#if WINDOWS
        bool mouseInNextLevelButton = false;

        //backbutton
        void nextLevelButton_MouseLeave(object sender, EventArgs e)
        {
            nextLevelLabel.IsSelected = false;
            mouseInNextLevelButton = false;
        }
        void nextLevelButton_MouseEnter(object sender, EventArgs e)
        {
            nextLevelLabel.IsSelected = true;
            mouseInNextLevelButton = true;
        }
#endif

        //weaponsbutton
        void weaponsButton_MouseLeave(object sender, EventArgs e)
        {
            weaponsLabel.IsSelected = false;
        }
        void weaponsButton_MouseEnter(object sender, EventArgs e)
        {
            weaponsLabel.IsSelected = true;
        }

        public bool mouseInWeaponButton
        {
            get
            {
                return weaponsLabel.IsSelected;
            }
        }

        public bool mouseInUpgradeButton
        {
            get
            {
                return upgradeEquipmentLabel.IsSelected;
            }
        }

        public bool mouseInShipButton
        {
            get
            {
                return shipLabel.IsSelected;
            }
        }

        //shipbutton
        void shipButton_MouseLeave(object sender, EventArgs e)
        {
            shipLabel.IsSelected = false;
        }
        void shipButton_MouseEnter(object sender, EventArgs e)
        {
            shipLabel.IsSelected = true;
        }

        //upgradeEbutton
        void upgradeEButton_MouseLeave(object sender, EventArgs e)
        {
            upgradeEquipmentLabel.IsSelected = false;
        }
        void upgradeEButton_MouseEnter(object sender, EventArgs e)
        {
            upgradeEquipmentLabel.IsSelected = true;
        }



        MouseState lastMs = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
#if WINDOWS
            MouseState currentMs = MouseManager.CurrentMouseState;
            if (lastMs.LeftButton == ButtonState.Released && currentMs.LeftButton == ButtonState.Pressed)
            {
                if (mouseInNextLevelButton && !firstShop)
                {
                    if (StateManager.level == GameLevel.Level1)
                    {
                        StateManager.level = GameLevel.Level2;
                    }
                    else if (StateManager.level == GameLevel.Level2)
                    {
                        StateManager.level = GameLevel.Level3;
                    }
                    else if (StateManager.level == GameLevel.Level3)
                    {
                        StateManager.level = GameLevel.Level4;
                    }
                    else
                    {
                        //TODO: Win Code;
                    }
                    StateManager.nextLevel = true;
                    StateManager.ScreenState = ScreenType.Game;
                }
                else if (mouseInNextLevelButton && firstShop)
                {
                    StateManager.level = GameLevel.Level1;
                    firstShop = false;
                    StateManager.ScreenState = ScreenType.Game;
                }
                if (mouseInWeaponButton)
                {
                    StateManager.ScreenState = ScreenType.WeaponSelect;
                }
                if (mouseInUpgradeButton)
                {
                    StateManager.ScreenState = ScreenType.UpgradeScreen;
                }
                if (mouseInShipButton)
                {
                    StateManager.ScreenState = ScreenType.TierSelect;
                }

            }
            lastMs = currentMs;
#endif
        }
    }
}
