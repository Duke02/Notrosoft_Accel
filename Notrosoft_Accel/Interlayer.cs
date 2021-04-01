using System.Collections.Generic;
using Notrosoft_Accel.Backend.Statistics;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel
{
    internal class Interlayer
    {
        private readonly List<List<double>> _doubles = new();
        private List<List<string>> _strings = new();

        public string doStatistics(List<List<string>> input, StatisticType[] stats, params object[] param)
        {
            // Ensure List is horizontal, not vertical.
            if (input.Count < input[0].Count)
                for (var i = 0; i < input[0].Count; i++)
                for (var j = 0; j < input.Count; j++)
                    _strings[i][j] = input[j][i];
            else _strings = input;

            // Assign values for _doubles and _ints
            for (var i = 0; i < _strings.Count; i++)
            for (var j = 0; j < _strings[0].Count; j++)
                _doubles[i][j] = double.Parse(_strings[i][j]);
            // Set the return variable for statistic operations.
            var keyValues = new Dictionary<string, object>();
            // String constructed based off the statistic performed.
            var returnStr = "";
            // String used for when a Statistical method is not implemented quite yet.
            var NaS = "Statistic method has not been added yet";

            foreach (var stat in stats)
            {
                switch (stat)
                {
                    case StatisticType.Mean:
                        var mean = new MeanStatistic();
                        keyValues = mean.OperateOrdinalData(_doubles, param);
                        keyValues.TryGetValue("mean", out var r);
                        returnStr += "Mean:" + r;
                        break;

                    case StatisticType.Median:
                        var median = new MedianStatistic();
                        keyValues = median.OperateOrdinalData(_doubles, param);
                        returnStr += "Median:";
                        break;

                    case StatisticType.Mode:
                        var mode = new ModeStatistic();
                        keyValues = mode.OperateOrdinalData(_doubles, param);
                        returnStr += "Mode:";
                        break;

                    case StatisticType.StandardDeviation:
                        var sDev = new StandardDeviationStatistic();
                        keyValues = sDev.OperateOrdinalData(_doubles, param);
                        returnStr += "Standard Deviation:";
                        break;

                    case StatisticType.Variance:
                        var variance = new VarianceStatistic();
                        keyValues = variance.OperateOrdinalData(_doubles, param);
                        returnStr += "Variance:";
                        break;

                    case StatisticType.CoefficientOfVariance:
                        var coeffVar = new CoefficientOfVarianceStatistic();
                        keyValues = coeffVar.OperateOrdinalData(_doubles, param);
                        returnStr += "Coefficient of Variance:";
                        break;

                    case StatisticType.Percentile:
                        // Need parameter between 0 and 1 for percentile
                        var percentile = new PercentileStatistic();
                        keyValues = percentile.OperateOrdinalData(_doubles, param);
                        returnStr += "Percentile:";
                        break;

                    case StatisticType.ProbabilityDistribution:
                        // Outputs mean and stdev
                        returnStr += "Probability:";
                        break;

                    case StatisticType.BinomialDistribution:
                        // Outputs something.TBD
                        var binDis = new BinomialDistributionStatistic();
                        keyValues = binDis.OperateOrdinalData(_doubles, param);
                        returnStr += "Binomial Distribution:";
                        break;

                    case StatisticType.LeastSquaresLine:
                        // /outputs slope and intercept
                        var leastSqr = new LeastSquareLineStatistic();
                        keyValues = leastSqr.OperateOrdinalData(_doubles, param);
                        returnStr += "Least Square Line:";
                        break;

                    case StatisticType.ChiSquare:
                        // TODO: Learn.
                        var chiSqr = new ChiSquareStatistic();
                        keyValues = chiSqr.OperateOrdinalData(_doubles, param);
                        returnStr += "Chi Square:";
                        break;

                    case StatisticType.CorrelationCoefficient:
                        var corrCoe = new CorrelationCoefficientStatistic();
                        keyValues = corrCoe.OperateOrdinalData(_doubles, param);
                        returnStr += "Correlation Coefficient:";
                        break;

                    case StatisticType.SignTest:
                        // Complicated.
                        var sign = new SignTestStatistic();
                        keyValues = sign.OperateOrdinalData(_doubles, param);
                        returnStr += "Sign Test:";
                        break;

                    case StatisticType.RankSumTest:
                        // Complicated.
                        var rankSum = new RankSumTestStatistic();
                        keyValues = rankSum.OperateOrdinalData(_doubles, param);
                        returnStr += "Rank Sum:";
                        break;

                    case StatisticType.SpearmanRankCorrelationCoefficient:
                        var spearman = new SpearmanRankCorrelationStatistic();
                        keyValues = spearman.OperateOrdinalData(_doubles, param);
                        returnStr += "Spearman Rank Correlation:";
                        break;

                    default:
                        returnStr += NaS;
                        break;
                }

                returnStr += "\n";
            }

            return returnStr;
        }
    }
}