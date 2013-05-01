using System;
using System.Collections.Generic;
using System.Text;

namespace NachoMark.Math
{
    /// <summary>
    /// Bounces a floating point value between a minimum and a maximum.
    /// </summary>
    public class FloatBounce
    {
        float min, max;

        /// <summary>
        /// Construct a new bouncer
        /// </summary>
        /// <param name="min">The minimal value of the bounced value
        /// </param>
        /// <param name="max">The maximal value of the bounced value
        /// </param>
        /// <param name="delta">The increment per query</param>
        public FloatBounce(float min, float max, float delta)
        {
            this.min = min; this.max = max; this.delta = delta;
        }

        float value, delta;

        /// <summary>
        /// Increment or decrement the value
        /// </summary>
        public void Update()
        {
            value += delta;

            if (value > max)
            {
                value = max;
                delta -= delta;
            }
            if (value < min)
            {
                value = min;
                delta -= delta;
            }
        }

        /// <summary>
        /// Acquire the next value
        /// </summary>
        public float Current
        {
            get
            {
                return value;
            }
        }
    }
}
