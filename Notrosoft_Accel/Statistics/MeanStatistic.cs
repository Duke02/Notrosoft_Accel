using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notrosoft_Accel.Statistics
{
    public class MeanStatistic : IStatistic
    {
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            var flattenedValues = Utilities.Flatten(values);
            return flattenedValues.Average();
        }
    }
}
