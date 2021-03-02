using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Infrastructure
{
    /// <summary>
    ///     General utility functions to use throughout the project.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        ///     Flattens the provided 2D container into a 1D container.
        /// </summary>
        /// <typeparam name="T">The type of data within the container.</typeparam>
        /// <param name="values">The values to flatten.</param>
        /// <returns>The input flattened into a 1D container.</returns>
        public static IEnumerable<T> Flatten<T>(IEnumerable<IEnumerable<T>> values)
        {
            return values.SelectMany(vals => vals);
        }

        /// <summary>
        ///     Gets the population variance of the given data.
        /// </summary>
        /// <param name="values">The data to calculate the variance over.</param>
        /// <returns>The population variance of the data.</returns>
        public static double GetVariance(IEnumerable<double> values)
        {
            var valuesArray = values.ToArray();
            // Return the average/mean of the inputted values.
            var average = valuesArray.Average();

            var squaredDifferences = valuesArray
                .Select(val => val - average) // Calculate the difference from the average.
                .Select(diff => diff * diff); // Calculate the squared difference

            // Return the average squared difference between the values, or the variance.
            return squaredDifferences.Average();
        }

        /// <summary>
        ///     Gets the covariance between the two inputted data collections.
        /// </summary>
        /// <param name="xValues">The first data collection.</param>
        /// <param name="yValues">The second data collection.</param>
        /// <returns>The scalar covariance value of the inputted data.</returns>
        public static double GetCovariance(IEnumerable<double> xValues, IEnumerable<double> yValues)
        {
            // Covariance = SumOf(x_i * y_i) - 1 / n * SumOf(x_i) * SumOf(y_i)
            // x_i = ith element of x
            // y_i = ith element of y
            // n = count of elements in x (must be same as count of elements in y)
            var yValuesArray = yValues.ToArray();
            var xValuesArray = xValues.ToArray();
            var xyProducts = xValuesArray.Zip(yValuesArray).Select(xy => xy.Item1 * xy.Item2);
            var sumOfX = xValuesArray.Sum();
            var sumOfY = yValuesArray.Sum();
            double n = xValuesArray.Length;

            return xyProducts.Sum() - 1 / n * sumOfX * sumOfY;
        }
    }
}