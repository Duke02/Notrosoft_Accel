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
            var flattenedValues = Utilities.Flatten(values);

            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            var variance = Utilities.GetGroupedVariance(flattenedValues);
            var mean = Utilities.GetGroupedMean(flattenedValues);

            return new Dictionary<string, object>
            {
                {"mean", mean},
                {"variance", variance}
            };
        }

        public Dictionary<string, object> OperateFrequencyData<T>(IEnumerable<FrequencyData<T>> values,
            params object[] parameters)
        {
            FrequencyData<double> flattenedValues;

            try
            {
                flattenedValues = Utilities.Flatten(values) as FrequencyData<double>;
            }
            catch (InvalidOperationException _)
            {
                throw new InvalidOperationException("Cannot perform Mean Statistic on non-numerical data!");
            }

            if (flattenedValues.Count == 0)
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            var variance = Utilities.GetGroupedVariance(flattenedValues);
            var mean = Utilities.GetGroupedMean(flattenedValues);

            return new Dictionary<string, object>
            {
                {"mean", mean},
                {"variance", variance}
            };
        }
    }
}