using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;

namespace NachoMark.OpenGL
{
    /// <summary>
    /// Class to modify a tri in a vertexbuffer
    /// </summary>
	public class Triangle
	{
		private static Vector2[] BaseTriangle = new Vector2[3] 
		{
			new Vector2(0f - 0.56f, 0f - 0.5f),
			new Vector2(0f - 0.56f, 1f - 0.5f),
			new Vector2(1.12f - 0.56f, 0.5f - 0.5f) 
		};

        Vertex[] targetArray; int offset;

        /// <summary>
        /// Construct a new Triangle.
        /// </summary>
        /// <param name="targetArray">Vertexbuffer wherein this triangle lives</param>
        /// <param name="offset">Position whereat it lives.</param>
        public Triangle(Vertex[] targetArray, int offset)
        {
            this.targetArray = targetArray;
            this.offset = offset;
        }

        int inBuf;
				
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
		public void Update(
            float X, float Y, 
            float Sine, float Cosine, 
            float Scale, float Depth)
		{
			for (int i = 0; i < 3; i++) {
                inBuf = offset + i;

                targetArray[inBuf].Position.X = 
                    X + BaseTriangle[i].X * Cosine * Scale - BaseTriangle[i].Y * Sine * Scale;
                targetArray[inBuf].Position.Y =
                    Y + BaseTriangle[i].X * Sine * Scale + BaseTriangle[i].Y * Cosine * Scale;
                targetArray[inBuf].Position.Z =
                    Depth;
			}
		}
	}
}

