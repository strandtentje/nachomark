using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics;

namespace NachoMark.OpenGL
{
    /// <summary>
    /// Class to modify the hue of a tri in a vertexbuffer.
    /// </summary>
    public class Colourization
    {
        Vertex[] GraphicsBuffer;
        int GraphicsBufferPosition;

        /// <summary>
        /// Construct a new Colourization
        /// </summary>
        /// <param name="GraphicsBuffer">For a tri in this buffer</param>
        /// <param name="GraphicsBufferPosition">At this position n..n+2</param>
        public Colourization(Vertex[] GraphicsBuffer, int GraphicsBufferPosition)
        {
            this.GraphicsBuffer = GraphicsBuffer;
            this.GraphicsBufferPosition = GraphicsBufferPosition;
        }

        float Red, Green, Blue;

        public float RedAdd, GreenAdd, BlueAdd;

        /// <summary>
        /// Update the color of the tri.
        /// </summary>
        /// <param name="Colour">The new base colour 0..15</param>
        /// <param name="Brightness">The brightness of the colour</param>
        /// <param name="BMod">Additional blueness</param>
        /// <param name="GMod">Additional greenness</param>
        /// <param name="RMod">Additional redness</param>
        public void Update(
            int Colour, float Brightness,
            float RMod, float GMod, float BMod)
        {
            RedAdd = RMod; GreenAdd = GMod; BlueAdd = BMod;

            Red = ((Colour & 4) != 0 ? 1.0f : 0f);
            Green = ((Colour & 2) != 0 ? 1.0f : 0f);
            Blue = ((Colour & 1) != 0 ? 1.0f : 0f);

            Red += RMod; Green += GMod; Blue += BMod;
            Red *= Brightness; Green *= Brightness; Blue *= Brightness;

            for (int i = GraphicsBufferPosition; i < GraphicsBufferPosition + Triangle.Model.Length; i++)
            {
                GraphicsBuffer[i].Color.A = 1.0f;
                GraphicsBuffer[i].Color.R = Red;
                GraphicsBuffer[i].Color.G = Green;
                GraphicsBuffer[i].Color.B = Blue;
            }
        }
    }
}
