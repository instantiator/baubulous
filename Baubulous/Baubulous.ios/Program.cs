using System;
using Foundation;
using UIKit;
using Baubulous.Portable;

namespace Baubulous.ios
{
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
    {
        private static BaubulousGame game;

        internal static void RunGame()
        {
            game = new BaubulousGame();
            game.Run();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
        }

        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }
    }
}
