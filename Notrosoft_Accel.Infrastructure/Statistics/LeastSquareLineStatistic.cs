using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the Least Squares Line Statistic.
    /// </summary>
    public class LeastSquareLineStatistic : Statistic
    {
        /// <summary>
        ///     Calculates the best fit linear line for the given data.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <param name="requestMessage"></param>
        /// <returns>The slope for the linear line. TODO: Needs to return intercept as well.</returns>
        public override StatisticOperateResponseMessage Operate(IEnumerable<IEnumerable<double>> values,
            StatisticPerformMessage requestMessage)
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

            return PackageOutputIntoMessage(requestMessage, slope, intercept);
        }

        public override StatisticOperateResponseMessage PackageOutputIntoMessage(StatisticPerformMessage requestMessage,
            params double[] output)
        {
            var slope = output[0];
            var intercept = output[1];

            var outputDict = new Dictionary<string, double>
            {
                {"slope", slope},
                {"intercept", intercept}
            };

            return new StatisticOperateResponseMessage(requestMessage.Statistic, outputDict, requestMessage.Parameters,
                requestMessage.TypeOfData, requestMessage.MessageId);
        }
    }
}