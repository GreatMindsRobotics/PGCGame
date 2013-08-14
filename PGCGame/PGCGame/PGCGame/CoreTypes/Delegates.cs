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

        public delegate void AsyncHandlerMethod(object asyncResult);

        /// <summary>
        /// Provides ability to run code on the next update of a screen.
        /// </summary>
        public delegate void NextRun();

        /// <summary>
        /// Checks if current window has focus; use to reduce input checks in Normal mode
        /// </summary>
        /// <returns>true if game has focus; otherwise false</returns>
        public delegate bool CheckIfWindowFocused();
    }
}
