using System;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel.Backend
{
    public class BackendCommunicator
    {
        public void InitializeEventHandlers()
        {
            // TODO: How do I set up this to handle the events the front end sends to me?
        }

        // public event EventHandler<Message> BackendHandler;
        //
        // protected virtual void OnMessageReceived(Message e)
        // {
        //     switch (e.MainMessageType)
        //     {
        //         // Get a request to perform a statistic.
        //         case PrimaryMessageType.StatisticOperateRequest:
        //             break;
        //         // Get a response from a previously sent message about getting
        //         // additional info for parameters, etc. for a statistic.
        //         case PrimaryMessageType.StatisticAdditionalInfoResponse:
        //             break;
        //         // Get a response from a previously sent message about 
        //         // getting the data to perform the statistics on.
        //         case PrimaryMessageType.DataResponse:
        //             break;
        //         case PrimaryMessageType.DataTypeRequest:
        //             break;
        //         case PrimaryMessageType.DataTypeResponse:
        //             break;
        //         case PrimaryMessageType.DataTypeInformationRequest:
        //             break;
        //         case PrimaryMessageType.DataTypeInformationResponse:
        //             break;
        //         // All the cases that the backend doesn't handle.
        //         case PrimaryMessageType.StatisticOperateResponse:
        //         case PrimaryMessageType.StatisticAdditionalInfoRequest:
        //         case PrimaryMessageType.StatisticError:
        //         case PrimaryMessageType.DataRequest:
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException();
        //     }
        // }

        protected virtual void OnStatisticPerformed(StatisticOperateResponseMessage response)
        {
            StatisticPerformed?.Invoke(this, response);
        }

        protected virtual void OnStatisticNeedsMoreInfo(StatisticAdditionalInfoRequestMessage request)
        {
            StatisticNeedsMoreInfo?.Invoke(this, request);
        }

        protected virtual void OnRequestData(DataRequestMessage e)
        {
            RequestData?.Invoke(this, e);
        }

        protected virtual void OnStatisticHadError(StatisticErrorMessage e)
        {
            StatisticHadError?.Invoke(this, e);
        }


        /// <summary>
        ///     Event for when the statistic has finished its operations.
        /// </summary>
        public event EventHandler<StatisticOperateResponseMessage> StatisticPerformed;

        /// <summary>
        ///     Event for when the statistic needs more information in order to operate.
        /// </summary>
        public event EventHandler<StatisticAdditionalInfoRequestMessage> StatisticNeedsMoreInfo;

        /// <summary>
        ///     Event for when the backend needs data.
        /// </summary>
        public event EventHandler<DataRequestMessage> RequestData;

        /// <summary>
        ///     Event for when the statistic had an error during execution.
        /// </summary>
        public event EventHandler<StatisticErrorMessage> StatisticHadError;
    }
}