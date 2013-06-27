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
        UpgradeScreen,
        TierSelect
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

    /// <summary>
    /// States for the Drone AI FSM
    /// </summary>
    public enum DroneState
    { 
        Stowed,
        Stowing,
        Deploying,
        Deployed,
        TargetAcquired,
        MovingToTarget,
        Attacking,
        EvadingFire,
        AcceptingFate,
        RIP
    }

    public enum PlayerType
    { 
        Ally,
        Enemy,
        Solo
    }

    public enum XmlDataFile
    { 
        Credits,
        SecondaryWeapons,
        ShipDescriptions
    }

    public enum SpaceMineState
    { 
        Stowed,
        Deploying,
        Deployed,
        Armed,
        RIP
    }

}
