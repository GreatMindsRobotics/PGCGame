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
                }
            }
        }
        public static ScreenManager AllScreens;
    }
}
