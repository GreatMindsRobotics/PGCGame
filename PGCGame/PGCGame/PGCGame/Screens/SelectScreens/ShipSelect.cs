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

namespace PGCGame.Screens
{
    public class ShipSelect : BaseSelectScreen
    {
        public ShipSelect(SpriteBatch spriteBatch)
            : base(spriteBatch)
        {
            
        }

        Sprite ship1;
        Sprite ship2;
        Sprite ship3;

        public override void LoadContent(ContentManager content)
        {
            ship1 = new Sprite(content.Load<Texture2D>("Images\\Battle Cruiser\\Tier1"), Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text1 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), " Battle Cruiser\n\n This is the srtrongest class \n in the fleet, but also the slowest.\n What it lacks in speed it makes \n up for in strength.\n\n Damage Per Shot: 20\n Amount of Health: 120");
            text1.Color = Color.White;
            ship1.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            ship1.Rotation = new SpriteRotation(90);


            items.Add(new KeyValuePair<Sprite, TextSprite>(ship1, text1));
           

            ship2 = new Sprite(content.Load<Texture2D>("Images\\Fighter Carrier\\Tier1"), Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text2 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), " Fighter Carrier\n\n This class fires an extremely fast\n Flak Cannon and has the ability to\n deploy drones. However, the drones\n and Flak Cannon aren't that powerful.\n After the Carrier gets destroyed, the\n drones die with it.\n\n Damage Per Shot: 2\n Amount of Health: 100\n Amount of Drones: 2\n Damage Per Drone Shot: 1\n Health Per Drone: 10");
            ship2.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.85f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .01f);
            ship2.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(ship2, text2));

            ship3 = new Sprite(content.Load<Texture2D>("Images\\Torpedo Ship\\Tier1"), Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text3 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), " Torpedo Ship\n\n This class is the most balanced\n ship in the game The torpedos do\n a lot of damage and the torpedos\n are hard to dodge!\n\n Damege Per Shot: 5\n Amount of Health: 110");
            ship3.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            ship3.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(ship3, text3));

            base.LoadContent(content);
            nextButtonClicked += new EventHandler(ShipSelect_nextButtonClicked);
        }

        void ShipSelect_nextButtonClicked(object sender, EventArgs e)
        {
            //TODO: Ship selection screen will choose ship
            if (items[selected].Key.Texture == ship1.Texture)
            {
                StateManager.InitializeSingleplayerGameScreen<BattleCruiser>(ShipTier.Tier1);
            }
            else if (items[selected].Key.Texture == ship2.Texture)
            {
                StateManager.InitializeSingleplayerGameScreen<FighterCarrier>(ShipTier.Tier1);
            }
            else if (items[selected].Key.Texture == ship3.Texture)
            {
                StateManager.InitializeSingleplayerGameScreen<TorpedoShip>(ShipTier.Tier1);
            }

            
            StateManager.ScreenState = ScreenState.Game;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
