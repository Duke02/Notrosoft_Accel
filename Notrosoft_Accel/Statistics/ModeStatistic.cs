using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notrosoft_Accel.Statistics
{
    class ModeStatistic : IStatistic
    {
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            // Flatten the 2d list of lists into a 1d list.
            var flattenedArray = Utilities.Flatten(values);
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
