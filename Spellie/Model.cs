using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;

namespace Spellie
{
	public class Model : List<Vector2>
	{
		public float OffsetX { get; private set; }
		public float OffsetY { get; private set; }
		public string Name { get; private set;} 

		public Model (string name, string file)
		{
			this.Name = name;

			CSV.Read(file, delegate(string[] coords)
         	{
				this.Add(
					new Vector2(
					float.Parse(coords[0]),
					float.Parse(coords[1])));
			});
		}

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
		public Vector3[] GetPoints (float x, float y, float s, float c, float r, float d)
		{
			List<Vector3> vecs = new List<Vector3>();

			foreach (Vector2 v in this)
				vecs.Add(new Vector3(
					x + v.X * c * r - v.Y * s * r,
					y + v.X * s * r + v.Y * c * r, 
					d));

			return vecs.ToArray();
		}
	}
}

