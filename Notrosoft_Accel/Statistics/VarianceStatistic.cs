using System;
using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Statistics
{
    /// <summary>
    ///     Essentially a wrapper class for <see cref="Utilities.GetVariance" />
    /// </summary>
    public class VarianceStatistic : IStatistic
    {
        /// <summary>
        ///     Calculates the population variance for the provided values.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <returns>The variation for the inputted values.</returns>
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            return Utilities.GetVariance(flattenedValues);
        }
    }
}