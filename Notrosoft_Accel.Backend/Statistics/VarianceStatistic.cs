using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Essentially a wrapper class for <see cref="Utilities.GetVariance" />
    /// </summary>
    public class VarianceStatistic : Statistic
    {
        /// <summary>
        ///     Calculates the population variance for the provided values.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <returns>The variation for the inputted values.</returns>
        public override double Operate(IEnumerable<IEnumerable<double>> values)
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