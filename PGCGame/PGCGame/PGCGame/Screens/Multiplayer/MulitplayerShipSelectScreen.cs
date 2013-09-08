using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Glib;
using Glib.XNA;
using Glib.XNA.SpriteLib;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace PGCGame.Screens.Multiplayer
{
    public class MulitplayerShipSelectScreen : BaseSelectScreen
    {
        public MulitplayerShipSelectScreen(SpriteBatch spriteBatch)
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
        }

        List<ShipInfo> MultiplayerShips = new List<ShipInfo>();
        ShipInfo fighterCarrier;
        ShipInfo torpedoShip;
        ShipInfo battleCruiser;

        public override void InitScreen(ScreenType screenType)
        {
            if (_firstTimeInit)
            {
                ChangeItem += new EventHandler(MulitplayerShipSelectScreen_ChangeItem);
                nextButtonClicked += new EventHandler(MulitplayerShipSelectScreen_nextButtonClicked);
            }

            

            fighterCarrier.Image = new Sprite(GameContent.Assets.Images.Ships[ShipType.FighterCarrier, ShipTier.Tier2], Vector2.Zero, Sprites.SpriteBatch);
            fighterCarrier.Image.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            fighterCarrier.Image.Rotation = new SpriteRotation(90);
            fighterCarrier.Description = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.Assets.Fonts.NormalText, "\n\n This class fires an extremely fast\n Flak Cannon. However, the Flak\n Cannon isn't that powerful.\n\n Damage Per Shot: 2\n Amount of Health: 100\n Amount of Drones: 2\n Damage Per Drone Shot: 1\n Health Per Drone: 10");
            fighterCarrier.Description.Color = Color.White;
            fighterCarrier.Name = FighterCarrier.ShipFriendlyName;
            fighterCarrier.Type = ShipType.FighterCarrier;
            fighterCarrier.Tier = ShipTier.Tier2;

            items.Add(new KeyValuePair<Sprite, TextSprite>(fighterCarrier.Image, fighterCarrier.Description));

            MultiplayerShips.Add(fighterCarrier);

            battleCruiser.Image = new Sprite(GameContent.Assets.Images.Ships[ShipType.BattleCruiser, ShipTier.Tier2], Vector2.Zero, Sprites.SpriteBatch);
            battleCruiser.Image.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            battleCruiser.Image.Rotation = new SpriteRotation(90);
            battleCruiser.Name = BattleCruiser.ShipFriendlyName;
            battleCruiser.Type = ShipType.BattleCruiser;
            battleCruiser.Tier = ShipTier.Tier2;

            battleCruiser.Description = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.Assets.Fonts.NormalText, "\n\n This is the strongest class \n in the fleet, but also the slowest.\n What it lacks in speed it makes \n up for in strength.\n\n Damage Per Shot: 20\n Amount of Health: 120");
            battleCruiser.Description.Color = Color.White;

            items.Add(new KeyValuePair<Sprite, TextSprite>(battleCruiser.Image, battleCruiser.Description));

            MultiplayerShips.Add(battleCruiser);

            torpedoShip.Image = new Sprite(GameContent.Assets.Images.Ships[ShipType.TorpedoShip, ShipTier.Tier2], Vector2.Zero, Sprites.SpriteBatch);
            torpedoShip.Image.Position = new Vector2(Sprites.SpriteBatch.GraphicsDevice.Viewport.Width * 0.81f, Sprites.SpriteBatch.GraphicsDevice.Viewport.Height * .12f);
            torpedoShip.Image.Rotation = new SpriteRotation(90);
            torpedoShip.Name = TorpedoShip.ShipFriendlyName;
            torpedoShip.Type = ShipType.TorpedoShip;
            torpedoShip.Description = new TextSprite(Sprites.SpriteBatch, Vector2.Zero, GameContent.Assets.Fonts.NormalText, "\n\n This class is the most balanced\n ship in the game. The torpedos do\n a lot of damage and \n are hard to dodge!\n\n Damege Per Shot: 5\n Amount of Health: 110");
            torpedoShip.Description.Color = Color.White;
            torpedoShip.Tier = ShipTier.Tier2;

            items.Add(new KeyValuePair<Sprite, TextSprite>(torpedoShip.Image, torpedoShip.Description));

            MultiplayerShips.Add(torpedoShip);


            base.InitScreen(screenType);
        }

        void MulitplayerShipSelectScreen_nextButtonClicked(object sender, EventArgs e)
        {
            StateManager.GetScreen<MPShipsScreen>(CoreTypes.ScreenType.MPShipsDisplay).InitScreen(CoreTypes.ScreenType.MPShipsDisplay);
            StateManager.NetworkData.SelectedNetworkShip = new ShipStats() { Type = MultiplayerShips[selected].Type, Tier = MultiplayerShips[selected].Tier };
            StateManager.NetworkData.DataWriter.Write(StateManager.NetworkData.SelectedNetworkShip.Type.ToString());
            foreach (LocalNetworkGamer g in StateManager.NetworkData.CurrentSession.LocalGamers)
            {
                g.SendData(StateManager.NetworkData.DataWriter, SendDataOptions.Reliable);
            }
            StateManager.ScreenState = CoreTypes.ScreenType.MPShipsDisplay;
        }

        void MulitplayerShipSelectScreen_ChangeItem(object sender, EventArgs e)
        {
            nameLabel.Text = MultiplayerShips[selected].Name;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
