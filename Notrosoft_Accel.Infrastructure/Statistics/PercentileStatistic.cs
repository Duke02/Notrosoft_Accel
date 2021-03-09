using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the percentile for the provided data.
    /// </summary>
    public class PercentileStatistic : Statistic
    {
        /// <summary>
        ///     Calculates the percentile for the provided data.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <returns>The requested percentile for the inputted data.</returns>
        public override StatisticOperateResponseMessage Operate(IEnumerable<IEnumerable<double>> values,
            StatisticOperateRequestMessage requestMessage)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Sort the values from least to greatest.
            var sortedValues = flattenedValues.OrderBy(val => val).ToArray();

            // TODO: Don't do this anymore.
            var neededParams = GetAdditionalParameters();
            var desiredPercentile = neededParams["percentile"];

            var indexOfPercentile = desiredPercentile * sortedValues.Length;

            double percentile;

            var isCleanInt = Math.Abs(indexOfPercentile - (int) indexOfPercentile) < 0.001;
            if (isCleanInt)
            {
                percentile = sortedValues[(int) indexOfPercentile];
            }
            else
            {
                // Need to figure out the closest indexes and the weight I have to do between them.
                var lowerIndex = (int) indexOfPercentile;
                var upperIndex = (int) (indexOfPercentile + 1);

                // The greater the weight, the closer the value
                // should be towards the element at the upper index.
                // Example: IndexOfPercentile is 2.6? weight is .6 
                // and the upperIndex should be weighted more heavily.
                var weight = indexOfPercentile - (int) indexOfPercentile;
                percentile = sortedValues[upperIndex] * weight + sortedValues[lowerIndex] * (1 - weight);
            }

            return PackageOutputIntoMessage(requestMessage, percentile);
        }

        /// <summary>
        ///     Gets the percentile parameter that the user wants to do.
        /// </summary>
        /// <returns>A dictionary containing the percentile that the user wants to use.</returns>
        public override Dictionary<string, double> GetAdditionalParameters()
        {
            // TODO: Actually have this call some front end code to get stuff working.
            return new()
            {
                {"percentile", .1}
            };
        }

        public override StatisticOperateResponseMessage PackageOutputIntoMessage(
            StatisticOperateRequestMessage requestMessage, params double[] output)
        {
            var percentile = output[0];

            var outputDict = new Dictionary<string, double>
            {
                {"percentile", percentile}
            };
            // TODO: Change this.
            var parameters = new Dictionary<string, double>();

            return new StatisticOperateResponseMessage(requestMessage.Statistic, outputDict, parameters,
                requestMessage.TypeOfData, requestMessage.MessageId);
        }
    }
}