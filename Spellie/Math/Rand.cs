using System;
using System.Collections.Generic;
using System.Text;

namespace NachoMark.Math
{
    public static class Rand
    {
        static Random Random = new Random();

        public static float om(float offset, float extra)
        {
            return offset + (float)Random.NextDouble() * extra;
        }

        public static int um(int exclusivemax)
        {
            return Random.Next(exclusivemax);
        }
    }
}
