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
using Microsoft.Xna.Framework.Net;

namespace PGCGame
{
    public static class StateManager
    {
        #region Private Fields
        private static BulletPool _bulletPool;
        private static Game _game;
        private static bool _hasBoughtScanner = false;
        public static int HealthPacks = 0;
        private static Stack<ScreenType> _screenStack = new Stack<ScreenType>();
        private static ScreenType _screenState = ScreenType.Title;
        private static GraphicsDeviceManager _gfx;
        private static Guid _enemyID = Guid.NewGuid();
        private static int _spaceBucks = 100000;
        #endregion Private Fields

        #region Public Fields


        public static bool GamerServicesAreAvailable = true;

        public static StorageDevice SelectedStorage = null;

        public static BulletCollection EnemyBullets = new BulletCollection();
        public static BulletCollection AllyBullets = new BulletCollection();

        public static Boolean[,] KnownMap = new Boolean[9, 15];

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
            set
            {
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
            AllyBullets = new BulletCollection();
            EnemyBullets = new BulletCollection();
            if (_bulletPool == null)
            {
                _bulletPool = new BulletPool();
            }
            Screens.Shop.firstShop = true;
            nextLevel = false;
            HighestUnlockedLevel = GameLevel.Level1;
            SpacePoints = 0;
            _hasBoughtScanner = false;
            Stack<ScreenType> _screenStack = new Stack<ScreenType>();
            Guid _enemyID = Guid.NewGuid();
            _spaceBucks = 100000;
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

        /// <summary>
        /// Gets the current pool of bullets.
        /// </summary>
        public static BulletPool BulletPool
        {
            get
            {
                if (_bulletPool == null)
                {
                    _bulletPool = new BulletPool();
                }
                return _bulletPool;
            }
        }


        public static event EventHandler<ViewportEventArgs> ScreenResolutionChanged
        {
            add
            {
                Options.ScreenResolutionChanged += value;
            }
            remove
            {
                Options.ScreenResolutionChanged -= value;
            }

        }

        public static UpgradesInfo Upgrades
        {
            get
            {
                UpgradesInfo ret = new UpgradesInfo();
                ret.HasScanner = StateManager.BoughtScanner;
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

        public static ScreenType LastScreen
        {
            get
            {
                return _screenStack.Peek();
            }
        }

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

        private static void PlayButtonSfx(object sender, EventArgs ea)
        {
            if (StateManager.Options.SFXEnabled)
            {
                GameContent.Assets.Sound[SoundEffectType.ButtonPressed].Play();
            }
        }

        /// <summary>
        /// Gets an event handler that plays the button click sound.
        /// </summary>
        public static EventHandler ButtonSFXHelper
        {
            get
            {
                return new EventHandler(PlayButtonSfx);
            }
        }

        public static bool ShowShipData
        {
            get
            {
                return BoughtScanner || StateManager.NetworkData.IsMultiplayer;
            }
        }

        /// <summary>
        /// Indicates whether or not the user has bought the "scanner" power up.
        /// </summary>
        public static bool BoughtScanner
        {
            get { return _hasBoughtScanner; }
            set { _hasBoughtScanner = value; }
        }


        public static Rectangle WorldSize { get; set; }

        public static Rectangle SpawnArea { get; set; }

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

        public static TextSprite CreateButtonTextSprite(Boolean boldFont, string text, Sprite parentSprite, Screen parentScreen)
        {
            TextSprite newTextSprite = new TextSprite(parentScreen.Sprites.SpriteBatch, Vector2.Zero, boldFont ? GameContent.Assets.Fonts.BoldText : GameContent.Assets.Fonts.NormalText, text);
            newTextSprite.IsHoverable = true;
#if WINDOWS
            newTextSprite.CallKeyboardClickEvent = false;
#endif
            newTextSprite.NonHoverColor = Color.White;
            newTextSprite.HoverColor = Color.MediumAquamarine;

            newTextSprite.ParentSprite = parentSprite;

            parentScreen.AdditionalSprites.Add(newTextSprite);
            return newTextSprite;
        }

        public static TScreen GetScreen<TScreen>(ScreenType screenType) where TScreen : BaseScreen
        {
            return AllScreens[screenType.ToString()] as TScreen;
        }

        public static void InitGame(Game underlyingGame)
        {
            _game = underlyingGame;
        }


        public static void InitializeSingleplayerGameScreen(ShipType type, ShipTier tier, bool allowEnemies)
        {
            InitializeSingleplayerGameScreen(new ShipStats(type, tier), allowEnemies);
        }

        public static void InitializeSingleplayerGameScreen(ShipStats ship)
        {
            InitializeSingleplayerGameScreen(ship, true);
        }

        public static void InitializeSingleplayerGameScreen(ShipStats ship, bool spawnEnemies)
        {
            GetScreen<PGCGame.Screens.GameScreen>(ScreenType.Game).InitializeScreen(ship, spawnEnemies);
        }

        public static void InitializeSingleplayerGameScreen(ShipType type, ShipTier tier)
        {
            InitializeSingleplayerGameScreen(type, tier, true);
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

        public static class NetworkData
        {
            public static NetworkSessionType SessionType;

            /// <summary>
            /// Determines whether the game is currently in a multiplayer session.
            /// </summary>
            public static bool IsMultiplayer
            {
                get
                {
                    return CurrentSession != null && !CurrentSession.IsDisposed && CurrentSession.AllGamers.Count > 0;
                }
                set
                {
                    if (value && !IsMultiplayer)
                    {
                        throw new InvalidOperationException("You cannot join a session by setting this property. Please assign CurrentSession to a NetworkSession object.");
                    }
                    else if (value && IsMultiplayer)
                    {
                        return;
                    }
                    else if (!value)
                    {
                        LeaveSession();
                    }
                }
            }

            /// <summary>
            /// Leaves and disposes of the current network session.
            /// </summary>
            public static void LeaveSession()
            {
                if (_game != null)
                {
                    _game.Services.RemoveService(typeof(NetworkSession));
                }
                if (CurrentSession != null && !CurrentSession.IsDisposed)
                {
                    CurrentSession.Dispose();
                }
                CurrentSession = null;
            }

            public static MultiplayerSessionType SessionMode;

            public static ShipStats SelectedNetworkShip;

            private static NetworkSession _currentSession;

            /// <summary>
            /// Gets the current <see cref="Microsoft.Xna.Framework.Net.NetworkSession"/> that is the game in which the user is playing. This value should not be used for checking if we are in a multiplayer game, for that use <seealso cref="IsMultiplayer"/>.
            /// </summary>
            public static NetworkSession CurrentSession
            {
                get { return _currentSession; }
                private set { _currentSession = value; }
            }


            /// <summary>
            /// Registers the specified network session as the current network session.
            /// </summary>
            public static void RegisterNetworkSession(NetworkSession register)
            {
                if (register == null)
                {
                    throw new ArgumentNullException("register");
                }
                CurrentSession = register;
                _game.Services.AddService(typeof(NetworkSession), CurrentSession);
                DataReader = new PacketReader();
                DataWriter = new PacketWriter();
                //CurrentSession.GameStarted += new EventHandler<GameStartedEventArgs>(CurrentSession_GameStarted);
                CurrentSession.GameEnded += new EventHandler<GameEndedEventArgs>(CurrentSession_GameEnded);
            }

            private static void CurrentSession_GameEnded(object sender, GameEndedEventArgs e)
            {
                if (CurrentSession != null)
                {
                    CurrentSession.Dispose();
                    CurrentSession = null;
                }
            }

            private static PacketReader _dataReader;

            /// <summary>
            /// Gets the <see cref="PacketReader"/> used to read data sent from the network.
            /// </summary>
            public static PacketReader DataReader
            {
                get { return _dataReader; }
                private set { _dataReader = value; }
            }

            private static PacketWriter _dataWriter;

            /// <summary>
            /// Gets the <see cref="PacketWriter"/> used to write data across the network.
            /// </summary>
            public static PacketWriter DataWriter
            {
                get { return _dataWriter; }
                private set { _dataWriter = value; }
            }

            public static AvailableNetworkSessionCollection AvailableSessions;
        }

        public static class DebugData
        {
            public static bool InfiniteMoney = false;
            public static bool DebugBackground = false;
            public static bool ShipSpeedIncrease = true;
            public static bool OPBullets = false;
            public static bool ShowShipIDs = false;
            public static bool EmergencyHeal = false;
            public static bool KillAll = false;
            public static bool Invincible = false;
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
            private static KeyboardState _lastKs = new KeyboardState();

            public static class DebugControlManager
            {
                static DebugControlManager()
                {
                    KeyboardManager.KeyDown += new Glib.XNA.SingleKeyEventHandler(KeyboardManager_KeyDown);
                }
                public static event EventHandler ShipHeal;
                public static event EventHandler ShipSuicide;

                private static void KeyboardManager_KeyDown(object source, Glib.XNA.SingleKeyEventArgs e)
                {
                    if (StateManager.DebugData.KillYourSelf && e.Key == Keys.F3 && ShipSuicide != null)
                    {
                        ShipSuicide(KeyboardManager.State, EventArgs.Empty);
                    }
                }

                internal static void Update()
                {
                    if (ShipHeal != null && StateManager.DebugData.EmergencyHeal && GamePadManager.One.Current.DPad.Down == ButtonState.Pressed && ButtonState.Pressed == GamePadManager.One.Current.DPad.Right)
                    {
                        ShipHeal(GamePadManager.One, EventArgs.Empty);
                    }
                    if (ShipHeal != null && StateManager.DebugData.EmergencyHeal && KeyboardManager.State.IsKeyDown(Keys.F4))
                    {
                        ShipHeal(KeyboardManager.State, EventArgs.Empty);
                    }
                    if (StateManager.DebugData.KillYourSelf && ShipSuicide != null && GamePadManager.One.Current.IsButtonDown(Buttons.RightStick) && GamePadManager.One.Current.IsButtonDown(Buttons.LeftStick))
                    {
                        ShipSuicide(GamePadManager.One, EventArgs.Empty);
                    }

                }
            }

            internal static void Update()
            {
                DebugControlManager.Update();
            }

            internal static void UpdateLastState()
            {
                _lastKs = KeyboardManager.State;
            }

            public static bool ShootControlDown
            {
                get
                {
#if WINDOWS
                    return (StateManager.Options.LeftButtonEnabled && MouseManager.CurrentMouseState.LeftButton == ButtonState.Pressed) || (!StateManager.Options.LeftButtonEnabled && KeyboardManager.State.IsKeyDown(Keys.Space));
#elif XBOX
                    return GamePadManager.One.Current.Triggers.Right > 0.875f;
#else
                    return false;
#endif
                }
            }

            public static bool IsKeyDownOnFrame(Keys key)
            {
                return _lastKs.IsKeyUp(key) && KeyboardManager.State.IsKeyDown(key);
            }

            public static bool DeploySecondaryWeapon(SecondaryWeaponType currentIndex)
            {
                return StateManager.PowerUps[currentIndex.ToInt()].Count > 0 && ((StateManager.Options.SecondaryButtonEnabled && KeyboardManager.State.IsKeyDown(Keys.RightShift) && _lastKs.IsKeyUp(Keys.RightShift)) || (!StateManager.Options.SecondaryButtonEnabled && KeyboardManager.State.IsKeyDown(Keys.R) && KeyboardManager.State.IsKeyUp(Keys.R)));

            }

            /// <param name="direction">-1 means left one, +1 means right one.</param>
            public static bool ShouldMoveSecondaryWeaponSelection(short direction)
            {
                if (direction != -1 && direction != 1)
                {
                    throw new ArgumentException("Direction must equal either -1 or 1.");
                }
                if (direction == 1)
                {
                    return (!StateManager.Options.SwitchButtonEnabled && (KeyboardManager.State.IsKeyDown(Keys.E) && _lastKs.IsKeyUp(Keys.E)))
                    || (StateManager.Options.SwitchButtonEnabled && (KeyboardManager.State.IsKeyDown(Keys.PageUp) && _lastKs.IsKeyUp(Keys.PageUp)));
                }
                else if (direction == -1)
                {
                    return (!StateManager.Options.SwitchButtonEnabled && (KeyboardManager.State.IsKeyDown(Keys.Q) && _lastKs.IsKeyUp(Keys.Q)))
                    || (StateManager.Options.SwitchButtonEnabled && (KeyboardManager.State.IsKeyDown(Keys.PageDown) && _lastKs.IsKeyUp(Keys.PageDown)));
                }
                return false;
            }

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

        public static class MusicManager
        {
            private static MediaState? _mediaState;

            private static ScreenMusic? _currentMusic = null;

            public static ScreenMusic? CurrentMusic
            {
                get { return _currentMusic; }
                private set { _currentMusic = value; }
            }
            

            public static MediaState MediaPlayerState
            {
                get
                {
                    if (!_mediaState.HasValue)
                    {
                        _mediaState = MediaPlayer.State;
                    }
                    return _mediaState.Value;
                }
                private set { _mediaState = value; }
            }

            public static void Stop()
            {
                MediaPlayer.Stop();
                _currentMusic = null;
                _mediaState = MediaState.Stopped;
            }

            public static void Resume()
            {
                MediaPlayer.Resume();
                _mediaState = MediaState.Playing;
            }

            public static void Pause()
            {
                MediaPlayer.Pause();
                _mediaState = MediaState.Paused;
            }

            public static void Play(ScreenMusic song)
            {
                MediaPlayer.Play(GameContent.Assets.Music[song]);
                _currentMusic = song;
                _mediaState = MediaState.Playing;
            }
        }

        public static class Options
        {
            private static bool _musicEnabled = true;

            public static event EventHandler<ViewportEventArgs> ScreenResolutionChanged;

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
