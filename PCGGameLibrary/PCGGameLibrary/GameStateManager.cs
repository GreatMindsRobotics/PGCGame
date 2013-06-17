using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitPractice
{
    public static class GameStateManager
    {
        //TODO: Alex:

        public static ScreenState ScreenState { get; set; }
        public static GameState GameState { get; set; }
        public static MoveDirection MoveDirection { get; set; }
    }
}
