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
        public static Vector3[] Model = new Vector3[9] 
		{
            new Vector3(-0.42f, -0.00f, 0.54f),
            new Vector3(-0.83f, 1f, -0.54f),
            new Vector3(-0.83f, -1f, -0.54f),
 
            new Vector3(1f, 0f, -0.62f),
            new Vector3(-0.42f, 0f, 0.54f),
            new Vector3(-0.83f, -1f, -0.54f),
 
            new Vector3(1f, 0f, -0.62f),
            new Vector3(-0.42f, 0f, 0.54f),
            new Vector3(-0.83f, 1f, -0.54f)
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
			for (int i = 0; i < Model.Length; i++) {
                inBuf = offset + i;

                targetArray[inBuf].Position.X =
                    X + Model[i].X * Cosine * Scale - Model[i].Z * Sine * Scale;
                targetArray[inBuf].Position.Y =
                    Y + Model[i].X * Sine * Scale + Model[i].Z * Cosine * Scale;
                targetArray[inBuf].Position.Z = -Model[i].Y * 0.02f + Depth;
			}
		}
	}
}

