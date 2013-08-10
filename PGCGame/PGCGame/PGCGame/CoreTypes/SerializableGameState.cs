using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame.CoreTypes
{
    public struct SerializableGameState
    {
        public ShipStats Ship;
        public int Cash;
        public GameLevel HighestLevel;
        public UpgradesInfo Upgrades;

    }

    public struct UpgradesInfo
    {
        public bool HasScanner;
        public int SpaceMineCount;
        public int ShrinkRayCount;
        public int EMPCount;
        public int HealthPackCount;
    }
}
