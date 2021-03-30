using System.Collections.Generic;
using Notrosoft_Accel.Infrastructure;
using Notrosoft_Accel.Backend.Statistics;

namespace Notrosoft_Accel
{
    internal class Interlayer
    {
        private readonly List<List<double>> _doubles = new();
        private readonly List<List<int>> _ints = new();
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
            {
                _doubles[i][j] = double.Parse(_strings[i][j]);
                _ints[i][j] = int.Parse(_strings[i][j]);
            }
            // Set the return variable for statistic operations.
            Dictionary<string, object> keyValues = new Dictionary<string, object>(); 
            // String constructed based off the statistic performed.
            string returnStr = "";
            // String used for when a Statistical method is not implemented quite yet.
            string NaS = "Statistic method has not been added yet";

            foreach (StatisticType stat in stats)
            {
                switch (stat)
                {
                    case StatisticType.Mean:
                        var mean = new MeanStatistic();
                        keyValues = mean.Operate(_doubles, param);
                        returnStr += "Mean:";
                        break;

                    case StatisticType.Median:
                        var median = new MedianStatistic();
                        keyValues = median.Operate(_doubles, param);
                        returnStr += "Median:";
                        break;

                    case StatisticType.Mode:
                        var mode = new ModeStatistic();
                        keyValues = mode.Operate(_doubles, param);
                        returnStr += "Mode:";
                        break;

                    case StatisticType.StandardDeviation:
                        var sDev = new StandardDeviationStatistic();
                        keyValues = sDev.Operate(_doubles, param);
                        returnStr += "Standard Deviation:";
                        break;

                    case StatisticType.Variance:
                        var variance = new VarianceStatistic();
                        keyValues = variance.Operate(_doubles, param);
                        returnStr += "Variance:";
                        break;

                    case StatisticType.CoefficientOfVariance:
                        var coeffVar = new CoefficientOfVarianceStatistic();
                        keyValues = coeffVar.Operate(_doubles, param);
                        returnStr += "Coefficient of Variance:";
                        break;

                    case StatisticType.Percentile:
                        // Need parameter between 0 and 1 for percentile
                        var percentile = new PercentileStatistic();
                        keyValues = percentile.Operate(_doubles, param);
                        returnStr += "Percentile:";
                        break;

                    case StatisticType.ProbabilityDistribution:
                        // Outputs mean and stdev
                        returnStr += "Probability:";
                        break;

                    case StatisticType.BinomialDistribution:
                        // Outputs something.TBD
                        var binDis = new BinomialDistributionStatistic();
                        keyValues = binDis.Operate(_doubles, param);
                        returnStr += "Binomial Distribution:";
                        break;

                    case StatisticType.LeastSquaresLine:
                        // /outputs slope and intercept
                        var leastSqr = new LeastSquareLineStatistic();
                        keyValues = leastSqr.Operate(_doubles, param);
                        returnStr += "Least Square Line:";
                        break;

                    case StatisticType.ChiSquare:
                        // TODO: Learn.
                        var chiSqr = new ChiSquareStatistic();
                        keyValues = chiSqr.Operate(_doubles, param);
                        returnStr += "Chi Square:";
                        break;

                    case StatisticType.CorrelationCoefficient:
                        var corrCoe = new CorrelationCoefficientStatistic();
                        keyValues = corrCoe.Operate(_doubles, param);
                        returnStr += "Correlation Coefficient:";
                        break;

                    case StatisticType.SignTest:
                        // Complicated.
                        var sign = new SignTestStatistic();
                        keyValues = sign.Operate(_doubles, param);
                        returnStr += "Sign Test:";
                        break;

                    case StatisticType.RankSumTest:
                        // Complicated.
                        var rankSum = new RankSumTestStatistic();
                        keyValues = rankSum.Operate(_doubles, param);
                        returnStr += "Rank Sum:";
                        break;

                    case StatisticType.SpearmanRankCorrelationCoefficient:
                        var spearman = new SpearmanRankCorrelationStatistic();
                        keyValues = spearman.Operate(_doubles, param);
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