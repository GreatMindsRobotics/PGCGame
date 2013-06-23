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
        private static ScreenState _screenState = ScreenState.Title;

        public static void InitializeSingleplayerGameScreen<T>(ShipTier tier) where T : Ship
        {
            AllScreens["gameScreen"].Cast<Screens.GameScreen>().InitializeScreen<T>(tier);
        }

        

        public static ScreenState ScreenState
        {
            get
            {
                return _screenState;
            }
            set
            {
                _screenState = value;
                foreach (Screen screen in AllScreens)
                {
                    screen.Visible = false;
                }

                switch (value)
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
                    
                }
            }
        }
        public static ScreenManager AllScreens;

        public static Point ViewportSize;

        private static GraphicsDeviceManager _gfx;
        public static GraphicsDeviceManager GraphicsManager 
        {
            get { return _gfx; }
            
            set
            {
                _gfx = value;
                ViewportSize = new Point(_gfx.GraphicsDevice.Viewport.Width, _gfx.GraphicsDevice.Viewport.Height);
            }

        
        }

        public static class Options
        {
            public static event EventHandler ScreenResolutionChanged;

            internal static void callResChangeEvent()
            {
                if (ScreenResolutionChanged != null)
                {
                    ScreenResolutionChanged(null, new ViewportEventArgs() { Viewport = GraphicsManager.GraphicsDevice.Viewport, IsFullScreen = HDEnabled });
                }
            }

            public static bool SFXEnabled { get; set; }

            private static bool _musicEnabled = true;


            public static bool MusicEnabled
            {
                get { return _musicEnabled; }
                set { _musicEnabled = value; }
            }
            
            
            public static bool HDEnabled { get; set; }
        }
    }
}
