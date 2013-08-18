using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame.CoreTypes
{
    public struct ShipStats
    {
        public ShipStats(ShipType type, ShipTier tier)
        {
            Type = type;
            Tier = tier;
        }

        public ShipStats(ShipType type) : this(type, ShipTier.Tier1)
        {
        }

        public ShipType Type;
        public ShipTier Tier;
    }
}
