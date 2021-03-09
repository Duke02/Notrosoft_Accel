using System;
using System.Collections.Generic;
using Notrosoft_Accel.Backend.Statistics;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    /// <summary>
    ///     Back end sends this request when it needs more data for a given statistic.
    /// </summary>
    public class StatisticAdditionalInfoRequestMessage : Message
    {
        public StatisticAdditionalInfoRequestMessage(StatisticType typeOfStatistic, Statistic desiredStat,
            IEnumerable<IEnumerable<double>> data) : base(PrimaryMessageType.StatisticAdditionalInfoRequest)
        {
            TypeOfStatistic = typeOfStatistic;
            StatisticToUse = desiredStat;
            Data = data;
        }

        /// <summary>
        ///     The type of statistic that the backend needs information for.
        /// </summary>
        public StatisticType TypeOfStatistic { get; }

        public Statistic StatisticToUse { get; }

        /// <summary>
        ///     The bounds for the statistic. Tests whether the user inputted value is within the bounds or not.
        /// </summary>
        /// Key is the name of the parameter needed.
        // TODO: Change this to reference StatisticToUse.
        public Dictionary<string, Predicate<double>> Bounds { get; }

        public IEnumerable<string> ParametersNeeded => Bounds.Keys;

        public IEnumerable<IEnumerable<double>> Data { get; }
    }
}