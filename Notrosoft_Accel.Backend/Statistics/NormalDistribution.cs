using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class NormalDistribution : IStatistic
    {
        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] parameters)
        {
            var flattenedValues = Utilities.Flatten(values).ToArray();

            if (!flattenedValues.Any())
                throw new ArgumentException("Normal Distribution Statistic requires input data.");

            // Calculate the variance and the mean
            var variance = Utilities.GetVariance(flattenedValues);
            var mean = flattenedValues.Average();

            // TODO: Return both values.
            return new Dictionary<string, object>
            {
                {"mean", mean},
                {"variance", variance}
            };
        }

        public Dictionary<string, object> OperateIntervalData(IEnumerable<IntervalData> values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> OperateFrequencyData<T>(IEnumerable<FrequencyData<T>> values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}