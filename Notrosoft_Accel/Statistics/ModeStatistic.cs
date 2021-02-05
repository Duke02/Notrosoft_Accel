using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notrosoft_Accel.Statistics
{
    /// <summary>
    /// Statistic class that calculates the mode of inputted data. 
    /// </summary>
    public class ModeStatistic : IStatistic
    {
        /// <summary>
        /// Calculates the mode (most common number) of the inputted data.
        /// </summary>
        /// <param name="values">A 2D data collection of numbers.</param>
        /// <returns>The most common number of the inputted data.</returns>
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            // Flatten the 2d list of lists into a 1d list.
            var flattenedArray = Utilities.Flatten(values);

            // Throw an exception if there are no input values.
            if (flattenedArray.Count() <= 0)
            {
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");
            }

            // Create a dictionary that holds the value of the input as the key
            // and the number of times the key occurs in the input.
            // Then order it based on the counts.
            var dictCount = flattenedArray.Distinct()
                .ToDictionary(val => val, val => flattenedArray.Count(v => v == val))
                .OrderByDescending(kv => kv.Value);
            // Return the element with the largest value.
            return dictCount.First().Key;
        }
    }
}
