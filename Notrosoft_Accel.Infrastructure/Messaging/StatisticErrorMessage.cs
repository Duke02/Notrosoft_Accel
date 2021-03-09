using System;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    /// <summary>
    ///     Backend sends this message when there was an error when calculating a statistic.
    /// </summary>
    public class StatisticErrorMessage : Message
    {
        public StatisticErrorMessage(Exception ex, string message) : base(PrimaryMessageType.StatisticError)
        {
            ExceptionEncountered = ex;
            Message = message;
        }

        public StatisticErrorMessage(Exception ex) : this(ex, ex.Message)
        {
        }

        public Exception ExceptionEncountered { get; }

        public string Message { get; }
    }
}