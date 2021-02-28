using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Notrosoft_Accel.Statistics
{
    /// <summary>
    /// 
    /// </summary>
    public class PercentileStatistic : IStatistic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <returns></returns>
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            if (values.Count() != 2)
            {
                throw new InvalidOperationException("Inputted values need to be a 2 dimensional collection of data!");
            }
            
            // TODO: There's probably an error here because of the indexing.
            // Just make it an array probably.
            var xValues = values[0];
            var yValues = values[1];

            // If the counts are the same between x and y values
            // we need to error out.
            if (xValues.Count() != yValues.Count())
            {
                throw new InvalidOperationException(
                    "X Values need to have the same amount of data points as the y values.");
            }

            // If we don't have any values in any of the data, exit out.
            if (xValues.Count() == 0)
            {
                throw new InvalidOperationException("Inputted data need to have more than 0 data points!");
            }

            var yAverage = yValues.Average();
            var xAverage = xValues.Average();

            var slope = Utilities.GetCovariance(xValues, yValues) / Utilities.GetVariance(xValues); 
            var intercept = yAverage - slope * xAverage;

            // TODO: Figure out a way to return the slope and intercept.
            return slope;
        }
    }
}