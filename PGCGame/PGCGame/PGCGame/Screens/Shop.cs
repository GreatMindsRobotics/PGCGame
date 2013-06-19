using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using Glib.XNA;
using Glib;
using Microsoft.Xna.Framework.Input;

namespace PGCGame.Screens
{
    public class Shop : Screen
    {
        public Shop(SpriteBatch spriteBatch)
            : base(spriteBatch, Color.Black)
        {
            
        }

        TextSprite upgradeELabel;
        TextSprite shipLabel;
        TextSprite weaponsLabel;

        public void LoadContent(ContentManager content)
        {
            //TODO: LOAD CONTENT

            //use Sprites to load your sprites
            //EX: Sprites.Add(new Sprite(content.Load<Texture2D>("assetName"), new Vector2(0, 0), Sprites.SpriteBatch));
            //OR
            //EX: Sprites.AddNewSprite(new Vector(0, 0), content.Load<Texture2D("assetName"));
            Texture2D buttonImage = content.Load<Texture2D>("Images\\Controls\\Button");
            SpriteFont SegoeUIMono = content.Load<SpriteFont>("Fonts\\SegoeUIMono");

            Sprite upgradeEButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .1f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            upgradeELabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .117f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .425f), SegoeUIMono, "Upgrade Equipment");
            upgradeELabel.Scale = new Vector2(0.8f);
            upgradeELabel.Color = Color.White;
            upgradeELabel.IsHoverable = true;
            upgradeELabel.IsManuallySelectable = true;
            upgradeELabel.NonHoverColor = Color.White;
            upgradeELabel.HoverColor = Color.MediumAquamarine;
            upgradeEButton.MouseEnter += new EventHandler(upgradeEButton_MouseEnter);
            upgradeEButton.MouseLeave += new EventHandler(upgradeEButton_MouseLeave);

            Sprites.Add(upgradeEButton);
            AdditionalSprites.Add(upgradeELabel);

            Sprite shipButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .400f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            shipLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .48f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .42f), SegoeUIMono, "Ship");
            shipLabel.Color = Color.White;
            shipLabel.IsHoverable = true;
            shipLabel.IsManuallySelectable = true;
            shipLabel.NonHoverColor = Color.White;
            shipLabel.HoverColor = Color.MediumAquamarine;
            shipButton.MouseEnter += new EventHandler(shipButton_MouseEnter);
            shipButton.MouseLeave += new EventHandler(shipButton_MouseLeave);

            Sprites.Add(shipButton);
            AdditionalSprites.Add(shipLabel);

            Sprite weaponsButton = new Sprite(buttonImage, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .700f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .4f), Sprites.SpriteBatch);
            weaponsLabel = new TextSprite(Sprites.SpriteBatch, new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * .76f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .42f), SegoeUIMono, "Weapons");
            weaponsLabel.Color = Color.White;
            weaponsLabel.IsHoverable = true;
            weaponsLabel.IsManuallySelectable = true;
            weaponsLabel.NonHoverColor = Color.White;
            weaponsLabel.HoverColor = Color.MediumAquamarine;
            weaponsButton.MouseEnter += new EventHandler(weaponsButton_MouseEnter);
            weaponsButton.MouseLeave += new EventHandler(weaponsButton_MouseLeave);

            Sprites.Add(weaponsButton);
            AdditionalSprites.Add(weaponsLabel);
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
            upgradeELabel.IsSelected = false;
        }
        void upgradeEButton_MouseEnter(object sender, EventArgs e)
        {
            upgradeELabel.IsSelected = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
