using System;
using System.Collections.Generic;
using OpenTK;

namespace Spellie
{
	public class VectorSet
	{
		public List<Vector2> Points = new List<Vector2>();
		public abstract void RecalcPoints();
	}

}

