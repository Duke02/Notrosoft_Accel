using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Infrastructure
{
    public class Bounds
    {
        public Bounds(double start, double end)
        {
            Start = start;
            End = end;
        }

        public Bounds(IEnumerable<double> data) : this(data.Min(), data.Max())
        {
        }

        public double Start { get; }
        public double End { get; }

        public bool IsWithinBounds(double value)
        {
            return value.CompareTo(Start) >= 0 && value.CompareTo(End) <= 0;
        }

        public double GetMidpoint()
        {
            return (Start - End) / 2.0;
        }
    }
}