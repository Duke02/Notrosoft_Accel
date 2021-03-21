using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class NormalDistributionStatistic : IStatistic
    {
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            var flattenedValues = Utilities.Flatten(values).ToArray();

            if (!flattenedValues.Any())
                throw new ArgumentException("Normal Distribution Statistic requires input data.");

            // Calculate the variance and the mean
            var variance = Utilities.GetVariance(flattenedValues);
            var mean = flattenedValues.Average();

            // TODO: Return both values.
            return mean;
        }
    }
}