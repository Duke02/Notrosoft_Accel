using Notrosoft_Accel.Data;
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
        public object Operate<T>(GenericData<T> values)
        {
            // TODO: See if you can have T extend both double and int
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = values.Flatten();

            // Throw an exception if there's nothing in the inputted container.
            if (flattenedValues.Count() <= 0)
            {
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");
            }

            // Return the average/mean of the inputted values.
            return flattenedValues.Average();
        }
    }
}
