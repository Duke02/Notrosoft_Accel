using System.Collections.Generic;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    public class StatisticOperateResponseMessage : Message
    {
        public StatisticOperateResponseMessage(StatisticType statistic, Dictionary<string, double> output,
            Dictionary<string, double> parameters, DataType dataType) : base(
            PrimaryMessageType.StatisticOperateResponse)
        {
            Statistic = statistic;
            Output = output;
            Parameters = parameters;
            DataTypeUsed = dataType;
        }

        public StatisticType Statistic { get; }

        public Dictionary<string, double> Output { get; }

        public Dictionary<string, double> Parameters { get; }

        public DataType DataTypeUsed { get; }
    }
}