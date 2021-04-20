using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class BinomialDistributionStatistic : IStatistic
    {
        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] parameters)
        {
            // https://en.wikipedia.org/wiki/Binomial_test
            var flattenedValues = Utilities.Flatten(values).ToArray();

            if (flattenedValues.Length == 0)
                throw new InvalidOperationException("Input data needs to have values in it to operate!");

            var hypothesis = (double) parameters[0];
            var probOfSuccess = (double) parameters[1];

            var confidence = parameters.Length > 2 ? (double) parameters[2] : .95;

            var n = flattenedValues.Length;
            var numSuccesses = flattenedValues.Count(v => v < hypothesis);

            var pValue = Enumerable.Range(0, numSuccesses + 1)
                .Sum(i => Utilities.BinomialProbability(n, i, probOfSuccess));
            var shouldRejectNullHypothesis = Utilities.ShouldRejectNullHypothesis(pValue, confidence);
            return new Dictionary<string, object>
            {
                {"P-Value", pValue},
                {"Reject Null Hypothesis?", shouldRejectNullHypothesis}
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
                throw new InvalidOperationException("Input data needs to have values in it to operate!");

            var hypothesis = (double)parameters[0];
            var probOfSuccess = (double)parameters[1];

            var confidence = parameters.Length > 2 ? (double)parameters[2] : .95;

            var n = flattenedValues.Length;
            var numSuccesses = flattenedValues.Count(v => v < hypothesis);

            var pValue = Enumerable.Range(0, numSuccesses + 1)
                .Sum(i => Utilities.BinomialProbability(n, i, probOfSuccess));
            var shouldRejectNullHypothesis = Utilities.ShouldRejectNullHypothesis(pValue, confidence);
            return new Dictionary<string, object>
            {
                {"P-Value", pValue},
                {"Reject Null Hypothesis?", shouldRejectNullHypothesis}
            };
        }

        public Dictionary<string, object> OperateFrequencyData<T>(FrequencyData<T> values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}