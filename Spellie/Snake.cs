using System;

namespace Spellie
{
	public class Snake
	{		
		public Level Level;
		Entity bait = new Entity();
		
		int ctr = 0;

		static Random rnd = new Random();

		public void Update()
		{			
			ctr++;

			if (ctr == 110)
			{
				bait.X = (4f + (float)rnd.NextDouble() * 4f) * (rnd.Next(2) == 1 ? -1 : 1);
				bait.Y = (4f + (float)rnd.NextDouble() * 4f) * (rnd.Next(2) == 1 ? -1 : 1);
				bait.Z = (1f + (float)rnd.NextDouble() * 16f);
				ctr =0;
			}			

			Level.DoPathfinding();

			Level.UpdatePoints();
		}

		public void Render(float r, float g, float b)
		{
			Level.Draw(r, g, b);
		}

		public Snake (int cBase)
		{
			Level = new Level(cBase);//"level");

			Level[0].Target = bait;
		}
	}
}

