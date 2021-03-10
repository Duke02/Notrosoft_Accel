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
        public StatisticAdditionalInfoRequestMessage() : base(PrimaryMessageType.StatisticAdditionalInfoRequest)
        {
        }

        /// <summary>
        ///     The type of statistic that the backend needs information for.
        /// </summary>
        public StatisticType Statistic { get; set; }

        public Statistic StatisticToUse { get; set; }

        /// <summary>
        ///     The bounds for the statistic. Tests whether the user inputted value is within the bounds or not.
        /// </summary>
        /// Key is the name of the parameter needed.
        // TODO: Change this to reference StatisticToUse.
        public Dictionary<string, Predicate<double>> Bounds { get; set; }

        public IEnumerable<string> ParametersNeeded => Bounds.Keys;

        public IEnumerable<IEnumerable<double>> Data { get; set; }

        public DataType TypeOfData { get; set; }
    }
}