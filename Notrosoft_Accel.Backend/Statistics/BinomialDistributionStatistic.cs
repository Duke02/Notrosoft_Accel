using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class BinomialDistributionStatistic : IStatistic
    {
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            var flattenedValues = Utilities.Flatten(values).ToArray();

            if (!flattenedValues.Any())
                throw new ArgumentException("Binomial Distribution requires input data to operate.");

            return 0;
        }
    }
}