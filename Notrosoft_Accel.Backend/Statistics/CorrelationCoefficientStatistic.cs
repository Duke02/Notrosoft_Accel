using System;
using System.Collections.Generic;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    public class CorrelationCoefficientStatistic : IStatistic
    {
        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] parameters)
        {
            var concreteValues = values.ToArray();

            if (concreteValues.Length != 2) throw new InvalidOperationException("Input data must be a 2D array.");

            var xData = concreteValues[0].ToArray();
            var yData = concreteValues[1].ToArray();

            if (xData.Length == 0 || yData.Length == 0)
                throw new InvalidOperationException("Input data must contain data.");

            if (xData.Length != yData.Length)
                throw new InvalidOperationException("Input data must be the same length!");

            var covariance = Utilities.GetCovariance(xData, yData);
            var stddevX = Math.Sqrt(Utilities.GetSampleVariance(xData));
            var stddevY = Math.Sqrt(Utilities.GetSampleVariance(yData));

            var pearsonCorrelationCoefficient = covariance / stddevX / stddevY;

            return new Dictionary<string, object>
            {
                {"coeff", pearsonCorrelationCoefficient}
            };
        }

        public Dictionary<string, object> OperateIntervalData(IntervalData values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> OperateFrequencyData<T>(FrequencyData<T> values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}