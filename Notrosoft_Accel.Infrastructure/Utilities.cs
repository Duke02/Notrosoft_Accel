using System;
using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Infrastructure
{
    /// <summary>
    ///     General utility functions to use throughout the project.
    /// </summary>
    public static class Utilities
    {
        // https://cs.stackexchange.com/a/14457
        // This value is +1 than the actual max.
        /// <summary>
        ///     The max input that the <see cref="Factorial" /> function can take. It's 1 more than the actual limit, so use i
        ///     < MaxFactorialInput.
        /// </summary>
        private const int MaxFactorialInput = 171;

        private static double[] _factorials;

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

        public static double Factorial(int n)
        {
            // If the inputted required is too big, then don't even try to compute it.
            if (n > MaxFactorialInput)
                throw new ArgumentOutOfRangeException(
                    $"Factorial function can only process {MaxFactorialInput}! without running out of space.");

            // If we've already calculated all of the possible factorials,
            // don't recalculate them.
            if (_factorials != null) return _factorials[n];

            // Calculate the factorials for the first time.
            _factorials = new double[MaxFactorialInput];

            // 0! = 1, 1! = 1
            // (Ngl I had to look this up)
            _factorials[0] = 1;
            _factorials[1] = 1;

            // n! = n * (n-1)!
            for (var i = 2; i < MaxFactorialInput; i++) _factorials[i] = i * _factorials[i - 1];

            // Return the requested factorial.
            return _factorials[n];
        }
    }
}