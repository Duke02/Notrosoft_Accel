using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notrosoft_Accel.Statistics
{
    /// <summary>
    /// Calculates the mathematical mean.
    /// </summary>
    public class MeanStatistic : IStatistic
    {
        /// <summary>
        /// Calculates the mathematical mean (commonly called Average) from the provided data.
        /// </summary>
        /// <param name="values">The 2D data to use in the calculations.</param>
        /// <returns>The mathematical mean of the inputted data.</returns>
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            var flattenedValues = Utilities.Flatten(values);
            return flattenedValues.Average();
        }
    }
}
