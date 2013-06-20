using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame
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
        ShipSelect
    }

    public enum ShipTier
    {
        Tier1,
        Tier2,
        Tier3,
        Tier4
    }

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

    public enum ScreenResolution
    {
        Normal,
        FullScreen
    }

    public enum VolumeSettings
    {
        Off = 0,
        Max = 100
    }
}
