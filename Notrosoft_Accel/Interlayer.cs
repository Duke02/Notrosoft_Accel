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
            _doubles.Clear();
            _strings.Clear();
            var s = new List<string>();
            // Ensure List is always presented in the same manner
            if (input.Count > input[0].Count)
                for (var i = 0; i < input.Count; i++)
                {
                    s.Clear();
                    for (var j = 0; j < input[0].Count; j++) s.Add(input[i][j]);
                    _strings.Add(s);
                }
            else _strings = input;

            
            // Assign values for _doubles and _ints
            for (var i = 0; i < _strings.Count; i++)
            {
                var row = new List<double>();
                for (var j = 0; j < _strings[i].Count; j++) row.Add(double.Parse(_strings[i][j]));
                _doubles.Add(row);
            }

            // Set the return variable for statistic operations.
            var keyValues = new Dictionary<string, object>();
            // String constructed based off the statistic performed.
            var returnStr = "";
            // String used for when a Statistical method is not implemented quite yet.
            var NaS = "Statistic method has not been added yet";

            var data = new OrdinalData(_doubles);

            foreach (var stat in stats)
            {
                switch (stat)
                {
                    case StatisticType.Mean:
                        var mean = new MeanStatistic();
                        keyValues = mean.OperateOrdinalData(data, param);
                        keyValues.TryGetValue("mean", out var meanr);
                        returnStr += "Mean: " + meanr;
                        break;

                    case StatisticType.Median:
                        var median = new MedianStatistic();
                        keyValues = median.OperateOrdinalData(data, param);
                        keyValues.TryGetValue("median", out var medr);
                        returnStr += "Median: " + medr;
                        break;

                    case StatisticType.Mode:
                        var mode = new ModeStatistic();
                        keyValues = mode.OperateOrdinalData(data, param);
                        keyValues.TryGetValue("mode", out var moder);
                        returnStr += "Mode: " + moder;
                        break;

                    case StatisticType.StandardDeviation:
                        var sDev = new StandardDeviationStatistic();
                        keyValues = sDev.OperateOrdinalData(data, param);
                        keyValues.TryGetValue("Standard Deviation", out var SDr);
                        returnStr += "Standard Deviation: " + SDr;
                        break;

                    case StatisticType.Variance:
                        var variance = new VarianceStatistic();
                        keyValues = variance.OperateOrdinalData(data, param);
                        returnStr += "Variance:";
                        break;

                    case StatisticType.CoefficientOfVariance:
                        var coeffVar = new CoefficientOfVarianceStatistic();
                        keyValues = coeffVar.OperateOrdinalData(data, param);
                        returnStr += "Coefficient of Variance:";
                        break;

                    case StatisticType.Percentile:
                        // Need parameter between 0 and 1 for percentile
                        var percentile = new PercentileStatistic();
                        keyValues = percentile.OperateOrdinalData(data, param);
                        returnStr += "Percentile:";
                        break;

                    case StatisticType.ProbabilityDistribution:
                        // Outputs mean and stdev
                        returnStr += "Probability:";
                        break;

                    case StatisticType.BinomialDistribution:
                        // hypothesis, probOfSuccess (0-1), confidence (0-1, optional)
                        var binDis = new BinomialDistributionStatistic();
                        keyValues = binDis.OperateOrdinalData(data, param);
                        returnStr += "Binomial Distribution:";
                        break;

                    case StatisticType.LeastSquaresLine:
                        // /outputs slope and intercept
                        var leastSqr = new LeastSquareLineStatistic();
                        keyValues = leastSqr.OperateOrdinalData(data, param);
                        returnStr += "Least Square Line:";
                        break;

                    case StatisticType.ChiSquare:
                        var chiSqr = new ChiSquareStatistic();
                        keyValues = chiSqr.OperateOrdinalData(data, param);
                        returnStr += "Chi Square:";
                        break;

                    case StatisticType.CorrelationCoefficient:
                        var corrCoe = new CorrelationCoefficientStatistic();
                        keyValues = corrCoe.OperateOrdinalData(data, param);
                        returnStr += "Correlation Coefficient:";
                        break;

                    case StatisticType.SignTest:
                        // comparisonType, valueToCompareAgainst
                        var sign = new SignTestStatistic();
                        keyValues = sign.OperateOrdinalData(data, param);
                        returnStr += "Sign Test:";
                        break;

                    case StatisticType.RankSumTest:

                        var rankSum = new RankSumTestStatistic();
                        keyValues = rankSum.OperateOrdinalData(data, param);
                        returnStr += "Rank Sum:";
                        break;

                    case StatisticType.SpearmanRankCorrelationCoefficient:
                        var spearman = new SpearmanRankCorrelationStatistic();
                        keyValues = spearman.OperateOrdinalData(data, param);
                        returnStr += "Spearman Rank Correlation:";
                        break;

                    default:
                        returnStr += NaS;
                        break;
                }

                returnStr += "\n\n";
            }

            return returnStr;
        }
    }
}