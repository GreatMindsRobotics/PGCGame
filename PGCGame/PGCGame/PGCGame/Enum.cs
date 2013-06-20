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
        Pause
    }

    public enum ShipTier
    {
        Tier1,
        Tier2,
        Tier3,
        Tier4
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
