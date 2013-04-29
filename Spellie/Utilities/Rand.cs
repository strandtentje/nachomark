using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spellie.Utilities
{
    public static class Rand
    {
        static Random rnd = new Random();

        public static float Om
        {
            get
            {
                return (float)rnd.NextDouble();
            }
        }

        public static bool Um
        {
            get
            {
                return rnd.Next(2) == 1;
            }
        }

        public static float y(float min, float amp)
        {
            return (min + Rand.Om * amp) * (Rand.Um ? -1 : 1);
        }

        public static int i(int max)
        {
            return rnd.Next(max);
        }
    }
}
