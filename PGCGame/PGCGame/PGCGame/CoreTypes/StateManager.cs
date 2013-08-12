using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Glib;
using Microsoft.Xna.Framework;

using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Glib.XNA.InputLib;
using Microsoft.Xna.Framework.Storage;

namespace PGCGame
{
    public static class StateManager
    {
        #region Private Fields

        private static bool _hasBoughtScanner = false;
        public static int HealthPacks = 0;
        private static Stack<ScreenType> _screenStack = new Stack<ScreenType>();
        private static ScreenType _screenState = ScreenType.Title;
        private static GraphicsDeviceManager _gfx;
        private static Guid _enemyID = Guid.NewGuid();
        private static int _spaceBucks = 200000;
        #endregion Private Fields

        #region Public Fields


        public static bool GamerServicesAreAvailable = true;

        public static StorageDevice SelectedStorage = null;

        public static BulletCollection EnemyBullets = new BulletCollection();
        public static BulletCollection AllyBullets = new BulletCollection();

        public static Boolean[,] KnownMap = new Boolean[9, 30];

        public static bool IsWSFirstUpdate = true;

        public static ShipType SelectedShip;
        public static ShipTier SelectedTier;

        public static int SpacePoints = 0;

        public static int AmountOfPointsRecievedInCurrentLevel = 0;

        public static int AmountOfSpaceBucksRecievedInCurrentLevel = 0;

        public static Stack<SecondaryWeapon>[] PowerUps = new Stack<SecondaryWeapon>[]{
            //0: SpaceMine
            new Stack<SecondaryWeapon>(),
            //1: ShrinkRay
            new Stack<SecondaryWeapon>(),
            //2: EMP
            new Stack<SecondaryWeapon>(),
            //3: HealthPack
            new Stack<SecondaryWeapon>()
        };

        public static Delegates.CheckIfWindowFocused IsWindowFocused;

        public static Delegates.QuitFunction Exit;

        /// <summary>
        /// Keeps track of active ships in the game. This info can be used for mini-map, collision detection, etc
        /// </summary>
        public static readonly ShipCollection EnemyShips = new ShipCollection();
        public static readonly ShipCollection AllyShips = new ShipCollection();

        /// <summary>
        /// Identifies the player in the network game
        /// </summary>
        public static readonly Guid PlayerID = Guid.NewGuid();

        public static int Lives = 5;
        public static int Deaths = 0;

        private static GameLevel _level = GameLevel.Level1;

        public static GameLevel HighestUnlockedLevel
        {
            get { return _level; }
            set {
                if (value != _level)
                {
                    _level = value;
                    SpaceBucks += AmountOfSpaceBucksRecievedInCurrentLevel;
                    SpacePoints += AmountOfPointsRecievedInCurrentLevel;
                    if (levelCompleted != null)
                    {
                        levelCompleted(null, EventArgs.Empty);


                    }
                }
            }
        }
        public static GameLevel CurrentLevel = GameLevel.Level1;
        
        public static Boolean nextLevel = false;
        public static event EventHandler levelCompleted;

        /// <summary>
        /// Resets fields
        /// </summary>
        public static void Reset()
        {
            Screens.Shop.firstShop = true;
            nextLevel = false;
            HighestUnlockedLevel = GameLevel.Level1;
            SpacePoints = 0;
            _hasBoughtScanner = false;
            Stack<ScreenType> _screenStack = new Stack<ScreenType>();
            Guid _enemyID = Guid.NewGuid();
            _spaceBucks = 10000;
            SpaceBucks = _spaceBucks;
            IsWSFirstUpdate = true;
            PowerUps = new Stack<SecondaryWeapon>[]{
            //0: SpaceMine
            new Stack<SecondaryWeapon>(),
            //1: ShrinkRay
            new Stack<SecondaryWeapon>(),
            //2: EMP
            new Stack<SecondaryWeapon>(),
            //3: HealthPack
            new Stack<SecondaryWeapon>()
        };
            Lives = 5;
        }

        /// <summary>
        /// Manages all screens in the game
        /// </summary>
        public static ScreenManager AllScreens;

        /// <summary>
        /// Keeps track of the current Viewport size 
        /// (used for switching between Normal mode and Full Screen mode)
        /// </summary>
        public static Point ViewportSize;

        /// <summary>
        /// An event called on screen switch.
        /// </summary>
        public static event EventHandler ScreenStateChanged;


        /// <summary>
        /// Random generator to use throughout the game; ensures no concurrent seeding of two Random objects
        /// </summary>
        public static readonly Random RandomGenerator = new Random();

        #endregion Public Fields

        #region Public Properties

