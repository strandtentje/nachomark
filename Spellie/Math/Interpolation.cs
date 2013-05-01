using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace NachoMark.Math
{
    public static class Interpolation
    {

        /// <summary>
        /// Interpolate a float from two other floats and a ratio.
        /// </summary>
        /// <param name="a">0-float</param>
        /// <param name="b">1-float</param>
        /// <param name="p">ratio-float (0-1)</param>
        /// <returns></returns>
        public static float interPolateRatio(float a, float b, float p)
        {
            float c = b - a;
            return a + c * p;
        }

        /// <summary>
        /// Interpolate a float from two other floats and a fraction.
        /// </summary>
        /// <param name="a">0-float</param>
        /// <param name="b">entire-float</param>
        /// <param name="part">ratio-float (0-entire)</param>
        /// <param name="entire">1-float</param>
        /// <returns></returns>
        public static float interPolatePc(float a, float b, int part, int entire)
        {
            return interPolateRatio(a, b, (float)part / (float)entire);
        }
        
        /// <summary>
        /// Interpolate a Vector2 from two other Vector2's and a fraction.
        /// </summary>
        /// <param name="a">0-vector</param>
        /// <param name="b">entire-vector</param>
        /// <param name="part">ratio-float (0-entire)</param>
        /// <param name="entire">1-float</param>
        /// <returns></returns>
        public static Vector2 interPolateVector(Vector2 a, Vector2 b, int part, int entire)
        {
            float r = (float)part / (float)entire;

            return new Vector2(
                interPolateRatio(a.X, b.X, r),
                interPolateRatio(a.Y, b.Y, r));
        }

    }
}
