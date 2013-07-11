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
    class TierSelect : BaseSelectScreen
    {
        public TierSelect(SpriteBatch spriteBatch)
            : base(spriteBatch)
        {

        }

        string nocredits = "You Have no more credits";
        TextSprite Credits;
        //This screen needs sprites for each tier of ship and descriptions for each tier.
        Dictionary<Sprite, int> itemsShown = new Dictionary<Sprite, int>();

        public override void InitScreen(ScreenType screenType)
        {
            Sprite battleCruiser;
            Sprite fighterCarrier;
            Sprite torpedoShip;
            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;




            Credits = new TextSprite(Sprites.SpriteBatch, SegoeUIMono, String.Format("You Have {0} Credits", StateManager.SpaceBucks));
            Credits.Position = new Vector2(5, 5);
            Credits.Color = Color.White;

            battleCruiser = new Sprite(GameContent.GameAssets.Images.Ships[ShipType.BattleCruiser, ShipTier.Tier2], Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text1 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This is the strongest class \n in the fleet, but also the slowest.\n What it lacks in speed it makes \n up for in strength.\n\n Damage Per Shot: 20\n Amount of Health: 120");
            text1.Color = Color.White;
            battleCruiser.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            battleCruiser.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(battleCruiser, text1));
            itemsShown.Add(battleCruiser, BattleCruiser.Cost[ShipTier.Tier2]);


            fighterCarrier = new Sprite(GameContent.GameAssets.Images.Ships[ShipType.FighterCarrier, ShipTier.Tier2], Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text2 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This class fires an extremely fast\n Flak Cannon and has the ability to\n deploy drones. However, the drones\n and Flak Cannon aren't that powerful.\n After the Carrier gets destroyed, the\n drones die with it.\n\n Damage Per Shot: 2\n Amount of Health: 100\n Amount of Drones: 2\n Damage Per Drone Shot: 1\n Health Per Drone: 10");
            fighterCarrier.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.85f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .01f);
            fighterCarrier.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(fighterCarrier, text2));
            itemsShown.Add(fighterCarrier, FighterCarrier.Cost[ShipTier.Tier2]);

            torpedoShip = new Sprite(GameContent.GameAssets.Images.Ships[ShipType.TorpedoShip, ShipTier.Tier2], Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text3 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This class is the most balanced\n ship in the game. The torpedos do\n a lot of damage and \n are hard to dodge!\n\n Damege Per Shot: 5\n Amount of Health: 110");
            torpedoShip.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            torpedoShip.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(torpedoShip, text3));
            itemsShown.Add(torpedoShip, TorpedoShip.Cost[ShipTier.Tier2]);

            ChangeItem += new EventHandler(TierSelect_ChangeItem);
            nextButtonClicked += new EventHandler(TierSelect_nextButtonClicked);



            AdditionalSprites.Add(Credits);
            base.InitScreen(screenType);


            acceptLabel.Text = "Buy";
        }

        void TierSelect_nextButtonClicked(object sender, EventArgs e)
        {
            bool bought = false;

            foreach (KeyValuePair<Sprite, int> kvp in itemsShown)
            {
                Sprite ship = kvp.Key;
                int cost = kvp.Value;

                if (bought == false)
                {
                    if (StateManager.SpaceBucks - cost < 0)
                    {
                        Credits.Text = nocredits;
                    }
                   
                    if (ship.Texture == items[selected].Key.Texture && cost <= StateManager.SpaceBucks)
                    {
                        StateManager.SpaceBucks -= cost;
                        Credits.Text = String.Format("You Have {0} Credits", StateManager.SpaceBucks);
                        bought = true;
                    }
                }


            }
        }

        void TierSelect_ChangeItem(object sender, EventArgs e)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
