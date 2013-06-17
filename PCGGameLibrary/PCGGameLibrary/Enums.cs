using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitPractice
{
    //TODO: Alex

    public enum ScreenState
    {
        //TODO: (TitleScreen, GamePlayScreen, GameOverScreen)
        TitleScreen,
        GamePlayScreen,
        GameOverScreen
    }

    public enum GameState
    {
        //TODO: (NotStarted, Playing, GameWon, GameLost, Paused)
        NotStarted,
        Playing,
        GameWon,
        GameLost,
        Paused
    }

    public enum MoveDirection
    { 
        //TODO: (Up, Down, Left, Right)
        Up,
        Down,
        Left,
        Right
    }
}
