using System;
using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Statistics
{
    /// <summary>
    /// </summary>
    public class CoefficientOfVarianceStatistic : IStatistic
    {
        /// <summary>
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <returns></returns>
        public double Operate(IEnumerable<IEnumerable<double>> values)
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
            return stddev / average;
        }
    }
}