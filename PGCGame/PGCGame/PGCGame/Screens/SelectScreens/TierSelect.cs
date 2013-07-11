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
    class TierSelect  : BaseSelectScreen
    {
        public TierSelect(SpriteBatch spriteBatch)
            : base(spriteBatch)
        {
            
        }

        //This screen needs sprites for each tier of ship and descriptions for each tier.
        List<Ship> itemsShown = new List<Ship>();

        public override void InitScreen(ScreenType screenType)
          {
            Ship BattleCruiser;
            Ship FighterCarreer;
            Ship TorpedoShip;
            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;

            
            BattleCruiser = new BattleCruiser(GameContent.GameAssets.Images.Ships[ShipType.BattleCruiser, ShipTier.Tier2], Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text1 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This is the strongest class \n in the fleet, but also the slowest.\n What it lacks in speed it makes \n up for in strength.\n\n Damage Per Shot: 20\n Amount of Health: 120");
            text1.Color = Color.White;
            BattleCruiser.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            BattleCruiser.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(BattleCruiser, text1));
            itemsShown.Add(BattleCruiser);


            FighterCarreer = new FighterCarrier(GameContent.GameAssets.Images.Ships[ShipType.FighterCarrier, ShipTier.Tier2], Vector2.Zero, Sprites.SpriteBatch, GameContent.GameAssets.Images.Ships[ShipType.Drone, ShipTier.Tier1]);
            TextSprite text2 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This class fires an extremely fast\n Flak Cannon and has the ability to\n deploy drones. However, the drones\n and Flak Cannon aren't that powerful.\n After the Carrier gets destroyed, the\n drones die with it.\n\n Damage Per Shot: 2\n Amount of Health: 100\n Amount of Drones: 2\n Damage Per Drone Shot: 1\n Health Per Drone: 10");
            FighterCarreer.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.85f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .01f);
            FighterCarreer.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(FighterCarreer, text2));
            itemsShown.Add(FighterCarreer);

            TorpedoShip = new TorpedoShip(GameContent.GameAssets.Images.Ships[ShipType.TorpedoShip, ShipTier.Tier2], Vector2.Zero, Sprites.SpriteBatch);
            TextSprite text3 = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This class is the most balanced\n ship in the game. The torpedos do\n a lot of damage and \n are hard to dodge!\n\n Damege Per Shot: 5\n Amount of Health: 110");
            TorpedoShip.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            TorpedoShip.Rotation = new SpriteRotation(90);

            items.Add(new KeyValuePair<Sprite, TextSprite>(TorpedoShip, text3));
            itemsShown.Add(TorpedoShip);

            ChangeItem += new EventHandler(TierSelect_ChangeItem);
            nextButtonClicked += new EventHandler(TierSelect_nextButtonClicked);

            base.InitScreen(screenType);

            acceptLabel.Text = "Buy";
        }

        void TierSelect_nextButtonClicked(object sender, EventArgs e)
        {
            foreach (Ship ship in itemsShown)
            {
                if (ship.Texture == items[selected].Key.Texture)
                {
                    
                    
                    StateManager.SpaceBucks -= 100000;
                   
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
