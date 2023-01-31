using Foundation;
using System;
using UIKit;

namespace EyesOfTheDragoniOS
{
    [Register("AppDelegate")]
    internal class Program : UIApplicationDelegate
    {
        private static iOS game;

        internal static void RunGame()
        {
            game = new iOS();
            game.Run();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, typeof(Program));
        }

        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }
    }
}
