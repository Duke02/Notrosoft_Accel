using System;
using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Statistics
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
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // The standard deviation is just the square root of the variance. 
            return Math.Sqrt(Utilities.GetVariance(flattenedValues));
        }
    }
}