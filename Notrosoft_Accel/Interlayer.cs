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
                        keyValues = mean.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("mean", out var meanr);
                        returnStr += "Mean: " + meanr;
                        break;

                    case StatisticType.Median:
                        var median = new MedianStatistic();
                        keyValues = median.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("median", out var medr);
                        returnStr += "Median: " + medr;
                        break;

                    case StatisticType.Mode:
                        var mode = new ModeStatistic();
                        keyValues = mode.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("mode", out var moder);
                        returnStr += "Mode: " + moder;
                        break;

                    case StatisticType.StandardDeviation:
                        var sDev = new StandardDeviationStatistic();
                        keyValues = sDev.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("Standard Deviation", out var SDr);
                        returnStr += "Standard Deviation: " + SDr;
                        break;

                    case StatisticType.Variance:
                        var variance = new VarianceStatistic();
                        keyValues = variance.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("variance", out var Var);
                        returnStr += "Variance:" + Var;
                        break;

                    case StatisticType.CoefficientOfVariance:
                        var coeffVar = new CoefficientOfVarianceStatistic();
                        keyValues = coeffVar.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("cv", out var CVr);
                        returnStr += "Coefficient of Variance:" + CVr;
                        break;

                    case StatisticType.Percentile:
                        // Need parameter between 0 and 1 for percentile
                        var percentile = new PercentileStatistic();
                        keyValues = percentile.OperateOrdinalData(data, 0.5);
                        keyValues.TryGetValue("percentile", out var Per);
                        returnStr += "Percentile:" + Per;
                        break;

                    case StatisticType.ProbabilityDistribution:
                        // Outputs mean and stdev
                        var PrsDev = new StandardDeviationStatistic();
                        keyValues = PrsDev.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("Standard Deviation", out var PrSDr);
                        var Prmean = new MeanStatistic();
                        keyValues = Prmean.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("mean", out var Prmeanr);
                        returnStr += "Probability Distr: Deviance = " + PrSDr + " Mean: " + Prmeanr;
                        break;

                    case StatisticType.BinomialDistribution:
                        // hypothesis, probOfSuccess (0-1), confidence (0-1, optional)
                        var binDis = new BinomialDistributionStatistic();
                        keyValues = binDis.OperateOrdinalData(data, 0.5);
                        keyValues.TryGetValue("P-Value", out var Pr);
                        returnStr += "Binomial Distribution:" + Pr;
                        break;

                    case StatisticType.LeastSquaresLine:
                        // /outputs slope and intercept
                        var leastSqr = new LeastSquareLineStatistic();
                        keyValues = leastSqr.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("slope", out var m);
                        keyValues.TryGetValue("intercept", out var b);
                        returnStr += "Least Square Line: y = " + m + "x + " + b;
                        break;

                    case StatisticType.ChiSquare:
                        var chiSqr = new ChiSquareStatistic();
                        keyValues = chiSqr.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("chi-square", out var ChSqr);
                        returnStr += "Chi Square:" + ChSqr;
                        break;

                    case StatisticType.CorrelationCoefficient:
                        var corrCoe = new CorrelationCoefficientStatistic();
                        keyValues = corrCoe.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("coeff", out var CCr);
                        returnStr += "Correlation Coefficient:" + CCr;
                        break;

                    case StatisticType.SignTest:
                        // comparisonType, valueToCompareAgainst
                        var sign = new SignTestStatistic();
                        keyValues = sign.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("sign-prop", out var SPr);
                        returnStr += "Sign Test:" + SPr;
                        break;

                    case StatisticType.RankSumTest:

                        var rankSum = new RankSumTestStatistic();
                        keyValues = rankSum.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("ranksum", out var RSr);
                        returnStr += "Rank Sum:" + RSr;
                        break;

                    case StatisticType.SpearmanRankCorrelationCoefficient:
                        var spearman = new SpearmanRankCorrelationStatistic();
                        keyValues = spearman.OperateOrdinalData(data, null);
                        keyValues.TryGetValue("spearman", out var SRCr);
                        returnStr += "Spearman Rank Correlation:" + SRCr;
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