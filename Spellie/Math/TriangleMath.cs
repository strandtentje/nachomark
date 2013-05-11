using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace NachoMark.Math
{
    public static class TriangleMath
    {
        /// <summary>
        /// Check if two triangles overlap
        /// </summary>
        /// <param name="A">A vector3 array of three entries</param>
        /// <param name="B">Another vector3 array of three entries 
        /// to overlap</param>
        /// <returns>Overlappiness of these two arrays</returns>
        public static bool overlappingTriangles(Vector3[] A, Vector3[] B)
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
        
    }
}
