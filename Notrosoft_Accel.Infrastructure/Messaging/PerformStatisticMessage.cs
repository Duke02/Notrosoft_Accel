namespace Notrosoft_Accel.Infrastructure.Messaging
{
    public class PerformStatisticMessage : Message
    {
        public PerformStatisticMessage(PrimaryMessageType mainMessageType) : base(mainMessageType)
        {
        }
    }
}