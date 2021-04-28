using System;
using System.Collections.Generic;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Backend.Statistics
{
    /// <summary>
    ///     The base interface for statistics that all statistics are to inherit from.
    /// </summary>
    public interface IStatistic
    {
        public Dictionary<string, object> Operate(IEnumerable<INotrosoftData> data, DataType dataType,
            params object[] parameters)
        {
            return dataType switch
            {
                DataType.Ordinal => DoOrdinalData(data as IEnumerable<OrdinalData>, parameters),
                DataType.Interval => DoIntervalData(data as IEnumerable<IntervalData>, parameters),
                DataType.Frequency => DoFrequencyData(data as IEnumerable<FrequencyData<double>>, parameters),
                _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
            };
        }

        /// <summary>
        ///     Base method that all statistics are to override to use for their statistics.
        /// </summary>
        /// <param name="values">The input values to calculate the statistic from.</param>
        /// <param name="parameters"></param>
        /// <returns>The statistic for the child class.</returns>
        public Dictionary<string, object> DoOrdinalData(IEnumerable<OrdinalData> values,
            params object[] parameters);

        public Dictionary<string, object> DoFrequencyData<T>(IEnumerable<FrequencyData<T>> values,
            params object[] parameters);

        public Dictionary<string, object> DoIntervalData(IEnumerable<IntervalData> values,
            params object[] parameters);
    }
}