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
        /// <returns>The standard deviation of the data.</returns>
        public Dictionary<string, object> Operate(IEnumerable<IEnumerable<double>> values, params object[] param)
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
                {"stdev", stddev}
            };
        }
    }
}