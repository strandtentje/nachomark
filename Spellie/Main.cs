using System;
using OpenTK;
using OpenTK.Graphics;
using System.Threading;

namespace NachoMark
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            Console.WriteLine("NachoMark by Rob Tierolff, www.borreh.nl");
			C.Read("settings", runWithSettings);
            Thread.Sleep(1900);
		}

		public static void runWithSettings(C.ValueSet config)
		{			
			using (
			SpellieVenster gw = new SpellieVenster(
				config))
			{
				gw.Run (config.TryGetInt("updaterate", 100));
			}
		}
	}
}
