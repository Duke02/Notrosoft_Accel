using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notrosoft_Accel
{
    /// <summary>
    /// General utility functions to use throughout the project.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Flattens the provided 2D container into a 1D container.
        /// </summary>
        /// <typeparam name="T">The type of data within the container.</typeparam>
        /// <param name="values">The values to flatten.</param>
        /// <returns>The input flattened into a 1D container.</returns>
        public static IEnumerable<T> Flatten<T>(IEnumerable<IEnumerable<T>> values)
        {
            return values.SelectMany(vals => vals);
        }

        public static double GetVariance(IEnumerable<double> values)
        {
            // Return the average/mean of the inputted values.
            var average = flattenedValues.Average();

            var squaredDifferences = flattenedValues
                .Select(val => val - average)           // Calculate the difference from the average.
                .Select(diff => diff * diff);           // Calculate the squared difference

            // Return the average squared difference between the values, or the variance.
            return squaredDifferences.Average();
        }
    }
}
