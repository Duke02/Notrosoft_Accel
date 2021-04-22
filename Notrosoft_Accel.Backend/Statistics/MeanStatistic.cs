using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the mathematical mean.
    /// </summary>
    public class MeanStatistic : IStatistic
    {
        /// <summary>
        ///     Calculates the mathematical mean (commonly called Average) from the provided data.
        /// </summary>
        /// <param name="values">The 2D data to use in the calculations.</param>
        /// <param name="param"></param>
        /// <returns>The mathematical mean of the inputted data.</returns>
        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] param)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Return the average/mean of the inputted values.
            return new Dictionary<string, object>
            {
                {"mean", flattenedValues.Average()}
            };
        }

        public Dictionary<string, object> OperateIntervalData(IEnumerable<IntervalData> values,
            params object[] parameters)
        {
            var flattenedValues = Utilities.Flatten(values);

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Return the average/mean of the inputted values.
            return new Dictionary<string, object>
            {
                {"mean", Utilities.GetGroupedMean(flattenedValues)}
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

            var totalSize = 0;
            var sum = 0;

            foreach (var (val, count) in flattenedValues)
            {
                sum += (int)val * count;
                totalSize += count;
            }

            return new Dictionary<string, object>
            {
                {"mean",  sum / totalSize }
            };
        }
    }
}