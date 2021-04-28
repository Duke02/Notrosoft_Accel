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
        private const double Tolerance = 0.001;

        /// <summary>
        ///     Calculates the Median Statistic from the provided 2 dimensional data.
        /// </summary>
        /// <param name="values">The 2 dimensional data to calculate the median from.</param>
        /// <param name="param"></param>
        /// <returns>The median of the data.</returns>
        public Dictionary<string, object> DoOrdinalData(IEnumerable<OrdinalData> values,
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

        public Dictionary<string, object> DoIntervalData(IEnumerable<IntervalData> values,
            params object[] parameters)
        {

            var flattenedInput = Utilities.Flatten(values);

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedInput.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");


            // https://mathlibra.com/calculation-of-percentiles-for-grouped-data/

            // This was taken from the percentile statistic since median is just the 50th percentile.
            var desiredPercentile = .5;

            var totalSize = flattenedInput.TotalSize;

            var locationOfPercentile = totalSize * desiredPercentile;

            var cumulativeFreqs = flattenedInput.CumulativeFrequencies;

            string prevInterval = string.Empty;
            string intervalName = string.Empty;
            foreach (var (name, freq) in cumulativeFreqs)
            {
                if (string.IsNullOrEmpty(intervalName))
                {
                    intervalName = name;
                    continue;
                }

                if (locationOfPercentile > freq)
                {
                    break;
                }

                prevInterval = intervalName;
                intervalName = name;
            }

            var boundsForInterval = flattenedInput.Definitions[intervalName];

            var previousCumulativeFrequency = cumulativeFreqs[prevInterval];

            var currentFrequency = flattenedInput.Frequencies[intervalName];

            var lowerBound = boundsForInterval.Start;

            var boundSize = boundsForInterval.Size;

            var median = lowerBound + (boundSize / currentFrequency) * (desiredPercentile * totalSize - previousCumulativeFrequency);

            return new Dictionary<string, object>
            {
                {
                    "median", median
                }
            };

        }

        public Dictionary<string, object> DoFrequencyData<T>(IEnumerable<FrequencyData<T>> values,
            params object[] parameters)
        {
            FrequencyData<double> flattenedInput;

            try
            {
                flattenedInput = Utilities.Flatten(values) as FrequencyData<double>;
            }
            catch (InvalidOperationException _)
            {
                throw new InvalidOperationException("Cannot perform Mean Statistic on non-numerical data!");
            }

            if (flattenedInput.Count == 0)
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");


            // This was taken from the percentile statistic since median is just the 50th percentile.
            var desiredPercentile = .5;

            var totalSize = flattenedInput.TotalSize;

            var locationOfPercentile = totalSize * desiredPercentile;

            var cumulativeFreqs = flattenedInput.CumulativeFrequencies;

            var previousValue = double.MinValue;
            var desiredValue = double.MinValue;
            foreach (var (name, freq) in cumulativeFreqs)
            {
                if (Math.Abs(desiredValue - double.MinValue) < Tolerance)
                {
                    desiredValue = name;
                    continue;
                }

                if (locationOfPercentile > freq)
                {
                    break;
                }

                previousValue = desiredValue;
                desiredValue = name;
            }


            var previousCumulativeFrequency = cumulativeFreqs[previousValue];

            var currentFrequency = flattenedInput[desiredValue];

            var lowerBound = desiredValue;

            // TODO: this might be wrong.
            var median = lowerBound + (1 / currentFrequency) * (desiredPercentile * totalSize - previousCumulativeFrequency);

            return new Dictionary<string, object>
            {
                {
                    "median", median
                }
            };



        }
    }
}