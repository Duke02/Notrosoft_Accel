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
        public event EventHandler<StatisticAdditionalInfoResponseMessage> HaveAdditionalInfoForStatistic;

        /// <summary>
        ///     Event for when the user has filled in all of the data and is ready for the backend.
        /// </summary>
        public event EventHandler<DataResponseMessage> HaveData;


        public virtual void OnHaveData(DataResponseMessage e)
        {
            HaveData?.Invoke(this, e);
        }

        public virtual void OnHaveAdditionalInfoForStatistic(StatisticAdditionalInfoResponseMessage e)
        {
            HaveAdditionalInfoForStatistic?.Invoke(this, e);
        }

        public virtual void OnNeedToPerformStatistic(StatisticOperateRequestMessage e)
        {
            NeedToPerformStatistic?.Invoke(this, e);
        }
    }
}