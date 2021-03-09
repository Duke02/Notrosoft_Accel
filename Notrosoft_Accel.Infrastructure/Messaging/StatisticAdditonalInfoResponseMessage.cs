using System.Collections.Generic;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    /// <summary>
    ///     Front end sends this response after getting more information from the user about a statistic the backend needed
    ///     more information for.
    /// </summary>
    public class StatisticAdditonalInfoResponseMessage : Message
    {
        public Dictionary<string, double> Paramters;

        public StatisticAdditonalInfoResponseMessage(StatisticType statistic, Dictionary<string, double> parameters,
            ulong requestId) : base(PrimaryMessageType.StatisticAdditionalInfoResponse)
        {
            Paramters = parameters;
            Statistic = statistic;
            RequestMessageId = requestId;
        }

        public StatisticType Statistic { get; }

        /// <summary>
        ///     The id of the request that this response answers.
        /// </summary>
        public ulong RequestMessageId { get; }
    }
}