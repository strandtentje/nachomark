using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;

namespace Spellie
{
	public class Triangle
	{
		private static Vector2[] tr = new Vector2[3] 
		{
			new Vector2(0f - 0.56f, 0f - 0.5f),
			new Vector2(0f - 0.56f, 1f - 0.5f),
			new Vector2(1.12f - 0.56f, 0.5f - 0.5f) 
		};
				
		Vector3[] vecs = new Vector3[3];

		/// <summary>
		/// Draw the Model
		/// </summary>
		/// <param name='x'>
		/// X position
		/// </param>
		/// <param name='y'>
		/// Y position
		/// </param>
		/// <param name='s'>
		/// Sine
		/// </param>
		/// <param name='c'>
		/// Cosine
		/// </param>
		/// <param name="r">
		/// Scale where 1.0 is like model
		/// </param>
		/// <param name="d">
		/// Depth
		/// </param>
		public Vector3[] Get (float x, float y, float s, float c, float r, float d)
		{
			for (int i = 0; i < 3; i++) {
				vecs [i].X = x + tr [i].X * c * r - tr [i].Y * s * r;
				vecs [i].Y = y + tr [i].X * s * r + tr [i].Y * c * r;
				vecs [i].Z = d;
			}
			
			return vecs;
		}
	}
}

