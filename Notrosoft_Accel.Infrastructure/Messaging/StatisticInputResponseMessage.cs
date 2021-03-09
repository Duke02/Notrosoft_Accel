using System.Collections.Generic;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    /// <summary>
    ///     Front end sends this response after getting more information from the user about a statistic the backend needed
    ///     more information for.
    /// </summary>
    public class StatisticInputResponseMessage : Message
    {
        public Dictionary<string, double> Paramters;


        public StatisticInputResponseMessage(StatisticType statistic, Dictionary<string, double> parameters) : base(
            PrimaryMessageType.StatisticInputResponse)
        {
            Paramters = parameters;
            Statistic = statistic;
        }

        public StatisticType Statistic { get; }
    }
}