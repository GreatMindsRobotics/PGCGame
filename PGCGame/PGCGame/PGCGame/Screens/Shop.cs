using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Glib.XNA.SpriteLib;
using Glib;
using Glib.XNA;

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
        public static event EventHandler levelBegin;
        Sprite upgradeEquipmentButton;
        TextSprite upgradeEquipmentLabel;

        Sprite shipButton;
        TextSprite shipLabel;

        Sprite weaponsButton;
        TextSprite weaponsLabel;
        TextSprite noShipLabel;

        Sprite PlayButton;
        TextSprite PlayLabel;

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


            upgradeEquipmentButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .1f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            upgradeEquipmentLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Equipment");
            upgradeEquipmentLabel.Color = Color.White;
            upgradeEquipmentLabel.IsHoverable = true;
            upgradeEquipmentLabel.ParentSprite = upgradeEquipmentButton;
            upgradeEquipmentLabel.NonHoverColor = Color.White;
            upgradeEquipmentLabel.HoverColor = Color.MediumAquamarine;

            Sprites.Add(upgradeEquipmentButton);
            AdditionalSprites.Add(upgradeEquipmentLabel);

            shipButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            shipLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Ship");
            shipLabel.Color = Color.White;
            shipLabel.IsHoverable = true;
            shipLabel.ParentSprite = shipButton;
            shipLabel.NonHoverColor = Color.White;
            shipLabel.HoverColor = Color.MediumAquamarine;


            Sprites.Add(shipButton);
            AdditionalSprites.Add(shipLabel);

            weaponsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .7f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            weaponsLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Weapons");
            weaponsLabel.ParentSprite = weaponsButton;
            weaponsLabel.Color = Color.White;
            weaponsLabel.IsHoverable = true;
            weaponsLabel.NonHoverColor = Color.White;
            weaponsLabel.HoverColor = Color.MediumAquamarine;

            Sprites.Add(weaponsButton);
            AdditionalSprites.Add(weaponsLabel);

            PlayButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .4f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .65f), Sprites.SpriteBatch);
            PlayLabel = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, SegoeUIMono, "Play");
            PlayLabel.Color = Color.White;
            PlayLabel.IsHoverable = true;
            PlayLabel.ParentSprite = PlayButton;
            PlayLabel.NonHoverColor = Color.White;
            PlayLabel.HoverColor = Color.MediumAquamarine;

            noShipLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(0, PlayLabel.Y), SegoeUIMono, "You must buy a ship");
            noShipLabel.Color = Color.White;
            noShipLabel.X = noShipLabel.GetCenterPosition(Graphics.Viewport).X;

            Sprites.Add(PlayButton);
            AdditionalSprites.Add(noShipLabel);
            AdditionalSprites.Add(PlayLabel);


            PlayLabel.Pressed += new EventHandler(nextLevelLabel_Pressed);
            weaponsLabel.Pressed += new EventHandler(weaponsLabel_Pressed);
            upgradeEquipmentLabel.Pressed += new EventHandler(upgradeEquipmentLabel_Pressed);
            shipLabel.Pressed += new EventHandler(shipLabel_Pressed);

#if XBOX
            AllButtons = new GamePadButtonEnumerator(new TextSprite[,] { 
            {upgradeEquipmentLabel, shipLabel, weaponsLabel},
            {null, PlayLabel, null}
            }, InputType.LeftJoystick);
            AllButtons.FireTextSpritePressed = true;
#endif
        }

#if XBOX
        GamePadButtonEnumerator AllButtons;
#endif

        void shipLabel_Pressed(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }
            StateManager.ScreenState = ScreenType.TierSelect;
        }

        void upgradeEquipmentLabel_Pressed(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }
            StateManager.ScreenState = ScreenType.UpgradeScreen;
        }

        void weaponsLabel_Pressed(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }
            StateManager.ScreenState = ScreenType.WeaponSelect;
        }

        void nextLevelLabel_Pressed(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }
            levelBegin(null, null);
            StateManager.InitializeSingleplayerGameScreen(StateManager.SelectedShip, StateManager.SelectedTier);
            StateManager.ScreenState = ScreenType.TransitionScreen;
        }

        void Options_ScreenResolutionChanged(object sender, EventArgs e)
        {
            //relocate all the sprites and labels to the correct position
            Sprites[0].Scale = new Vector2((float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Width / (float)Sprites[0].Texture.Width, (float)StateManager.GraphicsManager.GraphicsDevice.Viewport.Height / (float)Sprites[0].Texture.Height);
            shipButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f - (shipButton.Width / 2), Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .5f);
            upgradeEquipmentButton.Position = new Vector2(shipButton.X - (1.5f * shipButton.Width), Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .5f);      
            weaponsButton.Position = new Vector2(shipButton.X + (1.5f * shipButton.Width), Sprites.SpriteBatch.GraphicsDevice.Viewport.Height *.5f);
            PlayButton.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .5f - (PlayButton.Width / 2), Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .6f + (.5f * PlayButton.Height));
        }



        //MouseState lastMs = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            

#if XBOX
            AllButtons.Update(gameTime);
#endif

            PlayButton.Color = StateManager.SelectedTier == ShipTier.NoShip ? Color.Transparent : Color.White;
            PlayLabel.Visible = PlayButton.Color.A > 0;
            noShipLabel.Visible = !PlayLabel.Visible;
        }
    }
}
