using System;
using OpenTK;
using OpenTK.Graphics;

namespace Spellie
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Settings Configuration = new Settings ("settings.txt");

			using (
			SpellieVenster gw = new SpellieVenster(
				int.Parse(Configuration["width"]), 
				int.Parse(Configuration["height"]), 
				GraphicsMode.Default, "Spellie", 
				(GameWindowFlags)Enum.Parse(typeof(GameWindowFlags), Configuration["mode"]))) 
			{
				gw.Run(
					int.Parse(Configuration["gamespeed"]), 
					int.Parse(Configuration["framerate"]));
			}
		}
	}
}
