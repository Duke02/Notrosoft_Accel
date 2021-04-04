using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class RankSumTestStatistic : IStatistic
    {
        private const double Tolerance = .0001;

        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] parameters)
        {
            var concreteValues = values.ToArray();
            if (concreteValues.Length != 2)
                throw new InvalidOperationException("Input data must be a 2D list with equal values!");

            var xData = concreteValues[0].ToArray();
            var yData = concreteValues[1].ToArray();

            if (xData.Length == 0 || yData.Length == 0)
                throw new InvalidOperationException("Input must have data in both X and Y data.");

            var mannWhitneyUStat = xData.Sum(x => yData.Sum(y => InnerFunction(x, y)));

            return new Dictionary<string, object>
            {
                {"ranksum", mannWhitneyUStat}
            };
        }

        public Dictionary<string, object> OperateIntervalData(IEnumerable<IEnumerable<double>> values,
            Dictionary<string, Bounds<double>> intervalDefinitions, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> OperateFrequencyData(FrequencyData<T> values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }

        private double InnerFunction(double x, double y)
        {
            return Math.Abs(x - y) < Tolerance ? .5 : y < x ? 1 : 0;
        }
    }
}