        public static UpgradesInfo Upgrades
        {
            get
            {
                UpgradesInfo ret = new UpgradesInfo();
                ret.HasScanner = StateManager.HasBoughtScanner;
                ret.SpaceMineCount = StateManager.PowerUps[0].Count;
                ret.ShrinkRayCount = StateManager.PowerUps[1].Count;
                ret.EMPCount = StateManager.PowerUps[2].Count;
                ret.HealthPackCount = StateManager.PowerUps[3].Count;
                return ret;
            }
        }

        public static ShipStats ShipData
        {
            get
            {
                return new ShipStats() { Tier = SelectedTier, Type = SelectedShip };
            }
            set
            {
                SelectedTier = value.Tier;
                SelectedShip = value.Type;
            }
        }

        /// <summary>
        /// Gets or sets ScreenState for the game, indicating the currently visible screen.
        /// StateManager keeps track of active screens on the screenStack.
        /// </summary>
        public static ScreenType ScreenState
        {
            get
            {
                return _screenState;
            }
            set
            {
                if (value != ScreenType.TransitionScreen && value != ScreenType.LoadingScreen)
                {
                    _screenStack.Push(value);
                }
                _screenState = value;

                SwitchScreen(value);
            }
        }

        public static int SpaceBucks { get { return _spaceBucks; } set { _spaceBucks = value; } }

        public static Guid EnemyID
        {
            get
            {
                return _enemyID;
            }
            private set
            {
                _enemyID = value;
            }
        }

        /// <summary>
        /// Gets or sets the current GraphicsDeviceManager, and stores Viewport size in StateManager.ViewportSize
        /// </summary>
        public static GraphicsDeviceManager GraphicsManager
        {
            get { return _gfx; }

            set
            {
                _gfx = value;
                ViewportSize = new Point(_gfx.GraphicsDevice.Viewport.Width, _gfx.GraphicsDevice.Viewport.Height);
            }
        }

        /// <summary>
        /// Indicates whether or not the user has bought the "scanner" power up.
        /// </summary>
        public static bool HasBoughtScanner
        {
            get { return _hasBoughtScanner; }
            set { _hasBoughtScanner = value; }
        }


        public static Rectangle WorldSize { get; set; }

        #endregion Public Properties

        static StateManager()
        {
            //MediaPlayer.MediaStateChanged += new EventHandler<EventArgs>(MediaPlayer_MediaStateChanged);
        }

        #region Private Methods

        /*
        private static void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            //Game music handling
            AllScreens[ScreenType.Game.ToString()].Cast<PGCGame.Screens.GameScreen>().HandleMusicChange();
        }
        */

