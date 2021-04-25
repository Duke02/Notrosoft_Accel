using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the percentile for the provided data.
    /// </summary>
    public class PercentileStatistic : IStatistic
    {
        private const double Tolerance = 0.001;

        /// <summary>
        ///     Calculates the percentile for the provided data.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <param name="param"></param>
        /// <returns>The requested percentile for the inputted data.</returns>
        public Dictionary<string, object> PerformStatistic(IEnumerable<OrdinalData> values,
            params object[] param)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Sort the values from least to greatest.
            var sortedValues = flattenedValues.OrderBy(val => val).ToArray();

            var desiredPercentile = (double)param[0];

            var indexOfPercentile = desiredPercentile * sortedValues.Length;

            double percentile;

            var isCleanInt = Math.Abs(indexOfPercentile - (int)indexOfPercentile) < 0.001;
            if (isCleanInt)
            {
                percentile = sortedValues[(int)indexOfPercentile];
            }
            else
            {
                // Need to figure out the closest indexes and the weight I have to do between them.
                var lowerIndex = (int)indexOfPercentile;
                var upperIndex = (int)(indexOfPercentile + 1);

                // The greater the weight, the closer the value
                // should be towards the element at the upper index.
                // Example: IndexOfPercentile is 2.6? weight is .6 
                // and the upperIndex should be weighted more heavily.
                var weight = indexOfPercentile - (int)indexOfPercentile;
                percentile = sortedValues[upperIndex] * weight + sortedValues[lowerIndex] * (1 - weight);
            }

            return new Dictionary<string, object>
            {
                {"percentile", percentile}
            };
        }

        public Dictionary<string, object> PerformStatistic(IEnumerable<IntervalData> values,
            params object[] parameters)
        {
            var flattenedValues = Utilities.Flatten(values);

            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            var desiredPercentile = (double)parameters[0];

            // https://mathlibra.com/calculation-of-percentiles-for-grouped-data/

            var totalSize = flattenedValues.TotalSize;

            var locationOfPercentile = totalSize * desiredPercentile;

            var cumulativeFreqs = flattenedValues.CumulativeFrequencies;

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

            var boundsForInterval = flattenedValues.Definitions[intervalName];

            var previousCumulativeFrequency = cumulativeFreqs[prevInterval];

            var currentFrequency = flattenedValues.Frequencies[intervalName];

            var lowerBound = boundsForInterval.Start;

            var boundSize = boundsForInterval.Size;

            var percentile = lowerBound + (boundSize / currentFrequency) * (desiredPercentile * totalSize - previousCumulativeFrequency);

            return new Dictionary<string, object>
            {
                {"percentile", percentile}
            };
        }

        public Dictionary<string, object> PerformStatistic<T>(IEnumerable<FrequencyData<T>> values,
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
            var desiredPercentile = (double)parameters[0];

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

            var percentile = lowerBound + (1 / currentFrequency) * (desiredPercentile * totalSize - previousCumulativeFrequency);

            return new Dictionary<string, object>
            {
                {
                    "percentile", percentile
                }
            };
        }
    }
}