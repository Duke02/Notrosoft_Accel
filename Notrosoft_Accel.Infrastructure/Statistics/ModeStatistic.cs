﻿using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Statistic class that calculates the mode of inputted data.
    /// </summary>
    public class ModeStatistic : Statistic
    {
        /// <summary>
        ///     Calculates the mode (most common number) of the inputted data.
        /// </summary>
        /// <param name="values">A 2D data collection of numbers.</param>
        /// <param name="requestMessage"></param>
        /// <returns>The most common number of the inputted data.</returns>
        public override StatisticOperateResponseMessage Operate(IEnumerable<IEnumerable<double>> values,
            StatisticPerformMessage requestMessage)
        {
            // Flatten the 2d list of lists into a 1d list.
            var flattenedArray = Utilities.Flatten(values).ToArray();

            // Throw an exception if there are no input values.
            if (!flattenedArray.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // This is based off of the Counting Sort algorithm,
            // https://en.wikipedia.org/wiki/Counting_sort
            // but instead of having a secondary array, we just calculate 
            // the counts of the input values in a dictionary and 
            // get the max value with the greatest count.
            var distinctNums = flattenedArray.Distinct();
            var countDict = distinctNums.ToDictionary(n => n, _ => 0);

            foreach (var num in flattenedArray) countDict[num]++;

            var mode = countDict.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return PackageOutputIntoMessage(requestMessage, mode);
        }

        public override StatisticOperateResponseMessage PackageOutputIntoMessage(StatisticPerformMessage requestMessage,
            params double[] output)
        {
            var mode = output[0];

            var outputDict = new Dictionary<string, double>
            {
                {"mode", mode}
            };

            return new StatisticOperateResponseMessage(requestMessage.Statistic, outputDict, requestMessage.Parameters,
                requestMessage.TypeOfData, requestMessage.MessageId);
        }
    }
}