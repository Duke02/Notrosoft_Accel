using Notrosoft_Accel.Data;
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
        public object Operate<T>(GenericData<T> values)
        {
            // Flatten the 2d list of lists into a 1d list.
            var flattenedArray = values.Flatten();

            // Throw an exception if there are no input values.
            if (flattenedArray.Count() <= 0)
            {
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");
            }

            // This is based off of the Counting Sort algorithm,
            // https://en.wikipedia.org/wiki/Counting_sort
            // but instead of having a secondary array, we just calculate 
            // the counts of the input values in a dictionary and 
            // get the max value with the greatest count.
            var distinctNums = flattenedArray.Distinct();
            var countDict = distinctNums.ToDictionary(n => n, n => 0);

            foreach (var num in flattenedArray)
            {
                countDict[num]++;
            }

            return countDict.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }
    }
}
