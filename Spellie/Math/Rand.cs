using System;
using System.Collections.Generic;
using System.Text;

namespace NachoMark.Math
{
    /// <summary>
    /// Random operations
    /// </summary>
    public static class Rand
    {
        static Random Random = new Random();

        /// <summary>
        /// Calculate a random float in a range.
        /// </summary>
        /// <param name="offset">Offset</param>
        /// <param name="extra">Factor</param>
        /// <returns>Random number</returns>
        public static float om(float offset, float extra)
        {
            return offset + (float)Random.NextDouble() * extra;
        }

        /// <summary>
        /// Calculate a random number starting at 0
        /// </summary>
        /// <param name="exclusivemax">Exclusive maximum</param>
        /// <returns>Random number</returns>
        public static int um(int exclusivemax)
        {
            return Random.Next(exclusivemax);
        }
    }
}
