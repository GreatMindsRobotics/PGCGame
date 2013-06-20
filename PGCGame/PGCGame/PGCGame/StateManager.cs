using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glib.XNA.SpriteLib;

namespace PGCGame
{
    public static class StateManager
    {
        private static ScreenState _screenState = PGCGame.ScreenState.Title;
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
                    case PGCGame.ScreenState.Option:
                        AllScreens["optionScreen"].Visible = true;
                        break;
                    case ScreenState.Shop:
                        AllScreens["shopScreen"].Visible = true;
                        break;
                    case PGCGame.ScreenState.Pause:
                        AllScreens["pauseScreen"].Visible = true;
                        break;
                }
            }
        }
        public static ScreenManager AllScreens;

        public static class Options
        {
            private static int _volume;
            public static int Volume
            {
                get
                {
                    return _volume;
                }
                set
                {
                    if (value < 0)
                    {
                        _volume = 0;
                    }
                    else if (value > 100)
                    {
                        _volume = 100;
                    }
                    else
                    {
                        _volume = value;
                    }
                }
            }

            //TODO: ADD RESOLUTION
        }
    }
}
