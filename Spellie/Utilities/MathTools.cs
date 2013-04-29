using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace NachoMark
{
    public static class MathTools
    {

		public static bool overlappingTriangles (Vector3[] A, Vector3[] B)
		{
			bool one, two, three;

			one = pointInTriangle(A[0].Xy, A[1].Xy, A[2].Xy, B[0].Xy);
			two = pointInTriangle(A[0].Xy, A[1].Xy, A[2].Xy, B[1].Xy);
			three = pointInTriangle(A[0].Xy, A[1].Xy, A[2].Xy, B[2].Xy);

			return one || two || three;
		}

        /// <summary>
        /// Mathy things to check if a point is within a rectangle.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public static bool pointInTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
        {
            Vector2 v0, v1, v2;
            // Compute vectors        
            v0 = Vector2.Subtract(C, A);
            v1 = Vector2.Subtract(B, A);
            v2 = Vector2.Subtract(P, A);

            float dot00, dot01, dot02, dot11, dot12;
            // Compute dot products
            dot00 = Vector2.Dot(v0, v0);
            dot01 = Vector2.Dot(v0, v1);
            dot02 = Vector2.Dot(v0, v2);
            dot11 = Vector2.Dot(v1, v1);
            dot12 = Vector2.Dot(v1, v2);

            float invDenom, u, v;
            // Compute barycentric coordinates
            invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Check if point is in triangle
            return (u >= 0) && (v >= 0) && (u + v < 1);
        }

        /* public static bool rectanglesOverlap(Transformation A, Transformation B)
        {
            Vector2[] pts = { B.Corner00, B.Corner01, B.Corner11, B.Corner10 };
            bool wellDoThey = false;

            foreach (Vector2 item in pts)
            {
                wellDoThey = pointInTriangle(A.Corner00, A.Corner01, A.Corner11, item) ||
                    pointInTriangle(A.Corner00, A.Corner10, A.Corner11, item);
                if (wellDoThey) return true;
            }

            return false;
        }*/

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
