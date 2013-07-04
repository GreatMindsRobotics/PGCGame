using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGCGame.CoreTypes
{
    public static class Delegates
    {
        /// <summary>
        /// Provides ability to exit the game from any screen; delegate call to Game.Exit();
        /// </summary>
        public delegate void QuitFunction();
    }
}
