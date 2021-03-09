using System.Collections.Generic;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    public class StatisticOperateResponseMessage : Message
    {
        public StatisticOperateResponseMessage(StatisticType statistic, Dictionary<string, double> output,
            Dictionary<string, double> parameters, DataType dataType, ulong requestId) : base(
            PrimaryMessageType.StatisticOperateResponse)
        {
            Statistic = statistic;
            Output = output;
            Parameters = parameters;
            DataTypeUsed = dataType;
            RequestMessageId = requestId;
        }

        public StatisticType Statistic { get; }

        public Dictionary<string, double> Output { get; }

        public Dictionary<string, double> Parameters { get; }

        public DataType DataTypeUsed { get; }

        /// <summary>
        ///     The message id of the request that this response answers.
        /// </summary>
        public ulong RequestMessageId { get; }
    }
}