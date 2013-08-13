using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;

namespace PGCGame.Ships.Network
{
    public class SoloNetworkShip : NetworkShip
    {
        public SoloNetworkShip(ShipType type, ShipTier tier, SpriteBatch world) : base(type, tier, world)
        {
            PlayerType = CoreTypes.PlayerType.Solo;
        }
    }
}
