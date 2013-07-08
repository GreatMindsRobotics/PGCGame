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
        TextSprite backLabel;

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
            upgradeEquipmentButton.MouseEnter += new EventHandler(upgradeEButton_MouseEnter);
            upgradeEquipmentButton.MouseLeave += new EventHandler(upgradeEButton_MouseLeave);

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
            shipButton.MouseEnter += new EventHandler(shipButton_MouseEnter);
            shipButton.MouseLeave += new EventHandler(shipButton_MouseLeave);


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
            weaponsButton.MouseEnter += new EventHandler(weaponsButton_MouseEnter);
            weaponsButton.MouseLeave += new EventHandler(weaponsButton_MouseLeave);

            Sprites.Add(weaponsButton);
            AdditionalSprites.Add(weaponsLabel);

            Sprite backButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .65f), Sprites.SpriteBatch);
            backLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Back");
            backLabel.Position = new Vector2((backButton.X + backButton.Width / 2) - backLabel.Width / 2, (backButton.Y + backButton.Height / 2) - backLabel.Height / 2);
            backLabel.Color = Color.White;
            backLabel.IsHoverable = true;
            backLabel.IsManuallySelectable = true;
            backLabel.NonHoverColor = Color.White;
            backLabel.HoverColor = Color.MediumAquamarine;
            backButton.MouseEnter += new EventHandler(backButton_MouseEnter);
            backButton.MouseLeave += new EventHandler(backButton_MouseLeave);

            Sprites.Add(backButton);
            AdditionalSprites.Add(backLabel);
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            //relocate all the sprites and labels to the correct position
        }

        bool mouseInBackButton = false;

        //backbutton
        void backButton_MouseLeave(object sender, EventArgs e)
        {
            backLabel.IsSelected = false;
            mouseInBackButton = false;
        }
        void backButton_MouseEnter(object sender, EventArgs e)
        {
            backLabel.IsSelected = true;
            mouseInBackButton = true;
            
        }

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
            MouseState currentMs = MouseManager.CurrentMouseState;
            if (lastMs.LeftButton == ButtonState.Released && currentMs.LeftButton == ButtonState.Pressed)
            {
                if (mouseInBackButton)
                {
                    StateManager.GoBack();
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
        }
    }
}
