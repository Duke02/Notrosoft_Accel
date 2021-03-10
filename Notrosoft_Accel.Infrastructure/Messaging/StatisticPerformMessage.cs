using System.Collections.Generic;
using Notrosoft_Accel.Backend.Statistics;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    public class StatisticPerformMessage : Message
    {
        public StatisticPerformMessage() : base(PrimaryMessageType.StatisticPerform)
        {
        }

        public IEnumerable<IEnumerable<double>> Data { get; set; }

        public DataType TypeOfData { get; set; }

        public Dictionary<string, double> Parameters { get; set; }

        public ulong RequestMessageId { get; set; }

        public StatisticType Statistic { get; set; }

        public Statistic StatisticToPerform { get; set; }
    }
}