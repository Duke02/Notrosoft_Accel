using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class SpearmanRankCorrelationStatistic : IStatistic
    {
        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] parameters)
        {
            var concreteValues = values.Select(vals => vals.ToArray()).ToArray();

            if (concreteValues.Length != 2)
                throw new InvalidOperationException(
                    "Spearman Rank Correlation Statistic needs 2 variables to operate on.");

            var xData = concreteValues[0].ToList();
            var yData = concreteValues[1].ToList();

            var xRanks = Utilities.GetRanks(xData)
                .Select(x => (double) x)
                .ToArray();

            var yRanks = Utilities.GetRanks(yData)
                .Select(y => (double) y)
                .ToArray();

            var numerator = Utilities.GetCovariance(xRanks, yRanks);

            var stdDevXRank = Math.Sqrt(Utilities.GetSampleVariance(xRanks));
            var stdDevYRank = Math.Sqrt(Utilities.GetSampleVariance(yRanks));

            var spearman = numerator / stdDevXRank / stdDevYRank;

            return new Dictionary<string, object>
            {
                {"spearman", spearman}
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