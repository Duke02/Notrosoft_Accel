using System;
using System.Collections.Generic;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class SpearmanRankCorrelationStatistic : IStatistic
    {
        public Dictionary<string, object> Operate(IEnumerable<IEnumerable<double>> values, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}