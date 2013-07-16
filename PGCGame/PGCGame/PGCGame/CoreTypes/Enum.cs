using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame.CoreTypes
{
    public enum ScreenType
    {
        Title,
        MainMenu,
        Credits,
        Game,
        Options,
        Shop,
        Pause,
        ShipSelect,
        WeaponSelect,
        UpgradeScreen,
        TierSelect,
        LevelSelect,
        ControlScreen,
        NetworkSelectScreen
    }

    public enum ScreenBackgrounds
    {
        GlobalScrollingBg,
        Title,
        MainMenu,
        Credits,
        Options,
        Shop,
        Pause,
        ShipSelect,
        WeaponSelect,
        UpgradeScreen,
        TierSelect,
        LevelSelect
    }

    public enum ScreenMusic
    {
        GlobalPlayingBgMusic,
        Title,
        MainMenu,
        Credits,
        Options,
        Shop,
        Pause,
        ShipSelect,
        WeaponSelect,
        UpgradeScreen,
        TierSelect,
        LevelSelect,
        Level1,
        Level2,
        Level3,
        Level4
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
        Drone,
        EnemyBattleCruiser,
        EnemyFighterCarrier,
        EnemyTorpedoShip,
        EnemyDrone
    }

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum GameLevel
    { 
        Level1,
        Level2,
        Level3,
        Level4
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
        Solo,
        MyShip  //Current player
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

    public enum EMPState
    {
        Stowed,
        Deployed,
        RIP
    }

    public enum SecondaryWeaponType
    { 
        EMP,
        ShrinkRay,
        SpaceMine
    }

    public enum TextureDisplayType
    { 
        ShopDisplay,
        InGameUse
    }

    public enum EquipmentType
    { 
        Scanner,
        HealthPack
    }
}
