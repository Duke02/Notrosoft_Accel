using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class CorrelationCoefficientStatistic : IStatistic
    {
        public Dictionary<string, object> OperateOrdinalData(IEnumerable<IEnumerable<double>> values,
            params object[] parameters)
        {
            var concreteValues = values.ToArray();

            if (concreteValues.Length != 2) throw new InvalidOperationException("Input data must be a 2D array.");

            var xData = concreteValues[0].ToArray();
            var yData = concreteValues[1].ToArray();

            if (xData.Length == 0 || yData.Length == 0)
                throw new InvalidOperationException("Input data must contain data.");

            var covariance = Utilities.GetCovariance(xData, yData);
            var stddevX = Math.Sqrt(Utilities.GetVariance(xData));
            var stddevY = Math.Sqrt(Utilities.GetVariance(yData));

            var pearsonCorrelationCoefficient = covariance / stddevX / stddevY;

            return new Dictionary<string, object>
            {
                {"coeff", pearsonCorrelationCoefficient}
            };
        }

        public Dictionary<string, object> OperateFrequencyData(Dictionary<object, int> values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> OperateIntervalData(IEnumerable<IEnumerable<double>> values,
            Dictionary<string, Range> intervalDefinitions, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}