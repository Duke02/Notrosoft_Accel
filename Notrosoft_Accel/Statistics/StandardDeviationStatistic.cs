using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Notrosoft_Accel.Statistics
{
    /// <summary>
    /// The base interface for statistics that all statistics are to inherit from.
    /// </summary>
    public class StandardDeviationStatistic : IStatistic
    {
        /// <summary>
        /// Base method that all statistics are to override to use for their statistics.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <returns>The statistic for the child class.</returns>
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values);

            // Throw an exception if there's nothing in the inputted container.
            if(flattenedValues.Count() <= 0)
            {
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");
            }

            // The standard deviation is just the square root of the variance. 
            return Math.Sqrt(Utilities.GetVariance(flattenedValues));
        }
    }
}