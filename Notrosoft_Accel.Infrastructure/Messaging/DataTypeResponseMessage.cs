namespace Notrosoft_Accel.Infrastructure.Messaging
{
    // TODO: Is this needed?
    public class DataTypeResponseMessage : Message
    {
        public DataTypeResponseMessage(PrimaryMessageType mainMessageType) : base(mainMessageType)
        {
        }
    }
}