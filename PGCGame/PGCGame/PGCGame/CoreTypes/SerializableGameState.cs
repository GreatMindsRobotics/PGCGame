using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame.CoreTypes
{
    public struct SerializableGameState
    {
        public static SerializableGameState Current
        {
            get
            {
                SerializableGameState ret = new SerializableGameState();
                ret.Cash = StateManager.SpaceBucks;
                ret.Ship = StateManager.ShipData;
                ret.HighestLevel = StateManager.HighestUnlockedLevel;
                ret.Upgrades = StateManager.Upgrades;
                return ret;
            }
        }

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
