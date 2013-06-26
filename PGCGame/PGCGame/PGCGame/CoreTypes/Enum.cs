using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame.CoreTypes
{
    public enum ScreenState
    {
        //TODO: FOR MICHAEL
        //ADD MORE SCREEN STATES SUCH AS OPTIONS, PAUSED, ETC
        Title,
        MainMenu,
        Credits,
        Game,
        Option,
        Shop,
        Pause,
        ShipSelect,
        WeaponSelect,
        UpgradeScreen
    }

    public enum ShipTier
    {
        Tier1,
        Tier2,
        Tier3,
        Tier4
    }

    //TODO: Are we using this? If not, remove...
    public enum ShipRelativePosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public enum ShipType
    {
        BattleCruiser,
        FighterCarrier,
        TorpedoShip,
        Drone
    }

    public enum PlayerType
    { 
        Ally,
        Enemy,
        Solo
    }
}
