using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the Least Squares Line Statistic.
    /// </summary>
    public class LeastSquareLineStatistic : IStatistic
    {
        /// <summary>
        ///     Calculates the best fit linear line for the given data.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <param name="param"></param>
        /// <returns>The slope for the linear line. TODO: Needs to return intercept as well.</returns>
        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] param)
        {
            var valuesArray = values.ToArray();

            if (valuesArray.Length != 2)
                throw new InvalidOperationException("Inputted values need to be a 2 dimensional collection of data!");

            // TODO: There's probably an error here because of the indexing.
            // Just make it an array probably.
            var xValues = valuesArray[0].ToArray();
            var yValues = valuesArray[1].ToArray();

            // If the counts are the same between x and y values
            // we need to error out.
            if (xValues.Length != yValues.Length)
                throw new InvalidOperationException(
                    "X Values need to have the same amount of data points as the y values.");

            // If we don't have any values in any of the data, exit out.
            if (!xValues.Any())
                throw new InvalidOperationException("Inputted data need to have more than 0 data points!");

            var yAverage = yValues.Average();
            var xAverage = xValues.Average();

            // Took this from my CS 588 notes.
            // Basically the least square line is equal to
            // Cov(X, Y) / Var(X) + y_avg - x_Avg * Cov(X, Y) / Var(X)
            var slope = Utilities.GetCovariance(xValues, yValues) / Utilities.GetVariance(xValues);
            var intercept = yAverage - slope * xAverage;

            // TODO: Figure out a way to return the slope and intercept.
            return new Dictionary<string, object>
            {
                {"slope", slope},
                {"intercept", intercept}
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