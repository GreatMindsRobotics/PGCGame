using System;

namespace PGCGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(string[] args)
        {
            Game1 game = null;
            try
            {
                game = new Game1();

                game.Run();
            }
            catch (Exception ex)
            {
                bool needsAltMsg = false;
                if (game != null)
                {
                    try
                    {
                        needsAltMsg = !game.ShowError(ex);
                    }
                    catch { needsAltMsg = true; }
                }
                else
                {
                    needsAltMsg = true;
                }

#if WINDOWS
                if (needsAltMsg)
                {
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        
                        Console.WriteLine("ERROR IN RUNTIME OF PLEQUARIUS: GALACTIC COMMANDERS");
                        Console.WriteLine("Details of exception: {0}", ex.GetType().FullName);
                        Console.WriteLine(ex.Message);
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine("Inner error: {0}", ex.InnerException.GetType().FullName);
                            Console.WriteLine(ex.InnerException.Message);
                        }

                        return 1;
                    }
                    catch { needsAltMsg = true; }
                }
#endif

                if (needsAltMsg)
                {
                    //Sorry, Joe. I've tried everything that I have.
                    throw ex;
                }
            }
            finally
            {
                if (game != null)
                {
                    game.Dispose();
                }
            }
            return 0;
        }
    }
#endif
}

