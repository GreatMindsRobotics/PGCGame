using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;
using Glib;
using Microsoft.Xna.Framework;

using PGCGame.CoreTypes;
using Microsoft.Xna.Framework.Graphics;

namespace PGCGame
{
    public static class StateManager
    {
        #region Private Fields
        
        private static Stack<ScreenState> _screenStack = new Stack<ScreenState>();
        private static ScreenState _screenState = ScreenState.Title;
        private static GraphicsDeviceManager _gfx;                

        #endregion Private Fields

        #region Public Fields

        public static CheckIfWindowFocused IsWindowFocused;

        /// <summary>
        /// Keeps track of active ships in the game. This info can be used for mini-map, collision detection, etc
        /// </summary>
        public static readonly List<Ship> ActiveShips = new List<Ship>();

        /// <summary>
        /// Identifies the player in the network game
        /// </summary>
        public static readonly Guid PlayerID = Guid.NewGuid();

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
        /// Random generator to use throughout the game; ensures no concurrent seeding of two Random objects
        /// </summary>
        public static readonly Random RandomGenerator = new Random();

        #endregion Public Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets ScreenState for the game, indicating the currently visible screen.
        /// StateManager keeps track of active screens on the screenStack.
        /// </summary>
        public static ScreenState ScreenState
        {
            get
            {
                return _screenState;
            }
            set
            {
                _screenStack.Push(value);
                _screenState = value;

                SwitchScreen(value);
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

        #endregion Public Properties



        #region Private Methods
 
        /// <summary>
        /// Switches the active screen based on screenState parameter.
        /// Note: Currently only a single screen can be visible at any one time.
        /// </summary>
        /// <param name="screenState">Screen to switch to</param>
        private static void SwitchScreen(ScreenState screenState)
        {
            foreach (Screen screen in AllScreens)
            {
                screen.Visible = false;
            }

            switch (screenState)
            {
                case ScreenState.Title:
                    AllScreens["titleScreen"].Visible = true;

                    break;
                case ScreenState.MainMenu:
                    AllScreens["mainMenuScreen"].Visible = true;
                    break;
                case ScreenState.Credits:
                    AllScreens["creditsScreen"].Visible = true;
                    break;
                case ScreenState.Game:
                    AllScreens["gameScreen"].Visible = true;
                    break;
                case ScreenState.Option:
                    AllScreens["optionScreen"].Visible = true;
                    break;
                case ScreenState.Shop:
                    AllScreens["shopScreen"].Visible = true;
                    break;
                case ScreenState.Pause:
                    AllScreens["pauseScreen"].Visible = true;
                    break;
                case ScreenState.ShipSelect:
                    AllScreens["shipSelectScreen"].Visible = true;
                    break;
                case ScreenState.WeaponSelect:
                    AllScreens["weaponSelectScreen"].Visible = true;
                    break;
                case ScreenState.UpgradeScreen:
                    AllScreens["upgradeScreen"].Visible = true;
                    break;
                case ScreenState.TierSelect:
                AllScreens["tierSelectScreen"].Visible = true;
                break;
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Initializes the game for single player based on selected ship type and tier
        /// </summary>
        /// <typeparam name="T">Ship type</typeparam>
        /// <param name="tier">Ship tier</param>
        public static void InitializeSingleplayerGameScreen<T>(ShipTier tier) where T : Ship
        {
            AllScreens["gameScreen"].Cast<Screens.GameScreen>().InitializeScreen<T>(tier);
        }

        /// <summary>
        /// Returns to previous screen
        /// </summary>
        public static void GoBack()
        {
            _screenStack.Pop();
            _screenState = _screenStack.Peek();
            SwitchScreen(_screenState);
        }

        #endregion Public Methods


        #region Public Classes

        public static class Options
        {
            private static bool _musicEnabled = true;

            public static event EventHandler ScreenResolutionChanged;

            public static bool SFXEnabled { get; set; }
            public static bool ArrowKeysEnabled { get; set; }
            public static bool LeftButtonEnabled { get; set; }

            public static bool MusicEnabled
            {
                get { return _musicEnabled; }
                set { _musicEnabled = value; }
            }

            public static void CallResChangeEvent()
            {
                if (ScreenResolutionChanged != null)
                {
                    ScreenResolutionChanged(null, new ViewportEventArgs() { Viewport = GraphicsManager.GraphicsDevice.Viewport, IsFullScreen = GraphicsManager.IsFullScreen });
                }
            }
        }

        #endregion Public Classes
    }
}
