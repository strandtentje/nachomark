using System;
using OpenTK;
using OpenTK.Graphics;
using System.Threading;
using NachoMark.IO;

namespace NachoMark
{
	class MainClass
	{
        static SpellieVenster Window;
        static int updateRate;

		public static void Main (string[] args)
		{
            Console.WriteLine("NachoMark by Rob Tierolff, www.borreh.nl");
			CLON.Read("settings", runWithSettings);

            using (Window)
            {
                Window.Run(updateRate);
            }

            Thread.Sleep(1900);
		}

		public static void runWithSettings(ValueSet config)
		{
            if (config.Name == "settings")
            {
                Window = new SpellieVenster(config);
                updateRate = config.TryGetInt("updaterate", 100);
            }
            else
                Window.AddSnake(config);
		}
	}
}
