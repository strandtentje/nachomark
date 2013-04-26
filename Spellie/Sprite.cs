using System;
using System.Collections.Generic;
using OpenTK;

namespace Spellie
{
	public class Sprite : VectorSet, IDrawable
	{
		int speed = 0; int current = 0;
		List<int> Images = new List<int>();

		public Sprite (string[] images)
		{
			foreach(string imageFile in images)
				Images.Add(GLTools.loadTexture(imageFile));

			for(int i = 0; i < 4; i++) 
				this.Points.Add(
					new Vector2( (i & 2 > 0 ? 1.0f : 0.0f), (i & 1 > 0 ? 1.0f : 0.0f) )
								);
		}



		public void Draw()
		{

		}
	}
}

