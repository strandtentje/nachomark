using System;
using OpenTK;
using OpenTK.Graphics;
using System.Threading;
using NachoMark.IO;

namespace NachoMark
{
    /// <summary>
    /// Main routine that starts the screensaver or the config.
    /// </summary>
	class MainClass
	{
		public static void Main (string[] args)
		{
            foreach(string arg in args)
                if (arg.Contains("/s") || arg.Contains("/S"))
                { RunConfig(); return; }

			if (args.Length == 1)
            	RunScreensaver(args[0]);
			else
				RunScreensaver();
		}

        /// <summary>
        /// Load settings from file with clon
        /// and show them in a window. Allow 
        /// users to modify these settings.
        /// </summary>
        static void RunConfig()
        {

        }

        static SpellieVenster Window;
        static int updateRate;
        /// <summary>
        /// Load settings from file with clon, set the basic 
        /// parameters of the viewport window, add the 
        /// individual snakes and run.
        /// </summary>
        static void RunScreensaver(string settingsFile = "settings.clon")
        {
            Console.WriteLine("NachoMark by Rob Tierolff, www.borreh.nl");

            CLON settings = new CLON();
            settings.Load(settingsFile);

            ValueSet basicSettings = settings["settings"];
            Window = new SpellieVenster(basicSettings);

            string[] snakeNames = basicSettings["snakes"].Split('.');

            foreach (string snake in snakeNames)
                Window.AddSnake(settings[snake]);
            
            using (Window) 
                Window.Run(
                    basicSettings.TryGetInt("updaterate", 100));

            Thread.Sleep(1900);
        }
	}
}
