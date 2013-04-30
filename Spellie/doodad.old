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
		public void Get (float x, float y, float s, float c, float r, float d, ref Vertex[] array, int offset)
		{
			for (int i = 0; i < 3; i++) {
                array[i + offset].Position.X = x + tr[i].X * c * r - tr[i].Y * s * r;
                array[i + offset].Position.Y = y + tr[i].X * s * r + tr[i].Y * c * r;
                array[i + offset].Position.Z = d;                
			}			
		}
	}
}

