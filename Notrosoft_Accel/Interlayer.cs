using System;
using System.Collections.Generic;
using System.Text;

namespace Notrosoft_Accel
{
    class Interlayer
    {
        List<List<string>> _strings = new List<List<string>>();
        List<List<double>> _doubles = new List<List<double>>();
        List<List<int>> _ints = new List<List<int>>();

        public bool doStatistics(List<List<string>> input, string stat)
        {
            // Ensure List is horizontal, not vertical.
            if (input.Count < input[0].Count)
            {
                for (int i = 0; i < input[0].Count; i++)
                {
                    for (int j = 0; j < input.Count; j++)
                    {
                        _strings[i][j] = input[j][i];
                    }
                }
            }
            else _strings = input;

            // Assign values for _doubles and _ints
            for (int i = 0; i < _strings.Count; i++)
            {
                for (int j = 0; j < _strings[0].Count; j++)
                {
                    _doubles[i][j] = Double.Parse(_strings[i][j]);
                    _ints[i][j] = int.Parse(_strings[i][j]);
                }
            }

            switch (stat)
            {
                case "COVS": // CoefficientOfVarianceStatistic
                    return true;
                case "IS":   // IStatistic
                    return true;
                case "LSLS": // LeastSquareLineStatistic
                    return true;
                case "Mean": // MeanStatistic
                    return true;
                case "MedS": // MedianStatistic
                    return true;
                case "Mode": // ModeStatistic
                    return true;
                case "PS":   // PercentileStatistic
                    return true;
                case "SDS":  // StandardDeviationStatistic
                    return true;
                case "VarS": // VarianceStatistic
                    return true;
                default:     // Unknown case
                    return false;
            }
        }
    }
}
