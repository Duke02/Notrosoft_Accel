using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class SignTestStatistic : IStatistic
    {
        private const double Tolerance = 0.001;

        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] parameters)
        {
            var concreteValues = values.ToArray();
            if (concreteValues.Length != 2) throw new InvalidOperationException("Input must be a 2D container.");

            var xData = concreteValues[0].ToArray();
            var yData = concreteValues[1].ToArray();

            if (xData.Length == 0 || yData.Length == 0)
                throw new InvalidOperationException("Input data must have elements within it.");
            if (xData.Length != yData.Length)
                throw new InvalidOperationException(
                    "Input data must have the same amount of data between the different variables.");

            var signs = xData.Zip(yData)
                .Select(xy => xy.First - xy.Second)
                .Select(Math.Sign)
                .ToArray();

            var numGreater = signs.Count(s => s >= 0);
            var numTotal = signs.Length;

            var output = Utilities.CumulativeBinomialProbability(numTotal, numGreater, 0.5);

            return new Dictionary<string, object>
            {
                {"sign-prop", output}
            };
        }

        public Dictionary<string, object> OperateIntervalData(IntervalData values,
            params object[] parameters)
        {
            int length = values.Count();
            var flattened = new List<double>();
            foreach (var kv in values)
            {
                flattened.AddRange(kv.Value);
            }
            var flattenedValues = flattened.ToArray();

            if (flattenedValues.Length == 0)
                throw new InvalidOperationException(
                    "There must be input data in order to perform the sign test statistic!");

            var comparisonType = (ComparisonType)parameters[0];
            var value = (double)parameters[1];

            var numSuccesses = comparisonType switch
            {
                ComparisonType.GreaterThan => flattenedValues.Count(v => v > value),
                ComparisonType.LessThan => flattenedValues.Count(v => v < value),
                ComparisonType.EqualTo => flattenedValues.Count(v => Math.Abs(v - value) < Tolerance),
                ComparisonType.GreaterThanOrEqualTo => flattenedValues.Count(v => v >= value),
                ComparisonType.LessThanOrEqualTo => flattenedValues.Count(v => v <= value),
                _ => throw new ArgumentOutOfRangeException()
            };

            var p = 0.5;
            var n = flattenedValues.Length;
            var output = Utilities.BinomialProbability(n, numSuccesses, p);

            return new Dictionary<string, object>
            {
                {"sign-prop", output}
            };
        }

        public Dictionary<string, object> OperateFrequencyData<T>(FrequencyData<T> values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}