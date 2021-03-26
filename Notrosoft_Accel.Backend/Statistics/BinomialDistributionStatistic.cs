using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class BinomialDistributionStatistic : IStatistic
    {
        public Dictionary<string, object> Operate(IEnumerable<IEnumerable<double>> values, params object[] parameters)
        {
            // https://en.wikipedia.org/wiki/Binomial_test
            var flattenedValues = Utilities.Flatten(values).ToArray();

            if (flattenedValues.Length == 0)
                throw new InvalidOperationException("Input data needs to have values in it to operate!");

            var hypothesis = (double) parameters[0];

            var n = flattenedValues.Length;
            var k = flattenedValues.Count(v => v < hypothesis);

            var p = Enumerable.Range(0, k).Sum(i => Utilities.BinomialProbability(n, i, hypothesis));
            return new Dictionary<string, object>
            {
                {"prob", p}
            };
        }
    }
}