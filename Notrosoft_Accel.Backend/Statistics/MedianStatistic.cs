using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the median (middle value of sorted data) statistic from given data.
    /// </summary>
    public class MedianStatistic : IStatistic
    {
        /// <summary>
        ///     Calculates the Median Statistic from the provided 2 dimensional data.
        /// </summary>
        /// <param name="values">The 2 dimensional data to calculate the median from.</param>
        /// <param name="param"></param>
        /// <returns>The median of the data.</returns>
        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] param)
        {
            // Flattens the 2D input into a 1d enumerable.
            var flattenedInput = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedInput.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Sorts the input in ascending order then converts it into an array.
            var sortedInput = flattenedInput.OrderBy(val => val).ToArray();

            var count = sortedInput.Length;

            double median;

            // If the length is even...
            if (count % 2 == 0)
            {
                // Get the elements that are around the middle 
                var firstIndex = count / 2;
                var secondIndex = firstIndex - 1;

                // And return their midpoint.
                median = (sortedInput[secondIndex] + sortedInput[firstIndex]) / 2.0;
            }
            else
            {
                // Just return the middle of the input.
                var middleIndex = count / 2;

                median = sortedInput[middleIndex];
            }

            return new Dictionary<string, object>
            {
                {
                    "median", median
                }
            };
        }

        public Dictionary<string, object> OperateIntervalData(IntervalData values,
            params object[] parameters)
        {
            int length = values.Count();
            var flattened = new List<double>();
            foreach (var kv in values)
            {
                flattened.AddRange(kv.Value);
            }
            var flattenedInput = flattened.ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedInput.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Sorts the input in ascending order then converts it into an array.
            var sortedInput = flattenedInput.OrderBy(val => val).ToArray();

            var count = sortedInput.Length;

            double median;

            // If the length is even...
            if (count % 2 == 0)
            {
                // Get the elements that are around the middle 
                var firstIndex = count / 2;
                var secondIndex = firstIndex - 1;

                // And return their midpoint.
                median = (sortedInput[secondIndex] + sortedInput[firstIndex]) / 2.0;
            }
            else
            {
                // Just return the middle of the input.
                var middleIndex = count / 2;

                median = sortedInput[middleIndex];
            }

            return new Dictionary<string, object>
            {
                {
                    "median", median
                }
            };
        }

        public Dictionary<string, object> OperateFrequencyData<T>(FrequencyData<T> values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}