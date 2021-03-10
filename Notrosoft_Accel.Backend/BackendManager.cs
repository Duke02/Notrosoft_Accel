using System;
using Notrosoft_Accel.Backend.Statistics;
using Notrosoft_Accel.Infrastructure;
using Notrosoft_Accel.Infrastructure.Messaging;

namespace Notrosoft_Accel.Backend
{
    public class BackendManager
    {
        private readonly BackendCommunicator _backend;
        private readonly FrontEndCommunicator _frontEnd;

        public BackendManager()
        {
            _frontEnd = new FrontEndCommunicator();
            _backend = new BackendCommunicator();
            InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {
            _frontEnd.NeedToPerformStatistic += FrontEndOnNeedToPerformStatistic;
            _frontEnd.HaveAdditionalInfoForStatistic += FrontEndOnHaveAdditionalInfoForStatistic;
            _backend.PerformStatistic += BackendOnPerformStatistic;
        }

        private void BackendOnPerformStatistic(object sender, StatisticPerformMessage message)
        {
            var desiredStat = message.StatisticToPerform;
            var convertedData = message.Data;

            var outputMessage = desiredStat.Operate(convertedData, message);

            _backend.OnStatisticPerformed(outputMessage);
        }

        private void FrontEndOnHaveAdditionalInfoForStatistic(object sender,
            StatisticAdditionalInfoResponseMessage response)
        {
            var performStatMessage = new StatisticPerformMessage
            {
                Data = response.Data,
                Parameters = response.Parameters,
                RequestMessageId = response.RequestMessageId,
                Statistic = response.Statistic,
                StatisticToPerform = response.StatisticToPerform,
                TypeOfData = response.TypeOfData
            };

            _backend.OnPerformStatistic(performStatMessage);
        }

        private void FrontEndOnNeedToPerformStatistic(object sender, StatisticOperateRequestMessage request)
        {
            // Figure out the statistic that needs to be performed.
            // Convert the data to the appropriate format.
            // Figure out if the statistic needs additional info.
            // Send the data to the statistic.

            var convertedData = Utilities.ConvertData(request.Data, request.TypeOfData);
            Statistic desiredStat = null;
            var needsAdditionalInfo = false;

            switch (request.Statistic)
            {
                case StatisticType.Mean:
                    desiredStat = new MeanStatistic();
                    break;
                case StatisticType.Median:
                    desiredStat = new MedianStatistic();
                    break;
                case StatisticType.Mode:
                    desiredStat = new ModeStatistic();
                    break;
                case StatisticType.StandardDeviation:
                    desiredStat = new StandardDeviationStatistic();
                    break;
                case StatisticType.Variance:
                    desiredStat = new VarianceStatistic();
                    break;
                case StatisticType.CoefficientOfVariance:
                    desiredStat = new CoefficientOfVarianceStatistic();
                    break;
                case StatisticType.Percentile:
                    desiredStat = new PercentileStatistic();
                    needsAdditionalInfo = true;
                    break;
                case StatisticType.ProbabilityDistribution:
                    // TODO
                    break;
                case StatisticType.BinomialDistribution:
                    // TODO
                    break;
                case StatisticType.LeastSquaresLine:
                    desiredStat = new LeastSquareLineStatistic();
                    break;
                case StatisticType.ChiSquare:
                    // TODO
                    break;
                case StatisticType.CorrelationCoefficient:
                    // TODO
                    break;
                case StatisticType.SignTest:
                    // TODO
                    break;
                case StatisticType.RankSumTest:
                    // TODO
                    break;
                case StatisticType.SpearmanRankCorrelationCoefficient:
                    // TODO
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (desiredStat == null) return;

            if (needsAdditionalInfo)
            {
                // TODO: Set up way to get the message back and still be able to run the show.
                var additionalInfoMessage =
                    new StatisticAdditionalInfoRequestMessage
                    {
                        Data = convertedData,
                        Statistic = request.Statistic,
                        StatisticToUse = desiredStat,
                        TypeOfData = request.TypeOfData
                    };

                _backend.OnStatisticNeedsMoreInfo(additionalInfoMessage);
            }
            else
            {
                var performStatMessage = new StatisticPerformMessage
                {
                    Data = convertedData,
                    RequestMessageId = request.MessageId,
                    Statistic = request.Statistic,
                    StatisticToPerform = desiredStat,
                    TypeOfData = request.TypeOfData
                };

                _backend.OnPerformStatistic(performStatMessage);
            }
        }
    }
}