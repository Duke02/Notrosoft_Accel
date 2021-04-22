using System;
using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Infrastructure
{
    public class Bounds
    {
        public Bounds(double start, double end)
        {
            Start = Math.Min(end, start);
            End = Math.Max(end, start);
        }

        public Bounds(IEnumerable<double> data) : this(data.Min(), data.Max())
        {
        }

        /// <summary>
        ///     The inclusive start of the bounds.
        /// </summary>
        public double Start { get; }

        /// <summary>
        ///     The exclusive end of the bounds.
        /// </summary>
        public double End { get; }

        public bool IsWithinBounds(double value)
        {
            return value.CompareTo(Start) >= 0 && value.CompareTo(End) < 0;
        }

        public double GetMidpoint()
        {
            return (Start - End) / 2.0 + Start;
        }

        public double Size => Start - End;
    }
}