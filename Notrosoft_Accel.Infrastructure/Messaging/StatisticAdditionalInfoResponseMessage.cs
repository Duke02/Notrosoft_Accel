using System.Collections.Generic;
using Notrosoft_Accel.Backend.Statistics;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    /// <summary>
    ///     Front end sends this response after getting more information from the user about a statistic the backend needed
    ///     more information for.
    /// </summary>
    public class StatisticAdditionalInfoResponseMessage : Message
    {
        public StatisticAdditionalInfoResponseMessage() : base(PrimaryMessageType.StatisticAdditionalInfoResponse)
        {
        }

        public StatisticType Statistic { get; set; }

        /// <summary>
        ///     The id of the request that this response answers.
        /// </summary>
        public ulong RequestMessageId { get; set; }

        public Dictionary<string, double> Parameters { get; set; }

        public IEnumerable<IEnumerable<double>> Data { get; set; }

        public DataType TypeOfData { get; set; }

        public Statistic StatisticToPerform { get; set; }
    }
}