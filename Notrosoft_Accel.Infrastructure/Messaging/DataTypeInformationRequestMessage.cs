namespace Notrosoft_Accel.Infrastructure.Messaging
{
    /// <summary>
    ///     Backend sends this message when it needs more information about how to interpret the data that it has received.
    /// </summary>
    /// This is usually for interval and frequency data.
    public class DataTypeInformationRequestMessage : Message
    {
        public DataTypeInformationRequestMessage() : base(PrimaryMessageType.DataTypeInformationRequest)
        {
        }
    }
}