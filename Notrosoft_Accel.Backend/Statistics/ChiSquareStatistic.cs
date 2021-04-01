using System;
using System.Collections.Generic;
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


            throw new NotImplementedException();
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