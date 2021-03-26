using System.Collections.Generic;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel
{
    internal class Interlayer
    {
        private readonly List<List<double>> _doubles = new();
        private readonly List<List<int>> _ints = new();
        private List<List<string>> _strings = new();

        public bool doStatistics(List<List<string>> input, StatisticType stat)
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

            switch (stat)
            {
                case StatisticType.Mean:
                    return true;
                    break;
                case StatisticType.Median:
                    return true;
                    break;
                case StatisticType.Mode:
                    return true;
                    break;
                case StatisticType.StandardDeviation:
                    return true;
                    break;
                case StatisticType.Variance:
                    return true;
                    break;
                case StatisticType.CoefficientOfVariance:
                    return true;
                    break;
                case StatisticType.Percentile:
                    // Need parameter between 0 and 1 for percentile
                    return true;
                    break;
                case StatisticType.ProbabilityDistribution:
                    // Outputs mean and stdev
                    return true;
                    break;
                case StatisticType.BinomialDistribution:
                    // Outputs something.TBD
                    return true;
                    break;
                case StatisticType.LeastSquaresLine:
                    // /outputs slope and intercept
                    return true;
                    break;
                case StatisticType.ChiSquare:
                    // TODO: Learn.
                    return true;
                    break;
                case StatisticType.CorrelationCoefficient:
                    return true;
                    break;
                case StatisticType.SignTest:
                    // Complicated.
                    return true;
                    break;
                case StatisticType.RankSumTest:
                    // Complicated.
                    return true;
                    break;
                case StatisticType.SpearmanRankCorrelationCoefficient:
                    return true;
                    break;
                default:
                    return false;
            }
        }
    }
}