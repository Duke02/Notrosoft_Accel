using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Notrosoft_Accel.Statistics
{
    /// <summary>
    /// 
    /// </summary>
    public class LeastSquareLineStatistic : IStatistic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <returns></returns>
        public double Operate(IEnumerable<IEnumerable<double>> values)
        {
            var valuesArray = values.ToArray();
            
            if (valuesArray.Count() != 2)
            {
                throw new InvalidOperationException("Inputted values need to be a 2 dimensional collection of data!");
            }
            
            // TODO: There's probably an error here because of the indexing.
            // Just make it an array probably.
            var xValues = valuesArray[0].ToArray();
            var yValues = valuesArray[1].ToArray();

            // If the counts are the same between x and y values
            // we need to error out.
            if (xValues.Length != yValues.Length)
            {
                throw new InvalidOperationException(
                    "X Values need to have the same amount of data points as the y values.");
            }

            // If we don't have any values in any of the data, exit out.
            if (!xValues.Any())
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