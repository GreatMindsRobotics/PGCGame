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

        private struct ShipInfo
        {
            public Sprite Image;
            public ShipType Type;
            public ShipTier Tier;
            public string Name;
            public TextSprite Description;
            public int Cost;
        }

        string nocredits = "You Have no more credits";
        TextSprite Credits;
        //This screen needs sprites for each tier of ship and descriptions for each tier.
        List<ShipInfo> upgradeShips;

        public override void InitScreen(ScreenType screenType)
        {
            upgradeShips = new List<ShipInfo>();

            if (!_firstTimeInit)
            {
                items.Clear();
                Sprites.Clear();
                AdditionalSprites.Clear();
            }

            Texture2D buttonImage = GameContent.GameAssets.Images.Controls.Button;
            SpriteFont SegoeUIMono = GameContent.GameAssets.Fonts.NormalText;

            ShipTier upgradeTier = StateManager.SelectedTier == ShipTier.Tier4 ? ShipTier.Tier4 : StateManager.SelectedTier + 1;

            //Configure current credit balance display
            Credits = new TextSprite(Sprites.SpriteBatch, SegoeUIMono, String.Format("You Have {0} Credits", StateManager.SpaceBucks));
            Credits.Position = new Vector2(5, 5);
            Credits.Color = Color.White;


            //Configure Battle Cruiser
            ShipInfo battleCruiser = new ShipInfo();
            battleCruiser.Image = new Sprite(GameContent.GameAssets.Images.Ships[ShipType.BattleCruiser, upgradeTier], Vector2.Zero, Sprites.SpriteBatch);
            battleCruiser.Image.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            battleCruiser.Image.Rotation = new SpriteRotation(90);

            battleCruiser.Name = BattleCruiser.ShipFriendlyName;
            battleCruiser.Cost = BattleCruiser.Cost[upgradeTier];

            battleCruiser.Type = ShipType.BattleCruiser;
            battleCruiser.Tier = upgradeTier;

            battleCruiser.Description = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This is the strongest class \n in the fleet, but also the slowest.\n What it lacks in speed it makes \n up for in strength.\n\n Damage Per Shot: 20\n Amount of Health: 120");
            battleCruiser.Description.Color = Color.White;

            items.Add(new KeyValuePair<Sprite, TextSprite>(battleCruiser.Image, battleCruiser.Description));            
            
            upgradeShips.Add(battleCruiser);


            //Configure Fighter Carrier
            ShipInfo fighterCarrier = new ShipInfo();
            fighterCarrier.Image = new Sprite(GameContent.GameAssets.Images.Ships[ShipType.FighterCarrier, upgradeTier], Vector2.Zero, Sprites.SpriteBatch);
            fighterCarrier.Image.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.85f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .01f);
            fighterCarrier.Image.Rotation = new SpriteRotation(90);

            fighterCarrier.Name = FighterCarrier.ShipFriendlyName;
            fighterCarrier.Cost = FighterCarrier.Cost[upgradeTier];
            
            fighterCarrier.Type = ShipType.FighterCarrier;
            fighterCarrier.Tier = upgradeTier;

            fighterCarrier.Description = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This class fires an extremely fast\n Flak Cannon and has the ability to\n deploy drones. However, the drones\n and Flak Cannon aren't that powerful.\n After the Carrier gets destroyed, the\n drones die with it.\n\n Damage Per Shot: 2\n Amount of Health: 100\n Amount of Drones: 2\n Damage Per Drone Shot: 1\n Health Per Drone: 10");
            fighterCarrier.Description.Color = Color.White;

            items.Add(new KeyValuePair<Sprite, TextSprite>(fighterCarrier.Image, fighterCarrier.Description));
            
            upgradeShips.Add(fighterCarrier);


            //Configure Torpedo Ship
            ShipInfo torpedoShip = new ShipInfo();
            torpedoShip.Image = new Sprite(GameContent.GameAssets.Images.Ships[ShipType.TorpedoShip, upgradeTier], Vector2.Zero, Sprites.SpriteBatch);
            torpedoShip.Image.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            torpedoShip.Image.Rotation = new SpriteRotation(90);

            torpedoShip.Name = TorpedoShip.ShipFriendlyName;
            torpedoShip.Cost = TorpedoShip.Cost[upgradeTier];

            torpedoShip.Type = ShipType.TorpedoShip;
            torpedoShip.Tier = upgradeTier;
            torpedoShip.Description = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.GameAssets.Fonts.NormalText, "\n\n This class is the most balanced\n ship in the game. The torpedos do\n a lot of damage and \n are hard to dodge!\n\n Damege Per Shot: 5\n Amount of Health: 110");
            torpedoShip.Description.Color = Color.White;

            items.Add(new KeyValuePair<Sprite, TextSprite>(torpedoShip.Image, torpedoShip.Description));
            
            upgradeShips.Add(torpedoShip);

            //Add event handlers
            if (_firstTimeInit)
            {
                ChangeItem += new EventHandler(TierSelect_ChangeItem);
                nextButtonClicked += new EventHandler(TierSelect_nextButtonClicked);
            }

            AdditionalSprites.Add(Credits);
            base.InitScreen(screenType);

            acceptLabel.Text = "Buy";
        }

        void TierSelect_nextButtonClicked(object sender, EventArgs e)
        {
            bool bought = false;

            foreach (ShipInfo shipInfo in upgradeShips)
            {
                if (!bought)
                {
                    if (StateManager.SpaceBucks - shipInfo.Cost < 0)
                    {
                        Credits.Text = nocredits;
                    }

                    if (shipInfo.Image.Texture == items[selected].Key.Texture && shipInfo.Cost <= StateManager.SpaceBucks)
                    {
                        StateManager.SpaceBucks -= shipInfo.Cost;
                        Credits.Text = String.Format("You Have {0} Credits", StateManager.SpaceBucks);

                        //Save purchased ship info
                        StateManager.SelectedShip = shipInfo.Type;
                        StateManager.SelectedTier = shipInfo.Tier;

                        StateManager.InitializeSingleplayerGameScreen(shipInfo.Type, shipInfo.Tier);

                        bought = true;
                    }
                }


            }
        }

        void TierSelect_ChangeItem(object sender, EventArgs e)
        {
            foreach (ShipInfo shipInfo in upgradeShips)
            {
                if (shipInfo.Image.Texture == items[selected].Key.Texture)
                {
                    nameLabel.Text = string.Format("{0}\n{1}\n{2}", shipInfo.Tier, shipInfo.Name, shipInfo.Cost);
                    break;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
