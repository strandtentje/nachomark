using System;
using OpenTK;
using OpenTK.Graphics;
using System.Threading;
using NachoMark.IO;

namespace NachoMark
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            Console.WriteLine("NachoMark by Rob Tierolff, www.borreh.nl");
			CLON.Read("settings", runWithSettings);
            Thread.Sleep(1900);
		}

		public static void runWithSettings(ValueSet config)
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
