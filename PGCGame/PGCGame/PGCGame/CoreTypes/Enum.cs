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
        WeaponSelect,
        UpgradeScreen,
        TierSelect,
        LevelSelect,
        ControlScreen,
        NetworkSelectScreen,
        GameOver,
        TransitionScreen,
        LevelCompleteScreen,
        LoadingScreen,
        NetworkSessionsScreen,
        NetworkMatchSelection,
        MultiPlayerShipSelect,
        MPShipsDisplay,
        MPWinningScreen
    }

    /// <summary>
    /// The identifier for a network session property.
    /// </summary>
    /// <remarks>
    /// You can have up to 8 (Confirm on XNA Docs).
    /// </remarks>
    public enum NetworkSessionPropertyType
    {
        SessionType
    }

    /// <summary>
    /// Represents the type of a multiplayer session.
    /// </summary>
    public enum MultiplayerSessionType
    {
        Coop = 0,
        LMS = 1
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

    public enum SoundEffectType
    {
        DeployEMP,
        DeploySpaceMine,
        ExplodeSpaceMine,
        DeployHealthPack,
        DeployShrinkRay,
        SpaceDoorOpening,
        SpaceShipLeaving,
        BattleCruiserFire,
        FighterCarrierFire,
        TorpedoShipFire,
        EnemyExplodes,
        BoughtItem,
        EnemyShoots,
        ButtonPressed,
        DronesDeploy,
        DronesShoot,
        CloneMade
    }
    public enum ShipTier
    {
        NoShip = 0,
        Tier1,
        Tier2,
        Tier3,
        Tier4
    }

    public enum ShipType
    {
        NoShip,
        BattleCruiser,
        FighterCarrier,
        TorpedoShip,
        Drone,
        EnemyBattleCruiser,
        EnemyFighterCarrier,
        EnemyTorpedoShip,
        EnemyDrone,
        EnemyBoss,
        EnemyBossesClones,
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
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4
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
        SpaceMine = 0,
        ShrinkRay,
        EMP,
        HealthPack
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

    public enum SpriteSheetType
    {
        Explosion
    }

    public enum ShipState
    {
        Idle,
        Moving,
        Firing,
        Dead,
        Exploding,
        Alive
    }
}
