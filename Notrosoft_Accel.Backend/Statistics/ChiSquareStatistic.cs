using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class ChiSquareStatistic : IStatistic
    {
        public Dictionary<string, object> Operate(IEnumerable<IEnumerable<double>> values, params object[] parameters)
        {
            var flattenedValues = Utilities.Flatten(values).ToArray();

            if (flattenedValues.Length == 0)
                throw new InvalidOperationException("Chi Square Statistic must be given data in order to operate.");

            var frequencyData = Utilities.ConvertToFrequencyValues(flattenedValues);

            if (parameters.Length != frequencyData.Count)
                throw new InvalidOperationException(
                    "Input P Values must be the same length as the number of categories in the input data.");

            var pValues = parameters.Select(param => (double) param)
                .Zip(frequencyData)
                .ToDictionary(pKv => pKv.Second.Key,
                    pKv => pKv.First);

            var degreesOfFreedom = frequencyData.Count - 1;

            var sum = 0.0;
            var n = flattenedValues.Length;

            foreach (var value in flattenedValues)
            {
                var category = GetCategory(value, frequencyData);
                var numerator = Math.Pow(value / n - pValues[category], 2);
                sum += numerator / pValues[category];
            }

            return new Dictionary<string, object>
            {
                {"chi-square", sum}
            };
        }

        private string GetCategory(double value, Dictionary<string, int> categories)
        {
            throw new NotImplementedException();
        }
    }
}