using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the coefficient of variance statistic.
    /// </summary>
    public class CoefficientOfVarianceStatistic : IStatistic
    {
        /// <summary>
        ///     Calculates the coefficient of variance statistic for the provided data.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <param name="param"></param>
        /// <returns>The CV of the data.</returns>
        public Dictionary<string, object> DoOrdinalData(IEnumerable<OrdinalData> values,
            params object[] param)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Coefficient of variance is the stddev / mean.
            // So calculate the mean
            var average = flattenedValues.Average();
            // Then calculate the standard deviation.
            var stddev = Math.Sqrt(Utilities.GetVariance(flattenedValues));

            // And return their ratio.
            return new Dictionary<string, object>
            {
                {"cv", stddev / average}
            };
        }

        public Dictionary<string, object> DoIntervalData(IEnumerable<IntervalData> values,
            params object[] parameters)
        {
            var flattenedValues = Utilities.Flatten(values);

            if (flattenedValues.Count <= 0)
                throw new InvalidOperationException("Inputted values must have a count greater than 0!");

            var groupedAverage = Utilities.GetGroupedMean(flattenedValues);
            var groupedVariance = Utilities.GetGroupedVariance(flattenedValues);

            return new Dictionary<string, object>
            {
                {"cv", Math.Sqrt(groupedVariance) / groupedAverage}
            };
        }

        public Dictionary<string, object> DoFrequencyData<T>(IEnumerable<FrequencyData<T>> values,
            params object[] parameters)
        {
            throw new InvalidOperationException("Coefficient Of Variance cannot be performed on Frequency Data!");
        }
    }
}