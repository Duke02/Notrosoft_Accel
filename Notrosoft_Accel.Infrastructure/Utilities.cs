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
        public const int MaxFactorialInput = 171;

        private const double Tolerance = 0.001;

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
        ///     Gets the sample covariance between the two inputted data collections.
        /// </summary>
        /// <param name="xValues">The first data collection.</param>
        /// <param name="yValues">The second data collection.</param>
        /// <returns>The scalar covariance value of the inputted data.</returns>
        public static double GetCovariance(IEnumerable<double> xValues, IEnumerable<double> yValues)
        {
            var yValsArray = yValues.ToArray();
            var xValsArray = xValues.ToArray();

            if (xValsArray.Length != yValsArray.Length)
                throw new InvalidOperationException("X and Y values must be the same length to compute covariance!");
            if (xValsArray.Length == 0 || yValsArray.Length == 0)
                throw new InvalidOperationException("X and Y values must have data to compute covariance!");

            var xMean = xValsArray.Average();
            var xDiffFromMean = xValsArray.Select(x => x - xMean).ToArray();

            var yMean = yValsArray.Average();
            var yDiffFromMean = yValsArray.Select(y => y - yMean).ToArray();

            var numerator = xDiffFromMean.Zip(yDiffFromMean).Sum(xyDiff => xyDiff.First * xyDiff.Second);
            return numerator / (xValsArray.Length - 1);
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

        public static double Combination(int top, int bottom)
        {
            if (bottom > top)
                throw new InvalidOperationException(
                    "Top of combination must be greater than the bottom of the combination.");

            return Factorial(top) / Factorial(bottom) / Factorial(top - bottom);
        }

        public static double BinomialProbability(int n, int k, double p)
        {
            return Combination(n, k) * Math.Pow(p, k) * Math.Pow(1 - p, n - k);
        }

        /// <summary>
        ///     Converts the flattened ordinal data to Frequency data.
        /// </summary>
        /// <param name="flattenedValues">The flattened ordinal data to convert</param>
        /// <returns>The converted frequency data.</returns>
        public static Dictionary<object, int> ConvertToFrequencyValues(IEnumerable<object> flattenedValues)
        {
            var concreteValues = flattenedValues as object[] ?? flattenedValues.ToArray();
            return concreteValues.Distinct().ToDictionary(k => k,
                v => concreteValues.Count(cv => cv == v));
        }

        /// <summary>
        ///     Converts the interval data to frequency data.
        /// </summary>
        /// <param name="intervalData">The interval data to convert.</param>
        /// <returns>The converted frequency data.</returns>
        public static Dictionary<object, int> ConvertFromIntervalDataToFrequencyValues(
            Dictionary<string, IEnumerable<double>> intervalData)
        {
            return intervalData.ToDictionary(kv => kv.Key as object,
                kv => kv.Value.Count());
        }

        /// <summary>
        ///     Converts the flattened ordinal values to interval data with the given range definitions.
        /// </summary>
        /// <param name="flattenedValues">The ordinal data to convert.</param>
        /// <param name="intervalDefinitions">The definitions of the intervals.</param>
        /// <returns>The converted interval data.</returns>
        public static Dictionary<string, IEnumerable<double>> ConvertToIntervalData(IEnumerable<double> flattenedValues,
            Dictionary<string, Bounds<double>> intervalDefinitions)
        {
            return intervalDefinitions.ToDictionary(kv => kv.Key,
                kv => flattenedValues.Where(v => kv.Value.IsWithinBounds(v)));
        }

        /// <summary>
        ///     Converts the interval data to flattened ordinal data.
        /// </summary>
        /// <param name="intervalData">The interval data to convert.</param>
        /// <returns>The converted ordinal data as a 1d container.</returns>
        public static IEnumerable<double> ConvertFromIntervalDataToOrdinalData(
            Dictionary<string, IEnumerable<double>> intervalData)
        {
            return intervalData.SelectMany(kv => kv.Value);
        }

        /// <summary>
        ///     Checks if the null hypothesis should be rejected or if it is failed to be rejected.
        /// </summary>
        /// <param name="pValue">The p value that was computed previously and is to be used to check against the confidence.</param>
        /// <param name="confidence">The confidence of the check.</param>
        /// <returns>True if the null hypothesis is to be rejected, False if it is failed to be rejected.</returns>
        public static bool ShouldRejectNullHypothesis(double pValue, double confidence = 0.95)
        {
            return pValue < 1 - confidence;
        }
    }
}