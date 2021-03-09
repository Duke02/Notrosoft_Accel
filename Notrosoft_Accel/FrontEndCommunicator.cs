using System;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel
{
    public class FrontEndCommunicator
    {
        /// <summary>
        ///     Event for when the user selects a statistic to be performed.
        /// </summary>
        public event EventHandler<StatisticOperateRequestMessage> NeedToPerformStatistic;

        /// <summary>
        ///     Event for when the user has filled in the additional information that the statistic needs.
        /// </summary>
        public event EventHandler<StatisticAdditonalInfoResponseMessage> HaveAdditionalInfoForStatistic;

        /// <summary>
        ///     Event for when the user has filled in all of the data and is ready for the backend.
        /// </summary>
        public event EventHandler<DataResponseMessage> HaveData;


        protected virtual void OnHaveData(DataResponseMessage e)
        {
            HaveData?.Invoke(this, e);
        }

        protected virtual void OnHaveAdditionalInfoForStatistic(StatisticAdditonalInfoResponseMessage e)
        {
            HaveAdditionalInfoForStatistic?.Invoke(this, e);
        }

        protected virtual void OnNeedToPerformStatistic(StatisticOperateRequestMessage e)
        {
            NeedToPerformStatistic?.Invoke(this, e);
        }
    }
}