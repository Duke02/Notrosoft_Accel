using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the standard deviation statistic.
    /// </summary>
    public class StandardDeviationStatistic : IStatistic
    {
        /// <summary>
        ///     Calculates the population standard deviation of the inputted data.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <param name="param"></param>
        /// <returns>The standard deviation of the data.</returns>
        public Dictionary<string, object> DoOrdinalData(IEnumerable<OrdinalData> values,
            params double[] param)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // The standard deviation is just the square root of the variance. 
            var stddev = Math.Sqrt(Utilities.GetVariance(flattenedValues));

            return new Dictionary<string, object>
            {
                {"Standard Deviation", stddev}
            };
        }

        public Dictionary<string, object> DoIntervalData(IEnumerable<IntervalData> values,
            params double[] parameters)
        {

            var flattenedValues = Utilities.Flatten(values);

            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // The standard deviation is just the square root of the variance. 
            var stddev = Math.Sqrt(Utilities.GetGroupedVariance(flattenedValues));

            return new Dictionary<string, object>
            {
                {"Standard Deviation", stddev}
            };
        }

        public Dictionary<string, object> DoFrequencyData<T>(IEnumerable<FrequencyData<T>> values,
            params double[] parameters)
        {
            FrequencyData<double> flattenedValues;

            try
            {
                flattenedValues = Utilities.Flatten(values) as FrequencyData<double>;
            }
            catch (InvalidOperationException _)
            {
                throw new InvalidOperationException("Cannot perform Mean Statistic on non-numerical data!");
            }

            if (flattenedValues.Count == 0)
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            var stddev = Math.Sqrt(Utilities.GetGroupedVariance(flattenedValues));

            return new Dictionary<string, object>
            {
                {"Standard Deviation", stddev}
            };
        }
    }
}