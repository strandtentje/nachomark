using System;
using System.Collections.Generic;
using System.Text;

namespace NachoMark
{
    /// <summary>
    /// A PID controller with PIDC settings.
    /// </summary>
    public class PID
    {
        /// <summary>
        /// Gain of effect of current error on acceleration.
        /// </summary>
        public float ProportionalGain = 0.1f;
        /// <summary>
        /// Gain of effect of all errors ever on acceleration.
        /// </summary>
        public float IntegralGain = 0.1f;
        /// <summary>
        /// Gain of effect of the difference between the last 
        /// two errors on acceleration
        /// </summary>
        public float DifferentialGain = 0.1f;
        /// <summary>
        /// Percentual limiter on the integral error that may build up.
        /// </summary>
        public float IntegralCap = 0.1f;

        float
            proportionalError,
            integralError,
            differentialError;

        float
            latestAcceleration;

        /// <summary>
        /// Calculate an acceleration using the PID formula
        /// </summary>
        /// <param name="CurrentState">State current.</param>
        /// <param name="TargetState">State target</param>
        /// <returns>Acceleration</returns>
        public float GetAcceleration(float CurrentState, float TargetState)
        {
            proportionalError = TargetState - CurrentState;
            integralError += proportionalError;
            integralError *= IntegralCap;

            latestAcceleration =
                proportionalError * ProportionalGain + 
                integralError * IntegralGain +
                (proportionalError - differentialError) * DifferentialGain;

            differentialError = proportionalError;

            return latestAcceleration;
        }

        /// <summary>
        /// Consutrct a new PID that has thesame parameters
        /// as this one.
        /// </summary>
        /// <returns>The identical PID.</returns>
        public PID Clone()
        {
            return new PID()
            {
                ProportionalGain = this.ProportionalGain,
                IntegralGain = this.IntegralGain,
                DifferentialGain = this.DifferentialGain,
                IntegralCap = this.IntegralCap,
            };
        }
    }
}
