using System.Collections.Generic;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    /// <summary>
    ///     Front end sends this request when the user selects a statistic to begin.
    ///     Includes how the backend should interpret the data.
    /// </summary>
    public class StatisticOperateRequestMessage : Message
    {
        public StatisticOperateRequestMessage(StatisticType statistic, IEnumerable<IEnumerable<double>> data,
            DataType dataType) : base(PrimaryMessageType.StatisticOperateRequest)
        {
            Statistic = statistic;
            Data = data;
            TypeOfData = dataType;
        }

        public StatisticType Statistic { get; }

        // TODO: This might need to be moved.
        public IEnumerable<IEnumerable<double>> Data { get; }

        public DataType TypeOfData { get; }
    }
}