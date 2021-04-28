﻿using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class SpearmanRankCorrelationStatistic : IStatistic
    {
        public Dictionary<string, object> DoOrdinalData(IEnumerable<OrdinalData> values,
            params object[] parameters)
        {
            var concreteValues = values.Select(vals => vals.ToArray()).ToArray();

            if (concreteValues.Length != 2)
                throw new InvalidOperationException(
                    "Spearman Rank Correlation Statistic needs 2 variables to operate on.");

            var xData = concreteValues[0].ToList();
            var yData = concreteValues[1].ToList();

            var xRanks = Utilities.GetRanks(xData)
                .Select(x => (double)x)
                .ToArray();

            var yRanks = Utilities.GetRanks(yData)
                .Select(y => (double)y)
                .ToArray();

            var numerator = Utilities.GetSampleCovariance(xRanks, yRanks);

            var stdDevXRank = Math.Sqrt(Utilities.GetSampleVariance(xRanks));
            var stdDevYRank = Math.Sqrt(Utilities.GetSampleVariance(yRanks));

            var spearman = numerator / stdDevXRank / stdDevYRank;

            return new Dictionary<string, object>
            {
                {"spearman", spearman}
            };
        }

        public Dictionary<string, object> DoIntervalData(IEnumerable<IntervalData> values,
            params object[] parameters)
        {
            var concreteValues = values.ToArray();

            if (concreteValues.Length != 2)
            {
                throw new InvalidOperationException("Spearman Rank Correlation Statistic needs 2 variables to operate on.");
            }

            var x = concreteValues[0];
            var y = concreteValues[1];

            throw new NotImplementedException("Spearman Rank Correlation Coefficient is not implemented as we don't know how to rank non-ordinal data.");

        }

        public Dictionary<string, object> DoFrequencyData<T>(IEnumerable<FrequencyData<T>> values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}