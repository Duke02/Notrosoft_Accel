namespace Notrosoft_Accel.Infrastructure.Messaging
{
    /// <summary>
    ///     Backend sends this message when it needs to get the data from the front end.
    /// </summary>
    public class DataRequestMessage : Message
    {
        public DataRequestMessage() : base(PrimaryMessageType.DataRequest)
        {
        }
    }
}