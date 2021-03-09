using System.Collections.Generic;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    /// <summary>
    ///     Front end sends this message filled with the data in response to a DataRequest Message.
    /// </summary>
    public class DataResponseMessage : Message
    {
        public DataResponseMessage(DataType type, IEnumerable<IEnumerable<double>> data) : base(
            PrimaryMessageType.DataResponse)
        {
            TypeOfData = type;
            Data = data;
        }

        // TODO: Not sure if this should be in another class or not.
        public DataType TypeOfData { get; }

        public IEnumerable<IEnumerable<double>> Data { get; }
    }
}