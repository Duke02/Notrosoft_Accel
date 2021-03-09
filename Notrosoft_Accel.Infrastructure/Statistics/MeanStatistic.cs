using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the mathematical mean.
    /// </summary>
    public class MeanStatistic : Statistic
    {
        /// <summary>
        ///     Calculates the mathematical mean (commonly called Average) from the provided data.
        /// </summary>
        /// <param name="values">The 2D data to use in the calculations.</param>
        /// <returns>The mathematical mean of the inputted data.</returns>
        public override StatisticOperateResponseMessage Operate(IEnumerable<IEnumerable<double>> values,
            StatisticOperateRequestMessage requestMessage)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Return the average/mean of the inputted values.
            return PackageOutputIntoMessage(requestMessage, flattenedValues.Average());
        }

        public override StatisticOperateResponseMessage PackageOutputIntoMessage(
            StatisticOperateRequestMessage requestMessage, params double[] output)
        {
            var mean = output[0];

            var outputDict = new Dictionary<string, double>
            {
                {"mean", mean}
            };
            var parameters = new Dictionary<string, double>();

            return new StatisticOperateResponseMessage(requestMessage.Statistic, outputDict, parameters,
                requestMessage.TypeOfData, requestMessage.MessageId);
        }
    }
}