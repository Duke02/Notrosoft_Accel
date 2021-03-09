using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the standard deviation statistic.
    /// </summary>
    public class StandardDeviationStatistic : Statistic
    {
        /// <summary>
        ///     Calculates the population standard deviation of the inputted data.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <returns>The standard deviation of the data.</returns>
        public override StatisticOperateResponseMessage Operate(IEnumerable<IEnumerable<double>> values,
            StatisticOperateRequestMessage requestMessage)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            var stddev = Math.Sqrt(Utilities.GetVariance(flattenedValues));

            // The standard deviation is just the square root of the variance. 
            return PackageOutputIntoMessage(requestMessage, stddev);
        }

        public override StatisticOperateResponseMessage PackageOutputIntoMessage(
            StatisticOperateRequestMessage requestMessage, params double[] output)
        {
            var stddev = output[0];

            var outputDict = new Dictionary<string, double>
            {
                {"stddev", stddev}
            };
            var parameters = new Dictionary<string, double>();

            return new StatisticOperateResponseMessage(requestMessage.Statistic, outputDict, parameters,
                requestMessage.TypeOfData, requestMessage.MessageId);
        }
    }
}