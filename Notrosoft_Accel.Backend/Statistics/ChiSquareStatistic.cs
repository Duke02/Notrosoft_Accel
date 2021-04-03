using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class ChiSquareStatistic : IStatistic
    {
        public Dictionary<string, object> OperateOrdinalData(IEnumerable<IEnumerable<double>> values,
            params object[] parameters)
        {
            throw new InvalidOperationException("Chi Square Statistic cannot operate on Ordinal Data!");
        }

        public Dictionary<string, object> OperateFrequencyData(Dictionary<object, int> values,
            params object[] parameters)
        {
            if (values.Count == 0)
                throw new InvalidOperationException("Chi Square Statistic must be given data in order to operate.");

            if (parameters.Length != values.Count)
                throw new InvalidOperationException(
                    "Input P Values must be the same length as the number of categories in the input data.");

            // Assumes that each category should be the same probability.
            double totalN = values.Sum(kv => kv.Value);
            var testProportion = values.Count / totalN;

            var sum = values.Sum(observed =>
                Math.Pow(observed.Value / totalN - testProportion, 2) / testProportion);

            var chiSquareStat = sum * totalN;


            return new Dictionary<string, object>
            {
                {"chi-square", chiSquareStat}
            };
        }

        public Dictionary<string, object> OperateIntervalData(IEnumerable<IEnumerable<double>> values,
            Dictionary<string, Range> intervalDefinitions, params object[] parameters)
        {
            var intervalData = Utilities.ConvertToIntervalData(Utilities.Flatten(values),
                intervalDefinitions);
            return OperateFrequencyData(Utilities.ConvertFromIntervalDataToFrequencyValues(intervalData), parameters);
        }

        private string GetCategory(double value, Dictionary<string, int> categories)
        {
            throw new NotImplementedException();
        }
    }
}