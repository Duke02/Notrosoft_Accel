using System;
using System.Collections.Generic;
using System.Linq;
using Notrosoft_Accel.Backend.Statistics;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel
{
    internal class Interlayer
    {
        private readonly List<List<double>> _doubles = new();
        private List<List<string>> _strings = new();

        public string doStatistics(List<List<string>> input, StatisticType[] stats, DataType dataType,
            IntervalDefinitions intervals, params object[] param)
        {
            // _doubles.Clear();
            // _strings.Clear();
            // var s = new List<string>();
            // // Ensure List is always presented in the same manner
            // if (input.Count > input[0].Count)
            //     for (var i = 0; i < input.Count; i++)
            //     {
            //         s.Clear();
            //         for (var j = 0; j < input[0].Count; j++) s.Add(input[i][j]);
            //         _strings.Add(s);
            //     }
            // else _strings = input;

            // It should just be 
            var numColumns = input.Count;
            var convertedData = Utilities.ConvertData(input, dataType, numColumns, intervals).ToList();

            // Set the return variable for statistic operations.
            // var keyValues = new Dictionary<string, object>();
            // String constructed based off the statistic performed.
            // var returnStr = "";
            // String used for when a Statistical method is not implemented quite yet.
            // var NaS = "Statistic method has not been added yet";

            // var interData = new List<IntervalData>();
            // var ordData = _doubles.Select(d => new OrdinalData(d)).ToList();
            // if (intervals != null)
            // {
            //     var tempInter = new IntervalDefinitions(intervals);
            //     interData = _doubles.Select(ds => Utilities.ConvertToIntervalData(ds, tempInter)).ToList();
            // }

            var output = new List<string>(stats.Length);
            
            foreach (var statType in stats)
            {
                var stat = GetStatistic(statType);
                var statOutput = stat.Operate(convertedData, dataType, param);
                var joinedKeys = string.Join("\n", statOutput.Select(kv => $"{kv.Key}: {kv.Value}"));
                output.Add(joinedKeys);
            }

            return string.Join("\n", output);

                // TODO: This might be decent?
                // return string.Join("\n",
                //     stats.Select(sType => GetStatistic(sType).Operate(convertedData, dataType, param))
                //         .Select(d => string.Join("\n", d.Select(kv => $"{kv.Key}: {kv.Value}"))));

            // foreach (var statType in stats)
            // {
            //     var stat = GetStatistic(statType);
            //     var output = stat.Operate(convertedData, dataType, param);
            //
            //     returnStr += string.Join("\n", output.Select(kv => $"{kv.Key}: {kv.Value}"));
            //     returnStr += "\n";
            //     
            //     // switch (statType)
            //     // {
            //     //     case StatisticType.Mean:
            //     //         stat = new MeanStatistic();
            //     //         keyValues = stat.Operate(convertedData, dataType);
            //     //         // keyValues.TryGetValue("mean", out var meanr);
            //     //         // returnStr += "Mean: ";
            //     //         break;
            //     //
            //     //     case StatisticType.Median:
            //     //         var median = new MedianStatistic();
            //     //         keyValues = median.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("median", out var medr);
            //     //         returnStr += "Median: " + medr;
            //     //         break;
            //     //
            //     //     case StatisticType.Mode:
            //     //         var mode = new ModeStatistic();
            //     //         keyValues = mode.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("mode", out var moder);
            //     //         returnStr += "Mode: " + moder;
            //     //         break;
            //     //
            //     //     case StatisticType.StandardDeviation:
            //     //         var sDev = new StandardDeviationStatistic();
            //     //         keyValues = sDev.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("Standard Deviation", out var SDr);
            //     //         returnStr += "Standard Deviation: " + SDr;
            //     //         break;
            //     //
            //     //     case StatisticType.Variance:
            //     //         var variance = new VarianceStatistic();
            //     //         keyValues = variance.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("variance", out var Var);
            //     //         returnStr += "Variance:" + Var;
            //     //         break;
            //     //
            //     //     case StatisticType.CoefficientOfVariance:
            //     //         var coeffVar = new CoefficientOfVarianceStatistic();
            //     //         keyValues = coeffVar.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("cv", out var CVr);
            //     //         returnStr += "Coefficient of Variance:" + CVr;
            //     //         break;
            //     //
            //     //     case StatisticType.Percentile:
            //     //         // Need parameter between 0 and 1 for percentile
            //     //         var percentile = new PercentileStatistic();
            //     //         keyValues = percentile.DoOrdinalData((IEnumerable<OrdinalData>) ordData, 0.5);
            //     //         keyValues.TryGetValue("percentile", out var Per);
            //     //         returnStr += "Percentile:" + Per;
            //     //         break;
            //     //
            //     //     case StatisticType.ProbabilityDistribution:
            //     //         // Outputs mean and stdev
            //     //         var PrsDev = new StandardDeviationStatistic();
            //     //         keyValues = PrsDev.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("Standard Deviation", out var PrSDr);
            //     //         var Prmean = new MeanStatistic();
            //     //         keyValues = Prmean.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("mean", out var Prmeanr);
            //     //         returnStr += "Probability Distr: Deviance = " + PrSDr + " Mean = " + Prmeanr;
            //     //         break;
            //     //
            //     //     case StatisticType.BinomialDistribution:
            //     //         // hypothesis, probOfSuccess (0-1), confidence (0-1, optional)
            //     //         var binDis = new BinomialDistributionStatistic();
            //     //         keyValues = binDis.DoOrdinalData((IEnumerable<OrdinalData>) ordData, 0.5);
            //     //         keyValues.TryGetValue("P-Value", out var Pr);
            //     //         returnStr += "Binomial Distribution:" + Pr;
            //     //         break;
            //     //
            //     //     case StatisticType.LeastSquaresLine:
            //     //         // /outputs slope and intercept
            //     //         var leastSqr = new LeastSquareLineStatistic();
            //     //         keyValues = leastSqr.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("slope", out var m);
            //     //         keyValues.TryGetValue("intercept", out var b);
            //     //         returnStr += "Least Square Line: y = " + m + "x + " + b;
            //     //         break;
            //     //
            //     //     case StatisticType.ChiSquare:
            //     //         var chiSqr = new ChiSquareStatistic();
            //     //         keyValues = chiSqr.DoIntervalData(interData, null);
            //     //         keyValues.TryGetValue("chi-square", out var ChSqr);
            //     //         returnStr += "Chi Square:" + ChSqr;
            //     //         break;
            //     //
            //     //     case StatisticType.CorrelationCoefficient:
            //     //         var corrCoe = new CorrelationCoefficientStatistic();
            //     //         keyValues = corrCoe.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("coeff", out var CCr);
            //     //         returnStr += "Correlation Coefficient:" + CCr;
            //     //         break;
            //     //
            //     //     case StatisticType.SignTest:
            //     //         // comparisonType, valueToCompareAgainst
            //     //         var sign = new SignTestStatistic();
            //     //         keyValues = sign.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("sign-prop", out var SPr);
            //     //         returnStr += "Sign Test:" + SPr;
            //     //         break;
            //     //
            //     //     case StatisticType.RankSumTest:
            //     //
            //     //         var rankSum = new RankSumTestStatistic();
            //     //         keyValues = rankSum.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("ranksum", out var RSr);
            //     //         returnStr += "Rank Sum:" + RSr;
            //     //         break;
            //     //
            //     //     case StatisticType.SpearmanRankCorrelationCoefficient:
            //     //         var spearman = new SpearmanRankCorrelationStatistic();
            //     //         keyValues = spearman.DoOrdinalData((IEnumerable<OrdinalData>) ordData, null);
            //     //         keyValues.TryGetValue("spearman", out var SRCr);
            //     //         returnStr += "Spearman Rank Correlation:" + SRCr;
            //     //         break;
            //     //
            //     //     default:
            //     //         returnStr += NaS;
            //     //         break;
            //     // }
            // }
            //
            // return returnStr;
        }

        private IStatistic GetStatistic(StatisticType statType)
        {
            return statType switch
            {
                StatisticType.Mean => new MeanStatistic(),
                StatisticType.Median => new MedianStatistic(),
                StatisticType.Mode => new ModeStatistic(),
                StatisticType.StandardDeviation => new StandardDeviationStatistic(),
                StatisticType.Variance => new VarianceStatistic(),
                StatisticType.CoefficientOfVariance => new CoefficientOfVarianceStatistic(),
                StatisticType.Percentile => new PercentileStatistic(),
                StatisticType.ProbabilityDistribution => new NormalDistribution(),
                StatisticType.BinomialDistribution => new BinomialDistributionStatistic(),
                StatisticType.LeastSquaresLine => new LeastSquareLineStatistic(),
                StatisticType.ChiSquare => new ChiSquareStatistic(),
                StatisticType.CorrelationCoefficient => new CorrelationCoefficientStatistic(),
                StatisticType.SignTest => new SignTestStatistic(),
                StatisticType.RankSumTest => new RankSumTestStatistic(),
                StatisticType.SpearmanRankCorrelationCoefficient => new SpearmanRankCorrelationStatistic(),
                _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
            };
        }
    }
}