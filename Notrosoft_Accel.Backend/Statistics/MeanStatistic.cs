﻿using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     Calculates the mathematical mean.
    /// </summary>
    public class MeanStatistic : IStatistic
    {
        /// <summary>
        ///     Calculates the mathematical mean (commonly called Average) from the provided data.
        /// </summary>
        /// <param name="values">The 2D data to use in the calculations.</param>
        /// <param name="param"></param>
        /// <returns>The mathematical mean of the inputted data.</returns>
        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] param)
        {
            // Flatten the 2D inputted container into a 1D container.
            var flattenedValues = Utilities.Flatten(values).ToArray();

            // Throw an exception if there's nothing in the inputted container.
            if (!flattenedValues.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Return the average/mean of the inputted values.
            return new Dictionary<string, object>
            {
                {"mean", flattenedValues.Average()}
            };
        }

        public Dictionary<string, object> OperateIntervalData(IntervalData values,
            params object[] parameters)
        {
            // Throw an exception if there's nothing in the inputted container.
            if (!values.Any())
                throw new InvalidOperationException("Inputted values need to have a count greater than 0!");

            // Return the average/mean of the inputted values.
            return new Dictionary<string, object>
            {
                {"mean", Utilities.GetGroupedMean(values)}
            };
        }

        public Dictionary<string, object> OperateFrequencyData<T>(FrequencyData<T> values,
            params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}