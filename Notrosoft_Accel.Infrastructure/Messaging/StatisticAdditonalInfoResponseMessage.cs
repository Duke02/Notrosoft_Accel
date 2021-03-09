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


        public StatisticAdditonalInfoResponseMessage(StatisticType statistic, Dictionary<string, double> parameters) :
            base(
                PrimaryMessageType.StatisticAdditionalInfoResponse)
        {
            Paramters = parameters;
            Statistic = statistic;
        }

        public StatisticType Statistic { get; }
    }
}