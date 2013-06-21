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
            TextSprite text1 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "TODO");
            ship1.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            ship1.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(ship1, text1));

            ship2 = new Sprite(content.Load<Texture2D>("Images\\Fighter Carrier\\Tier1"), Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text2 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "TODO");
            ship2.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.85f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .01f);
            ship2.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(ship2, text2));

            ship3 = new Sprite(content.Load<Texture2D>("Images\\Torpedo Ship\\Tier1"), Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text3 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, content.Load<SpriteFont>("Fonts\\SegoeUIMono"), "TODO");
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
