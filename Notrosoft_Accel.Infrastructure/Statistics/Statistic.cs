using System.Collections.Generic;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     The base interface for statistics that all statistics are to inherit from.
    /// </summary>
    public abstract class Statistic
    {
        /// <summary>
        ///     Base method that all statistics are to override to use for their statistics.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <param name="requestMessage"></param>
        /// <returns>The statistic for the child class.</returns>
        public abstract StatisticOperateResponseMessage Operate(IEnumerable<IEnumerable<double>> values,
            StatisticOperateRequestMessage requestMessage);

        public virtual Dictionary<string, double> GetAdditionalParameters()
        {
            return new();
        }

        public abstract StatisticOperateResponseMessage PackageOutputIntoMessage(
            StatisticOperateRequestMessage requestMessage, params double[] output);
    }
}