using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notrosoft_Accel.Statistics
{
    public class MedianStatistic : IStatistic
    {
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            // Flattens the 2D input into a 1d enumerable.
            var flattenedInput = Utilities.Flatten(values);

            // Sorts the input in ascending order then converts it into an array.
            var sortedInput = flattenedInput.OrderBy(val => val).ToArray();

            var count = sortedInput.Length;

            // If the length is even...
            if (count % 2 == 0)
            {
                // Get the elements that are around the middle 
                int firstIndex = count / 2;
                int secondIndex = firstIndex - 1;

                // And return their midpoint.
                return (sortedInput[secondIndex] + sortedInput[firstIndex]) / 2.0;
            }
            else
            {
                // Just return the middle of the input.
                int middleIndex = count / 2;

                return sortedInput[middleIndex];
            }
        }
    }
}
