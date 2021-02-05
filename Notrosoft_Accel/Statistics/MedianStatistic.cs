using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notrosoft_Accel.Statistics
{
    /// <summary>
    /// Calculates the median (middle value of sorted data) statistic from given data.
    /// </summary>
    public class MedianStatistic : IStatistic
    {
        /// <summary>
        /// Calculates the Median Statistic from the provided 2 dimensional data.
        /// </summary>
        /// <param name="values">The 2 dimensional data to calculate the median from.</param>
        /// <returns>The median of the data.</returns>
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            // Flattens the 2D input into a 1d enumerable.
            var flattenedInput = Utilities.Flatten(values);

            // Throw an exception if there's nothing in the inputted container.
            if (flattenedInput.Count() <= 0)
            {
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");
            }

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
