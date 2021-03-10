using System;

namespace Notrosoft_Accel.Infrastructure.Messaging
{
    public abstract class Message : EventArgs
    {
        private static ulong _lastMessageId;

        protected Message(PrimaryMessageType mainMessageType)
        {
            MainMessageType = mainMessageType;
            MessageId = ++_lastMessageId;
        }

        /// <summary>
        ///     The main message type for the message. Helps with parsing the type of message.
        /// </summary>
        public PrimaryMessageType MainMessageType { get; }

        public ulong MessageId { get; }
    }

    public enum PrimaryMessageType
    {
        /// <summary>
        ///     Front end sends this request when the user selects a statistic to begin.
        ///     Includes how the backend should interpret the data.
        /// </summary>
        StatisticOperateRequest,

        /// <summary>
        ///     Back end sends this response when it has finished performing a statistical operation.
        ///     Message includes the results.
        /// </summary>
        StatisticOperateResponse,

        /// <summary>
        ///     Back end sends this request when it needs more data for a given statistic.
        /// </summary>
        StatisticAdditionalInfoRequest,

        /// <summary>
        ///     Front end sends this response after getting more information from the user about a statistic the backend needed
        ///     more information for.
        /// </summary>
        StatisticAdditionalInfoResponse,

        /// <summary>
        ///     Backend sends this message when there was an error when calculating a statistic.
        /// </summary>
        StatisticError,

        /// <summary>
        ///     Backend sends this message when it needs to get the data from the front end.
        /// </summary>
        DataRequest,

        /// <summary>
        ///     Front end sends this message filled with the data in response to a DataRequest Message.
        /// </summary>
        DataResponse,

        /// <summary>
        ///     Backend sends this message when it needs to know how to interpret the data from the front end.
        /// </summary>
        // TODO: Is this necessary?
        DataTypeRequest,

        /// <summary>
        ///     Front end sends this message in response to a DataTypeRequest message with the data type of the data.
        /// </summary>
        // TODO: Is this necessary?
        DataTypeResponse,

        /// <summary>
        ///     Backend sends this message when it needs more information about how to interpret the data that it has received.
        /// </summary>
        /// This is usually for interval and frequency data.
        // TODO: Is this necessary?
        DataTypeInformationRequest,

        /// <summary>
        ///     Front end sends this message in response to a DataTypeInformationRequest message with the additional information
        ///     that the back end needed.
        /// </summary>
        // TODO: Is this necessary?
        DataTypeInformationResponse,

        /// <summary>
        ///     Backend sends this information to send all the relevant data for the statistic actually operate.
        /// </summary>
        StatisticPerform
    }
}