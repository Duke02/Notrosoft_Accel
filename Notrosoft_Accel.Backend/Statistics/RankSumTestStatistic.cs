using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class RankSumTestStatistic : IStatistic
    {
        private const double Tolerance = .0001;

        public Dictionary<string, object> DoOrdinalData(IEnumerable<OrdinalData> values,
            params double[] parameters)
        {
            var concreteValues = values.ToArray();
            if (concreteValues.Length != 2)
                throw new InvalidOperationException("Input data must be a 2D list with equal values!");

            var xData = concreteValues[0].ToArray();
            var yData = concreteValues[1].ToArray();

            if (xData.Length == 0 || yData.Length == 0)
                throw new InvalidOperationException("Input must have data in both X and Y data.");

            var mannWhitneyUStat = xData.Sum(x => yData.Sum(y => InnerFunction(x, y)));

            return new Dictionary<string, object>
            {
                {"ranksum", mannWhitneyUStat}
            };
        }

        public Dictionary<string, object> DoIntervalData(IEnumerable<IntervalData> values,
            params double[] parameters)
        {
            // TODO: This might be totally wrong ngl.

            var concreteValues = values.ToArray();

            if (concreteValues.Length != 2)
            {
                throw new InvalidOperationException("Input data must be a 2D list!");
            }

            var x = concreteValues[0];
            var y = concreteValues[1];

            if (x.Count == 0 || y.Count == 0)
            {
                throw new InvalidOperationException("Input data must have data in it!");
            }

            //  x.Frequencies.Select(kv => (kv.Value * x.Definitions[kv.Key].GetMidpoint()) - x_mean);

            var weightedXVals = x.Frequencies.Select(kv => kv.Value * x.Definitions[kv.Key].GetMidpoint());
            var weightedYVals = y.Frequencies.Select(kv => kv.Value * y.Definitions[kv.Key].GetMidpoint());

            var mannWhitneyUStat = weightedXVals.Sum(xVal => weightedYVals.Sum(yVal => InnerFunction(xVal, yVal)));

            return new Dictionary<string, object>
            {
                {"ranksum", mannWhitneyUStat}
            };
        }

        public Dictionary<string, object> DoFrequencyData<T>(IEnumerable<FrequencyData<T>> values,
            params double[] parameters)
        {
            // TODO: This might be totally wrong ngl.
            FrequencyData<double>[] concreteValues;

            try
            {
                concreteValues = values.ToArray() as FrequencyData<double>[];
            }
            catch (InvalidCastException _)
            {
                throw new InvalidOperationException("Cannot perform rank sum test on non-numerical data!");
            }

            if (concreteValues.Length != 2)
            {
                throw new InvalidOperationException("Input data must be a 2D list!");
            }

            var x = concreteValues[0];
            var y = concreteValues[1];

            if (x.Count == 0 || y.Count == 0)
            {
                throw new InvalidOperationException("Input data must have data in it!");
            }

            //  x.Frequencies.Select(kv => (kv.Value * x.Definitions[kv.Key].GetMidpoint()) - x_mean);

            var weightedXVals = x.Select(kv => kv.Value * kv.Key);
            var weightedYVals = y.Select(kv => kv.Value * kv.Key);

            var mannWhitneyUStat = weightedXVals.Sum(xVal => weightedYVals.Sum(yVal => InnerFunction(xVal, yVal)));

            return new Dictionary<string, object>
            {
                {"ranksum", mannWhitneyUStat}
            };
        }

        private double InnerFunction(double x, double y)
        {
            return Math.Abs(x - y) < Tolerance ? .5 : y < x ? 1 : 0;
        }
    }
}