        /// <summary>
        /// Switches the active screen based on screenState parameter.
        /// Note: Currently only a single screen can be visible at any one time.
        /// </summary>
        /// <param name="screenType">Screen to switch to</param>
        private static void SwitchScreen(ScreenType screenType)
        {
            foreach (Screen screen in AllScreens)
            {
                screen.Visible = false;
            }

            //Find the right screen
            Screen activeScreen = AllScreens[screenType.ToString()];

            //Special handling
            switch (screenType)
            {
                case ScreenType.Game:
                    activeScreen.Cast<PGCGame.Screens.GameScreen>().ResetLastKS(Keys.Escape);
                    break;

                case ScreenType.GameOver:
                    activeScreen.Cast<PGCGame.Screens.GameOver>();
                    break;

                case ScreenType.Pause:
                    activeScreen.Cast<PGCGame.Screens.PauseScreen>().lastState = new KeyboardState(Keys.Escape);
                    break;

                case ScreenType.Credits:
                    if (StateManager.Options.MusicEnabled)
                    {
                        activeScreen.Cast<PGCGame.Screens.Credits>().PlayMusic();
                    }
                    break;
            }

            //Set selected screen visible
            activeScreen.Visible = true;
            if (ScreenStateChanged != null)
            {
                ScreenStateChanged(null, EventArgs.Empty);
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Initializes the game for single player based on selected ship type and tier
        /// </summary>
        /// <typeparam name="T">Ship type</typeparam>
        /// <param name="tier">Ship tier</param>
        public static void InitializeSingleplayerGameScreen<T>(ShipTier tier) where T : PGCGame.Ships.Allies.BaseAllyShip
        {
            //Save current tier
            SelectedTier = tier;
            AllScreens[ScreenType.Game.ToString()].Cast<Screens.GameScreen>().InitializeScreen<T>(tier);
        }

        public static void InitializeSingleplayerGameScreen(ShipType type, ShipTier tier)
        {
            switch (type)
            {
                case ShipType.BattleCruiser:
                    InitializeSingleplayerGameScreen<BattleCruiser>(tier);
                    return;
                case ShipType.FighterCarrier:
                    InitializeSingleplayerGameScreen<FighterCarrier>(tier);
                    return;
                case ShipType.TorpedoShip:
                    InitializeSingleplayerGameScreen<TorpedoShip>(tier);
                    return;
                default:
                    throw new NotImplementedException("Not a supported ship.");

            }
        }

        /// <summary>
        /// Returns to previous screen
        /// </summary>
        public static void GoBack()
        {
            _screenStack.Pop();
            if (_screenStack.Count == 0)
            {
                StateManager.Exit();
                return;
            }
            _screenState = _screenStack.Peek();
            SwitchScreen(_screenState);
        }

        #endregion Public Methods


        #region Public Classes


        public static class DebugData
        {
            public static bool InfiniteMoney = true;
            public static bool DebugBackground = false;
            public static bool ShipSpeedIncrease = true;
            public static bool OPBullets = false;
            public static bool ShowShipIDs = true;
            public static bool EmergencyHeal = true;
            public static bool KillAll = true;
            public static bool Invincible = true;
            public static bool BringDronesBack = false;
            public static bool KillYourSelf = true;
            /// <summary>
            /// Buggy - due to reference passing
            /// </summary>
            public static bool InfiniteSecondaryWeapons = false;

            public static bool FogOfWarEnabled = true;

            /// <summary>
            /// Multiply the game framerate - by default 60 FPS - by this amount.
            /// </summary>
            public static int OverclockAmount = 1;
        }

        public static class InputManager
        {
            public static bool ShouldMove(MoveDirection direction)
            {
#if WINDOWS
                Keys[] pressed = Keyboard.GetState().GetPressedKeys();
                switch (direction)
                {
                    case MoveDirection.Down:
                        return pressed.Contains(Options.ArrowKeysEnabled ? Keys.Down : Keys.S);
                    case MoveDirection.Left:
                        return pressed.Contains(Options.ArrowKeysEnabled ? Keys.Left : Keys.A);
                    case MoveDirection.Right:
                        return pressed.Contains(Options.ArrowKeysEnabled ? Keys.Right : Keys.D);
                    case MoveDirection.Up:
                        return pressed.Contains(Options.ArrowKeysEnabled ? Keys.Up : Keys.W);
                    default:
                        throw new NotImplementedException("The specified direction is not implemented.");
                }
#elif XBOX
                switch (direction)
                {
                    case MoveDirection.Down:
                        return GamePadManager.One.Current.ThumbSticks.Left.Y < 0;
                    case MoveDirection.Left:
                        return GamePadManager.One.Current.ThumbSticks.Left.X < 0;
                    case MoveDirection.Right:
                        return GamePadManager.One.Current.ThumbSticks.Left.X > 0;
                    case MoveDirection.Up:
                        return GamePadManager.One.Current.ThumbSticks.Left.Y > 0;
                    default:
                        throw new NotImplementedException("The specified direction is not implemented.");
                }
#endif
            }
        }

        public static class Options
        {
            private static bool _musicEnabled = true;

            public static event EventHandler ScreenResolutionChanged;

            private static bool _sfxEnabled = true;

            public static bool SFXEnabled
            {
                get { return _sfxEnabled; }
                set { _sfxEnabled = value; }
            }
            

            public static bool ArrowKeysEnabled { get; set; }
            public static bool LeftButtonEnabled { get; set; }
            public static bool SecondaryButtonEnabled { get; set; }
            public static bool SwitchButtonEnabled { get; set; }
            public static bool DeployDronesEnabled { get; set; }

            public static event EventHandler MusicStateChanged;

            public static bool MusicEnabled
            {
                get { return _musicEnabled; }
                set
                {
                    if (value != _musicEnabled)
                    {
                        _musicEnabled = value;
                        if (MusicStateChanged != null)
                        {
                            MusicStateChanged(null, EventArgs.Empty);
                        }
                    }
                }
            }

            public static void ToggleFullscreen()
            {
                StateManager.GraphicsManager.PreferredBackBufferWidth = StateManager.GraphicsManager.IsFullScreen ? StateManager.ViewportSize.X : StateManager.GraphicsManager.GraphicsDevice.DisplayMode.Width;
                StateManager.GraphicsManager.PreferredBackBufferHeight = StateManager.GraphicsManager.IsFullScreen ? StateManager.ViewportSize.Y : StateManager.GraphicsManager.GraphicsDevice.DisplayMode.Height;
                StateManager.GraphicsManager.ToggleFullScreen();

                //Loop through each screen and adjust display size
                //TODO: Scrolling backgrounds do not properly reset
                foreach (Screen s in StateManager.AllScreens)
                {
                    s.Target = new RenderTarget2D(StateManager.GraphicsManager.GraphicsDevice, StateManager.GraphicsManager.PreferredBackBufferWidth, StateManager.GraphicsManager.PreferredBackBufferHeight);
                }

                if (ScreenResolutionChanged != null)
                {
                    ScreenResolutionChanged(null, new ViewportEventArgs() { Viewport = GraphicsManager.GraphicsDevice.Viewport, IsFullScreen = GraphicsManager.IsFullScreen });
                }
            }
        }

        #endregion Public Classes
    }
}
