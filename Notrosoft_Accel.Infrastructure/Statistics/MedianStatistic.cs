using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the median (middle value of sorted data) statistic from given data.
    /// </summary>
    public class MedianStatistic : Statistic
    {
        /// <summary>
        ///     Calculates the Median Statistic from the provided 2 dimensional data.
        /// </summary>
        /// <param name="values">The 2 dimensional data to calculate the median from.</param>
        /// <returns>The median of the data.</returns>
        public override StatisticOperateResponseMessage Operate(IEnumerable<IEnumerable<double>> values,
            StatisticOperateRequestMessage requestMessage)
        {
            // Flattens the 2D input into a 1d enumerable.
            var flattenedInput = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedInput.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Sorts the input in ascending order then converts it into an array.
            var sortedInput = flattenedInput.OrderBy(val => val).ToArray();

            var count = sortedInput.Length;

            var medianValue = -1.0;

            // If the length is even...
            if (count % 2 == 0)
            {
                // Get the elements that are around the middle 
                var firstIndex = count / 2;
                var secondIndex = firstIndex - 1;

                // And return their midpoint.
                medianValue = (sortedInput[secondIndex] + sortedInput[firstIndex]) / 2.0;
            }
            else
            {
                // Just return the middle of the input.
                var middleIndex = count / 2;

                medianValue = sortedInput[middleIndex];
            }

            return PackageOutputIntoMessage(requestMessage, medianValue);
        }

        public override StatisticOperateResponseMessage PackageOutputIntoMessage(
            StatisticOperateRequestMessage requestMessage, params double[] output)
        {
            var median = output[0];

            var outputDict = new Dictionary<string, double>
            {
                {"median", median}
            };
            var parameters = new Dictionary<string, double>();

            return new StatisticOperateResponseMessage(requestMessage.Statistic, outputDict, parameters,
                requestMessage.TypeOfData, requestMessage.MessageId);
        }
    }
}