using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework;
using Glib.XNA.SpriteLib;
using Glib;
using PGCGame.Screens;

namespace PGCGame.Ships.Network
{
    public class SoloNetworkShip : NetworkShip
    {
        public static SoloNetworkShip CreateFromData(Vector4 shipData, LocalNetworkGamer you)
        {
            SoloNetworkShip returnVal = new SoloNetworkShip(StateManager.NetworkData.SelectedNetworkShip.Type, StateManager.NetworkData.SelectedNetworkShip.Tier, GameScreen.World, you);
            returnVal.Position = new Vector2(shipData.X, shipData.Y);
            returnVal.Rotation = SpriteRotation.FromRadians(shipData.Z);
            returnVal.CurrentHealth = shipData.W.ToInt();
            returnVal.PlayerType = PlayerType.Solo;

            return returnVal;
        }

        public SoloNetworkShip(ShipType type, ShipTier tier, SpriteBatch world, NetworkGamer control) : base(type, tier, world, control)
        {
            PlayerType = CoreTypes.PlayerType.Solo;
        }
    }
}
