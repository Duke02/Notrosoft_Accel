using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notrosoft_Accel.Backend.Statistics;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Tests
{
    [TestClass]
    public class StatisticsTest
    {
        [TestMethod]
        public void Statistics_Mode_ExceptionTest()
        {
            var modeStatistic = new ModeStatistic();

            var data = TestHelperFunctions.GetEmpty1dData();

            Assert.ThrowsException<InvalidOperationException>(() => modeStatistic.DoOrdinalData(data));
        }

        [TestMethod]
        public void Statistics_Mode_Test1()
        {
            var modeStatistic = new ModeStatistic();

            var data = TestHelperFunctions.GetSmallData1d();

            var expected = 0;
            var actual = (double)modeStatistic.DoOrdinalData(data)["mode"];


            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance,
                "Mode Statistic does not give the correct value!");
        }

        [TestMethod]
        public void Statistics_Mode_LotsOfNumbers()
        {
            var modeStatistic = new ModeStatistic();

            var data = TestHelperFunctions.GetLargeData1d();

            var expected = 1.0;
            var actual = (double)modeStatistic.DoOrdinalData(data)["mode"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance,
                "Mode Statistic does not give the correct value!");
        }

        [TestMethod]
        public void Statistics_Mean_Test()
        {
            var meanStatistic = new MeanStatistic();
            var data = TestHelperFunctions.GetSmallData1d();

            var expected = 0.9375;
            var actual = (double)meanStatistic.DoOrdinalData(data)["mean"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance,
                "Mean Statistic does not give the correct value!");
        }

        [TestMethod]
        public void Statistics_Mean_LotsOfNumbers()
        {
            var meanStatistic = new MeanStatistic();
            var data = TestHelperFunctions.GetLargeData1d();

            var expected = 150.3292683;
            var actual = (double)meanStatistic.DoOrdinalData(data)["mean"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance,
                "Mean Statistic does not give the correct value!");
        }

        [TestMethod]
        public void Statistics_Mean_ExceptionTest()
        {
            var meanStatistic = new MeanStatistic();
            var data = TestHelperFunctions.GetEmpty1dData();

            Assert.ThrowsException<InvalidOperationException>(() => meanStatistic.DoOrdinalData(data));
        }

        [TestMethod]
        public void Statistics_Median_ExceptionTest()
        {
            var medianStatistic = new MedianStatistic();
            var data = TestHelperFunctions.GetEmpty1dData();

            Assert.ThrowsException<InvalidOperationException>(() => medianStatistic.DoOrdinalData(data));
        }

        [TestMethod]
        public void Statistics_Median_TestOdd()
        {
            var medianStatistic = new MedianStatistic();
            var data = TestHelperFunctions.GetSmallData1dOdd();

            var expected = 1.0;
            var actual = (double)medianStatistic.DoOrdinalData(data)["median"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_Median_TestEven()
        {
            var medianStatistic = new MedianStatistic();
            var data = TestHelperFunctions.GetSmallData1dEven();

            var expected = 1.0;
            var actual = (double)medianStatistic.DoOrdinalData(data)["median"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_Median_LotsOfNumbers()
        {
            var medianStatistic = new MedianStatistic();
            var data = TestHelperFunctions.GetLargeData1d();

            var expected = 124;
            var actual = (double)medianStatistic.DoOrdinalData(data)["median"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_Variance_LotsOfNumbers()
        {
            var varianceStatistic = new VarianceStatistic();
            var data = TestHelperFunctions.GetLargeData1d();

            var expected = 13514.36719;
            var actual = (double)varianceStatistic.DoOrdinalData(data)["variance"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_Variance()
        {
            var varianceStatistic = new VarianceStatistic();
            var data = TestHelperFunctions.GetSmallData1d();

            var expected = 0.68359375;
            var actual = (double)varianceStatistic.DoOrdinalData(data)["variance"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_Variance_ExceptionTest()
        {
            var varianceStat = new VarianceStatistic();
            var data = TestHelperFunctions.GetEmpty1dData();

            Assert.ThrowsException<InvalidOperationException>(() => varianceStat.DoOrdinalData(data));
        }

        [TestMethod]
        public void Statistics_StandardDeviation_ExceptionTest()
        {
            var stddevStat = new StandardDeviationStatistic();
            var data = TestHelperFunctions.GetEmpty1dData();

            Assert.ThrowsException<InvalidOperationException>(() => stddevStat.DoOrdinalData(data));
        }

        [TestMethod]
        public void Statistics_StandardDeviation()
        {
            var stddevStatistic = new StandardDeviationStatistic();
            var data = TestHelperFunctions.GetSmallData1d();

            var expected = 0.8267972847;
            var actual = (double)stddevStatistic.DoOrdinalData(data)["Standard Deviation"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_StandardDeviation_LotsOfNumbers()
        {
            var stddevStatistic = new StandardDeviationStatistic();
            var data = TestHelperFunctions.GetLargeData1d();

            var expected = 116.2513105;
            var actual = (double)stddevStatistic.DoOrdinalData(data)["Standard Deviation"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_CoeffVariation()
        {
            var cvStatistic = new CoefficientOfVarianceStatistic();
            var data = TestHelperFunctions.GetSmallData1d();

            var expected = 0.8819171037;
            var actual = (double)cvStatistic.DoOrdinalData(data)["cv"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_CoeffVariation_LotsOfNumbers()
        {
            var cvStatistic = new CoefficientOfVarianceStatistic();
            var data = TestHelperFunctions.GetLargeData1d();

            var expected = 0.7733112242;
            var actual = (double)cvStatistic.DoOrdinalData(data)["cv"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_CoeffVariation_ExceptionTest()
        {
            var cvStat = new CoefficientOfVarianceStatistic();
            var data = TestHelperFunctions.GetEmpty1dData();

            Assert.ThrowsException<InvalidOperationException>(() => cvStat.DoOrdinalData(data));
        }

        [TestMethod]
        public void Statistics_BinDist_FailRejectNullHypothesis()
        {
            var binDistStat = new BinomialDistributionStatistic();
            var data = TestHelperFunctions.GetSmallData1d();

            const double hypothesis = 1;
            const double probSuccess = 1.0 / 3.0;

            const double expectedPValue = 0.7374313133;
            const bool expectedShouldReject = false;

            var actualOutput = binDistStat.DoOrdinalData((IEnumerable<OrdinalData>) data, hypothesis, probSuccess);

            var actualPValue = (double)actualOutput["P-Value"];
            var actualShouldReject = (bool)actualOutput["Reject Null Hypothesis?"];

            Assert.AreEqual(expectedPValue, actualPValue, TestHelperFunctions.Tolerance, "P Value is not the same!");
            Assert.AreEqual(expectedShouldReject, actualShouldReject,
                "Could not agree on rejecting the null hypothesis!");
        }

        [TestMethod]
        public void Statistics_BinDist_RejectNullHypothesis()
        {
            var binDistStat = new BinomialDistributionStatistic();
            var data = TestHelperFunctions.GetSmallData1d();

            const double hypothesis = 0;
            const double probSuccess = 1.0 / 3.0;

            const double expectedPValue = 0.00152243884;
            const bool expectedShouldReject = true;

            var actualOutput = binDistStat.DoOrdinalData((IEnumerable<OrdinalData>) data, hypothesis, probSuccess);

            var actualPValue = (double)actualOutput["P-Value"];
            var actualShouldReject = (bool)actualOutput["Reject Null Hypothesis?"];

            Assert.AreEqual(expectedPValue, actualPValue, TestHelperFunctions.Tolerance, "P Value is not the same!");
            Assert.AreEqual(expectedShouldReject, actualShouldReject,
                "Could not agree on rejecting the null hypothesis!");
        }

        [TestMethod]
        public void Statistics_BinDist_CustomConfidence()
        {
            var binDistStat = new BinomialDistributionStatistic();
            var data = TestHelperFunctions.GetSmallData1d();

            const double hypothesis = 0;
            const double probSuccess = 1.0 / 3.0;
            const double confidence = .999;


            const double expectedPValue = 0.00152243884;
            const bool expectedShouldReject = false;

            var actualOutput = binDistStat.DoOrdinalData((IEnumerable<OrdinalData>) data, hypothesis, probSuccess, confidence);

            var actualPValue = (double)actualOutput["P-Value"];
            var actualShouldReject = (bool)actualOutput["Reject Null Hypothesis?"];

            Assert.AreEqual(expectedPValue, actualPValue, TestHelperFunctions.Tolerance, "P Value is not the same!");
            Assert.AreEqual(expectedShouldReject, actualShouldReject,
                "Could not agree on rejecting the null hypothesis!");
        }

        [TestMethod]
        public void Statistics_BinDist_Empty()
        {
            var binDistStat = new BinomialDistributionStatistic();
            var data = TestHelperFunctions.GetEmpty1dData();

            const double hypothesis = 0;
            const double probSuccess = 1.0 / 3.0;
            const double confidence = .999;

            Assert.ThrowsException<InvalidOperationException>(() =>
                binDistStat.DoOrdinalData((IEnumerable<OrdinalData>) data, hypothesis, probSuccess, confidence));
        }

        [TestMethod]
        public void Statistics_CorrelationCoeff()
        {
            var corrCoeff = new CorrelationCoefficientStatistic();

            var data = TestHelperFunctions.GetSmallData2d();

            const double expected = 0.2210526794;

            var actual = (double)corrCoeff.DoOrdinalData(data)["coeff"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_LeastSquareLine()
        {
            var stat = new LeastSquareLineStatistic();

            var data = TestHelperFunctions.GetSmallData2d();

            const double expectedSlope = 0.2580645161;
            const double expectedYIntercept = 2.032258065;

            var actualOutput = stat.DoOrdinalData(data);
            var actualSlope = (double)actualOutput["slope"];
            var actualYIntercept = (double)actualOutput["intercept"];

            Assert.AreEqual(expectedSlope, actualSlope, TestHelperFunctions.Tolerance);
            Assert.AreEqual(expectedYIntercept, actualYIntercept, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_NormalDist()
        {
            var stat = new NormalDistribution();

            var data = TestHelperFunctions.GetSmallData1d();

            const double expectedVariance = 0.68359375;
            const double expectedMean = 0.9375;

            var actualOutput = stat.DoOrdinalData(data);

            var actualMean = (double)actualOutput["mean"];
            var actualVariance = (double)actualOutput["variance"];

            Assert.AreEqual(expectedMean, actualMean, TestHelperFunctions.Tolerance);
            Assert.AreEqual(expectedVariance, actualVariance, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_PercentileTest()
        {
            var stat = new PercentileStatistic();

            var data = TestHelperFunctions.GetSmallData1d();

            const double expectedTenth = 0;
            const double expectedQuarter = 0;
            const double expectedThirdQuarter = 2;
            const double expectedNinety = 2;

            var tenth = (double)stat.DoOrdinalData((IEnumerable<OrdinalData>) data, .1)["percentile"];
            var quarter = (double)stat.DoOrdinalData((IEnumerable<OrdinalData>) data, .25)["percentile"];
            var thirdQuarter = (double)stat.DoOrdinalData((IEnumerable<OrdinalData>) data, .75)["percentile"];
            var ninety = (double)stat.DoOrdinalData((IEnumerable<OrdinalData>) data, 0.9)["percentile"];

            Assert.AreEqual(expectedTenth, tenth, TestHelperFunctions.Tolerance);
            Assert.AreEqual(expectedQuarter, quarter, TestHelperFunctions.Tolerance);
            Assert.AreEqual(expectedThirdQuarter, thirdQuarter, TestHelperFunctions.Tolerance);
            Assert.AreEqual(expectedNinety, ninety, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_ChiSquareTest()
        {
            var stat = new ChiSquareStatistic();

            var baseData = TestHelperFunctions.GetSmallData1d();
            var intervalDefinitions = TestHelperFunctions.GetSmallDataIntervalDefinitions();

            var intervalData = Utilities.ConvertToIntervalData(Utilities.Flatten(baseData), intervalDefinitions);

            const double expected = 5.666666666666666;

            var actual = (double)stat.DoIntervalData(new List<IntervalData> { intervalData })["chi-square"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_RankSumTest()
        {
            var stat = new RankSumTestStatistic();
            var data = TestHelperFunctions.GetSmallData2d();

            const double expected = 174.5;

            var actual = (double)stat.DoOrdinalData(data)["ranksum"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_SignTest()
        {
            var stat = new SignTestStatistic();

            var data = TestHelperFunctions.GetSmallData2d();

            const double expected = 0.0669002533;

            var actual = (double)stat.DoOrdinalData(data)["sign-prop"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_SpearmanRankCorrTest()
        {
            var stat = new SpearmanRankCorrelationStatistic();

            var data = TestHelperFunctions.GetSmallData2d();

            const double expected = 0.3982620897;

            var actual = (double)stat.DoOrdinalData(data)["spearman"];

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Statistics_MeanIntervalTest()
        {
            var stat = new MeanStatistic();

            var data = TestHelperFunctions.GetIntervalData();

            var actual = stat.DoIntervalData(new List<IntervalData> { data })["mean"];
        }
    }
}