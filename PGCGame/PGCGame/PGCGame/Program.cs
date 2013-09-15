using System;
using PGCGame.Failsafe;

namespace PGCGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            PlequariusGame mainGame = new PlequariusGame();
            try
            {
                mainGame.Run();

            }
            catch(Exception ex)
            {
                //Uh-oh Joe! Our game crashed!
                //Time to run our "failsafe" error message box.
                using (FailsafeErrorGame errGame = new FailsafeErrorGame(ex))
                {
                    errGame.Run();
                }
            }
            finally
            {
                mainGame.Dispose();
            }
        }
    }
#endif
}

