using System;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel.Backend
{
    public class BackendCommunicator
    {
        public virtual void OnStatisticPerformed(StatisticOperateResponseMessage response)
        {
            StatisticPerformed?.Invoke(this, response);
        }

        public virtual void OnStatisticNeedsMoreInfo(StatisticAdditionalInfoRequestMessage request)
        {
            StatisticNeedsMoreInfo?.Invoke(this, request);
        }

        public virtual void OnRequestData(DataRequestMessage e)
        {
            RequestData?.Invoke(this, e);
        }

        public virtual void OnStatisticHadError(StatisticErrorMessage e)
        {
            StatisticHadError?.Invoke(this, e);
        }

        public virtual void OnPerformStatistic(StatisticPerformMessage e)
        {
            PerformStatistic?.Invoke(this, e);
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

        /// <summary>
        ///     Event for when the backend is ready for actually performing a statistic.
        /// </summary>
        public event EventHandler<StatisticPerformMessage> PerformStatistic;
    }
}