using System;
using OpenTK;
using OpenTK.Graphics;

namespace Spellie
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			C.Read("settings", runWithSettings);
		}

		public static void runWithSettings(C.ValueSet config)
		{			
			using (
			SpellieVenster gw = new SpellieVenster(
				config))
			{
				gw.Run (100);
			}
		}
	}
}
