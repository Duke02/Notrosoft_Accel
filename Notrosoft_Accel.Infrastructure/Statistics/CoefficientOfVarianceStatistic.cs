using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the coefficient of variance statistic.
    /// </summary>
    public class CoefficientOfVarianceStatistic : Statistic
    {
        /// <summary>
        ///     Calculates the coefficient of variance statistic for the provided data.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <param name="requestMessage"></param>
        /// <returns>The CV of the data.</returns>
        public override StatisticOperateResponseMessage Operate(IEnumerable<IEnumerable<double>> values,
            StatisticOperateRequestMessage requestMessage)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Coefficient of variance is the stddev / mean.
            // So calculate the mean
            var average = flattenedValues.Average();
            // Then calculate the standard deviation.
            var stddev = Math.Sqrt(Utilities.GetVariance(flattenedValues));

            // And return their ratio.
            return PackageOutputIntoMessage(requestMessage, stddev / average);
        }

        public override StatisticOperateResponseMessage PackageOutputIntoMessage(
            StatisticOperateRequestMessage requestMessage, params double[] output)
        {
            var cv = output[0];
            var outputDict = new Dictionary<string, double>
            {
                {"cv", cv}
            };
            return new StatisticOperateResponseMessage(StatisticType.CoefficientOfVariance, outputDict,
                new Dictionary<string, double>(), requestMessage.TypeOfData, requestMessage.MessageId);
        }
    }
}