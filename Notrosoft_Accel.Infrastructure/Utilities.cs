using System;
using System.Collections;
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

        public static IntervalData Flatten(IEnumerable<IntervalData> values)
        {
            var data = values.SelectMany(v => v.Values).SelectMany(v => v);
            var definitions = values.First(v => v.Definitions != null).Definitions;
            return ConvertToIntervalData(data, definitions);
        }

        public static FrequencyData<T> Flatten<T>(IEnumerable<FrequencyData<T>> values)
        {
            return new FrequencyData<T>(values.SelectMany(v => v).ToDictionary(kv => kv.Key, kv => kv.Value));
        }

        public static IEnumerable<INotrosoftData> ConvertData(IEnumerable<IEnumerable<string>> data, DataType dataType, int numColumns,
            IntervalDefinitions definitions = null)
        {
            return dataType switch
            {
                DataType.Ordinal => OrdinalData.Convert(data, numColumns),
                DataType.Interval => IntervalData.Convert(data, definitions),
                DataType.Frequency => FrequencyData<double>.ConvertDouble(data, numColumns),
                _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
            };
        }

        public static IEnumerable<IEnumerable<double>> ParseForDoubles(IEnumerable<IEnumerable<string>> input)
        {
            var output = new List<List<double>>();
            var concreteData = input.Select(i => i.ToArray()).ToArray();

            foreach (var col in concreteData)
            {
                var tempArray = new List<double>(col.Length);

                foreach (var cellStr in col)
                {
                    if (!double.TryParse(cellStr, out var parsedVal))
                    {
                        throw new InvalidOperationException("Cannot parse data for Ordinal data.");
                    }

                    tempArray.Add(parsedVal);
                }

                output.Add(new List<double>(tempArray));
            }

            return output;
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

        public static double GetSampleVariance(IEnumerable<double> values)
        {
            var valuesArray = values.ToArray();
            var average = valuesArray.Average();

            var n = valuesArray.Length;

            var sumSquaredDifferences = valuesArray
                .Select(x => x - average)
                .Select(xDiff => xDiff * xDiff)
                .Sum();

            return sumSquaredDifferences / (n - 1);
        }

        public static double GetMidpoint(IEnumerable<double> data)
        {
            var concreteData = data.ToArray();
            return (concreteData.Max() + concreteData.Min()) / 2.0;
        }

        public static IEnumerable<double> GetWeightedValues(FrequencyData<double> freqData)
        {
            return freqData.Select(kv => kv.Key * kv.Value);
        }

        public static double GetGroupedMean(IntervalData intervalData)
        {
            // Get the total size of the sample.
            var n = intervalData.Sum(kv => kv.Value.Count());
            // Get each midpoint for the bounds of the interval.
            var midpoints = intervalData.Definitions.ToDictionary(kv => kv.Key,
                kv => kv.Value.GetMidpoint());
            // Get the number of occurrences of each interval.
            var frequencies = intervalData.Select(kv => (kv.Key, kv.Value.Count()));

            // Sum up the weighted values.
            var sum = 0.0;
            foreach (var (intervalName, frequency) in frequencies) sum += frequency * midpoints[intervalName];

            // Return the average.
            return sum / n;
        }

        public static double GetGroupedMean(FrequencyData<double> frequencyData)
        {
            // https://www.mathsisfun.com/data/mean-frequency-table.html
            return GetWeightedValues(frequencyData).Sum() / frequencyData.TotalSize;
        }

        public static double GetGroupedVariance(IntervalData intervalData)
        {
            // https://ncalculators.com/statistics/grouped-data-standard-deviation-calculator.html
            var n = intervalData.Sum(kv => kv.Value.Count());
            var squaredMidpoints = intervalData.Definitions.ToDictionary(
                kv => kv.Key,
                kv => Math.Pow(kv.Value.GetMidpoint(), 2.0)
            );

            var meanSquared = Math.Pow(GetGroupedMean(intervalData), 2.0);
            var frequencies = intervalData.Select(kv => (kv.Key, kv.Value.Count()));

            var sum = 0.0;

            foreach (var (name, freq) in frequencies) sum += freq * squaredMidpoints[name];

            return (sum - n * meanSquared) / (n - 1);
        }

        public static double GetGroupedVariance(FrequencyData<double> freqData)
        {
            // https://www150.statcan.gc.ca/n1/edu/power-pouvoir/ch12/5214891-eng.htm Example 2
            var mean = GetGroupedMean(freqData);

            return (1.0 / freqData.TotalSize) * freqData
                .Select(kv => (Diff: (kv.Key - mean) * (kv.Key - mean), Freq: kv.Value))
                .Sum(kv => kv.Diff * kv.Freq);
        }

        public static double GetGroupedCovariance(IntervalData x, IntervalData y)
        {
            var x_N = x.TotalSize;
            var y_N = y.TotalSize;

            // TODO: They might be able to have the same total size? Not sure.
            if (x_N != y_N || x.Count != y.Count)
            {
                throw new InvalidOperationException("Cannot get grouped covariance of variables of different lengths!");
            }

            var x_mean = GetGroupedMean(x);
            var y_mean = GetGroupedMean(y);

            var x_diffs = x.Frequencies.Select(kv => (kv.Value * x.Definitions[kv.Key].GetMidpoint()) - x_mean);
            var y_diffs = y.Frequencies.Select(kv => (kv.Value * y.Definitions[kv.Key].GetMidpoint()) - y_mean);

            return x_diffs.Zip(y_diffs).Sum(xy => xy.First * xy.Second) / (x_N - 1.0);
        }

        public static double GetGroupedCovariance(FrequencyData<double> x, FrequencyData<double> y)
        {
            if (x.Count != y.Count)
            {
                throw new InvalidOperationException("Cannot get grouped covariance of variables of different lengths!");
            }

            var n = x.TotalSize;

            var x_mean = GetGroupedMean(x);
            var y_mean = GetGroupedMean(y);

            var x_diffs = x.Select(f => (f.Key - x_mean) * f.Value);
            var y_diffs = y.Select(f => (f.Key - y_mean) * f.Value);

            return x_diffs.Zip(y_diffs).Sum(xy => xy.First * xy.Second) / (n - 1.0);
        }

        /// <summary>
        ///     Gets the sample covariance between the two inputted data collections.
        /// </summary>
        /// <param name="xValues">The first data collection.</param>
        /// <param name="yValues">The second data collection.</param>
        /// <returns>The scalar covariance value of the inputted data.</returns>
        public static double GetSampleCovariance(IEnumerable<double> xValues, IEnumerable<double> yValues)
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


        public static double BinomialProbability(int nTotal, int nSuccesses, double probOfSuccess)
        {
            return Combination(nTotal, nSuccesses) * Math.Pow(probOfSuccess, nSuccesses) *
                   Math.Pow(1 - probOfSuccess, nTotal - nSuccesses);
        }

        public static double CumulativeBinomialProbability(int nTotal, int nSuccesses, double probOfSuccess)
        {
            return Enumerable.Range(0, nSuccesses + 1).Sum(i => BinomialProbability(nTotal, i, probOfSuccess));
        }

        public static IEnumerable<int> GetRanks(IEnumerable<double> data)
        {
            var concreteData = data.ToList();

            return concreteData.Select(d => concreteData.Count(cd => cd < d)).ToArray();
        }

        /// <summary>
        ///     Converts the flattened ordinal data to Frequency data.
        /// </summary>
        /// <param name="flattenedValues">The flattened ordinal data to convert</param>
        /// <returns>The converted frequency data.</returns>
        public static FrequencyData<T> ConvertToFrequencyValues<T>(IEnumerable<T> flattenedValues)
        {
            var concreteValues = flattenedValues as T[] ?? flattenedValues.ToArray();
            return new FrequencyData<T>(concreteValues.Distinct().ToDictionary(k => k,
                v => concreteValues.Count(cv => Equals(cv, v))));
        }

        /// <summary>
        ///     Converts the interval data to frequency data.
        /// </summary>
        /// <param name="intervalData">The interval data to convert.</param>
        /// <returns>The converted frequency data.</returns>
        public static FrequencyData<T> ConvertFromIntervalDataToFrequencyValues<T>(IntervalData intervalData)
            where T : class
        {
            return new(intervalData.ToDictionary(kv => kv.Key as T,
                kv => kv.Value.Count()));
        }

        /// <summary>
        ///     Converts the flattened ordinal values to interval data with the given range definitions.
        /// </summary>
        /// <param name="flattenedValues">The ordinal data to convert.</param>
        /// <param name="intervalDefinitions">The definitions of the intervals.</param>
        /// <returns>The converted interval data.</returns>
        public static IntervalData ConvertToIntervalData(IEnumerable<double> flattenedValues,
            IntervalDefinitions intervalDefinitions)
        {
            return new(intervalDefinitions.ToDictionary(kv => kv.Key,
                kv => flattenedValues.Where(v => kv.Value.IsWithinBounds(v))));
        }

        /// <summary>
        ///     Converts the interval data to flattened ordinal data.
        /// </summary>
        /// <param name="intervalData">The interval data to convert.</param>
        /// <returns>The converted ordinal data as a 1d container.</returns>
        public static IEnumerable<double> ConvertFromIntervalDataToOrdinalData(IntervalData intervalData)
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