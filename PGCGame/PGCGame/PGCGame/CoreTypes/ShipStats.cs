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

        public override bool Equals(object obj)
        {
            if (obj != null && obj is ShipStats)
            {
                ShipStats val = (ShipStats)obj;
                return val.Type == Type && val.Tier == Tier;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 23;
                hash = hash * 31 + this.Type.GetHashCode();
                hash = hash * 31 + this.Tier.GetHashCode();
                return hash;
            }
        }
    }
}
