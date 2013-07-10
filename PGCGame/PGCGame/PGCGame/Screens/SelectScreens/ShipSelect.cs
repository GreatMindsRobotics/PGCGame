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
using Glib.XNA.InputLib;

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

        List<KeyValuePair<Sprite, string>> itemsShown = new List<KeyValuePair<Sprite, string>>();

        public override void InitScreen(ScreenType screenType)
        {
            ship1 = new Sprite(GameContent.GameAssets.Images.Ships[ShipType.BattleCruiser, ShipTier.Tier1], Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text1 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This is the strongest class \n in the fleet, but also the slowest.\n What it lacks in speed it makes \n up for in strength.\n\n Damage Per Shot: 20\n Amount of Health: 120");
            text1.Color = Color.White;
            ship1.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            ship1.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(ship1, text1));            
            itemsShown.Add(new KeyValuePair<Sprite,string>(ship1, "Battle Cruiser"));


            ship2 = new Sprite(GameContent.GameAssets.Images.Ships[ShipType.FighterCarrier, ShipTier.Tier1], Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text2 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This class fires an extremely fast\n Flak Cannon and has the ability to\n deploy drones. However, the drones\n and Flak Cannon aren't that powerful.\n After the Carrier gets destroyed, the\n drones die with it.\n\n Damage Per Shot: 2\n Amount of Health: 100\n Amount of Drones: 2\n Damage Per Drone Shot: 1\n Health Per Drone: 10");
            ship2.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.85f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .01f);
            ship2.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(ship2, text2));
            itemsShown.Add(new KeyValuePair<Sprite, string>(ship2, "Fighter Carrier"));


            ship3 = new Sprite(GameContent.GameAssets.Images.Ships[ShipType.TorpedoShip, ShipTier.Tier1], Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text3 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This class is the most balanced\n ship in the game. The torpedos do\n a lot of damage and \n are hard to dodge!\n\n Damage Per Shot: 5\n Amount of Health: 110");
            ship3.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            ship3.Rotation = new SpriteRotation(90);
            
            items.Add(new KeyValuePair<Sprite, TextSprite>(ship3, text3));
            itemsShown.Add(new KeyValuePair<Sprite, string>(ship3, "Torpedo Ship"));

            
            nextButtonClicked += new EventHandler(ShipSelect_nextButtonClicked);
            ChangeItem += new EventHandler(ShipSelect_ChangeItem);

            base.InitScreen(screenType);
            acceptLabel.Text = "Next";
        }

        void ShipSelect_ChangeItem(object sender, EventArgs e)
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

        void ShipSelect_nextButtonClicked(object sender, EventArgs e)
        {
            if (items[selected].Key.Texture == ship1.Texture)
            {
                StateManager.SelectedShip = ShipType.BattleCruiser;
            }
            else if (items[selected].Key.Texture == ship2.Texture)
            {
                StateManager.SelectedShip = ShipType.FighterCarrier;
            }
            else if (items[selected].Key.Texture == ship3.Texture)
            {
                StateManager.SelectedShip = ShipType.TorpedoShip;
            }
            //TODO: Tiers
            StateManager.SelectedTier = ShipTier.Tier1;

            StateManager.ScreenState = ScreenType.LevelSelect;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
