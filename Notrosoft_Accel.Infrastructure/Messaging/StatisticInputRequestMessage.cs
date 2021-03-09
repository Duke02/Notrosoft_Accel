using System;
using System.Collections.Generic;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    /// <summary>
    ///     Back end sends this request when it needs more data for a given statistic.
    /// </summary>
    public class StatisticInputRequestMessage : Message
    {
        protected StatisticInputRequestMessage(StatisticType statistic, Dictionary<string, Predicate<double>> bounds) :
            base(PrimaryMessageType.StatisticInputRequest)
        {
            Statistic = statistic;
            Bounds = bounds;
        }

        /// <summary>
        ///     The type of statistic that the backend needs information for.
        /// </summary>
        public StatisticType Statistic { get; }

        /// <summary>
        ///     The bounds for the statistic. Tests whether the user inputted value is within the bounds or not.
        /// </summary>
        /// Key is the name of the parameter needed.
        public Dictionary<string, Predicate<double>> Bounds { get; }

        public IEnumerable<string> ParametersNeeded => Bounds.Keys;
    }
}