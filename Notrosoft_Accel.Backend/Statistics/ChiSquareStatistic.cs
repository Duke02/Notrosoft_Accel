using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class ChiSquareStatistic : IStatistic
    {
        public Dictionary<string, object> PerformStatistic(OrdinalData values,
            params object[] parameters)
        {
            throw new InvalidOperationException("Chi Square Statistic cannot operate on Ordinal Data!");
        }

        public Dictionary<string, object> OperateIntervalData(IEnumerable<IntervalData> values,
            params object[] parameters)
        {
            return OperateFrequencyData(values.Select(v => Utilities.ConvertFromIntervalDataToFrequencyValues<string>(v)), parameters);
        }

        public Dictionary<string, object> OperateFrequencyData<T>(IEnumerable<FrequencyData<T>> values,
            params object[] parameters)
        {
            var flattenedValues = Utilities.Flatten(values);

            if (flattenedValues.Count == 0)
                throw new InvalidOperationException("Chi Square Statistic must be given data in order to operate.");

            // if (parameters.Length != values.Count)
            //     throw new InvalidOperationException(
            //         "Input P Values must be the same length as the number of categories in the input data.");

            // Assumes that each category should be the same probability.
            double totalN =flattenedValues.Sum(kv => kv.Value);
            var testProportion = flattenedValues.Count / totalN;

            var sum = flattenedValues.Sum(observed =>
                Math.Pow(observed.Value / totalN - testProportion, 2) / testProportion);

            var chiSquareStat = sum * totalN;

            return new Dictionary<string, object>
            {
                {"chi-square", chiSquareStat}
            };
        }

        private string GetCategory(double value, Dictionary<string, int> categories)
        {
            throw new NotImplementedException();
        }
    }
}