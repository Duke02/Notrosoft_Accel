using System;

namespace Notrosoft_Accel.Infrastructure
{
    public class Bounds<T> where T : IComparable
    {
        public Bounds(T start, T end)
        {
            Start = start;
            End = end;
        }

        public T Start { get; }
        public T End { get; }

        public bool IsWithinBounds(T value)
        {
            return value.CompareTo(Start) >= 0 && value.CompareTo(End) <= 0;
        }
    }
}