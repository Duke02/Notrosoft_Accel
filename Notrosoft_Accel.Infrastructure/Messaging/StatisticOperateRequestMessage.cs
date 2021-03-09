namespace Notrosoft_Accel.Infrastructure.Messaging
{
    /// <summary>
    ///     Front end sends this request when the user selects a statistic to begin.
    ///     Includes how the backend should interpret the data.
    /// </summary>
    public class StatisticOperateRequestMessage : Message
    {
        public StatisticOperateRequestMessage(StatisticType statistic) : base(
            PrimaryMessageType.StatisticOperateRequest)
        {
            Statistic = statistic;
        }

        public StatisticType Statistic { get; }
    }
}