using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Essentially a wrapper class for <see cref="Utilities.GetVariance" />
    /// </summary>
    public class VarianceStatistic : IStatistic
    {
        /// <summary>
        ///     Calculates the population variance for the provided values.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <param name="param"></param>
        /// <returns>The variation for the inputted values.</returns>
        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] param)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            var variance = Utilities.GetVariance(flattenedValues);

            return new Dictionary<string, object>
            {
                {"variance", variance}
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

            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            var variance = Utilities.GetVariance(flattenedValues);

            return new Dictionary<string, object>
            {
                {"variance", variance}
            };
        }

        public Dictionary<string, object> OperateFrequencyData<T>(FrequencyData<T> values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}