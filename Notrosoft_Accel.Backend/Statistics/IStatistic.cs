using System.Collections.Generic;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     The base interface for statistics that all statistics are to inherit from.
    /// </summary>
    public interface IStatistic
    {
        /// <summary>
        ///     Base method that all statistics are to override to use for their statistics.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <param name="parameters"></param>
        /// <returns>The statistic for the child class.</returns>
        public Dictionary<string, object> OperateOrdinalData(OrdinalData values,
            params object[] parameters);

        public Dictionary<string, object> OperateFrequencyData<T>(IEnumerable<FrequencyData<T>> values,
            params object[] parameters);

        public Dictionary<string, object> OperateIntervalData(IEnumerable<IntervalData> values,
            params object[] parameters);
    }
}