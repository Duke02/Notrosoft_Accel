using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notrosoft_Accel.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notrosoft_Accel.Tests
{
    [TestClass]
    public class StatisticsTest
    {
        private const double _tolerance = 0.001;

        private List<double> GetLotsOfNumbers(int currIndex)
        {
            return Enumerable.Range(1, (currIndex + 1) * (currIndex + 1)).Select(x => (double)x).ToList();
        }

        [TestMethod]
        public void Statistics_Mode_Test1()
        {
            var modeStatistic = new ModeStatistic();

            var data = new List<List<double>>();
            for (int i = 0; i < 5; i++)
            {
                data.Add(new List<double>
                {
                    1, 1, 2, 3, 4
                });
            }

            var expected = 1.0;
            var actual = modeStatistic.Operate(data);

            Assert.AreEqual(expected, actual, _tolerance, $"Mode Statistic does not give the correct value!");
        }

        [TestMethod]
        public void Statistics_Mode_Test2()
        {
            var modeStatistic = new ModeStatistic();

            var data = new List<List<double>>();
            for (int i = 0; i < 5; i++)
            {
                data.Add(new List<double>
                {
                    1, 2, 2, 3, 4
                });
            }

            var expected = 2.0;
            var actual = modeStatistic.Operate(data);

            Assert.AreEqual(expected, actual, _tolerance, $"Mode Statistic does not give the correct value!");
        }

        [TestMethod]
        public void Statistics_Mode_ExceptionTest()
        {
            var modeStatistic = new ModeStatistic();

            var data = new List<List<double>>
            {
                new List<double>()
            };

            Assert.ThrowsException<InvalidOperationException>(() => modeStatistic.Operate(data));
        }

        [TestMethod]
        public void Statistics_Mode_LotsOfNumbers()
        {
            var modeStatistic = new ModeStatistic();

            var data = new List<List<double>>();
            for (int i = 0; i < 25; i++)
            {
                // Makes it 5525 items long.
                // But only 625 distinct items.
                data.Add(GetLotsOfNumbers(i));
            }

            var expected = 1.0;
            var actual = modeStatistic.Operate(data);

            Assert.AreEqual(expected, actual, _tolerance, $"Mode Statistic does not give the correct value!");
        }

        [TestMethod]
        public void Statistics_Mean_Test()
        {
            var meanStatistic = new MeanStatistic();
            var data = new List<List<double>>();
            for (int i = 0; i < 2; i++)
            {
                data.Add(new List<double>
                {
                    1, 0, 1, 1, 0
                });
            }

            var expected = 6.0 / 10.0;
            var actual = meanStatistic.Operate(data);

            Assert.AreEqual(expected, actual, _tolerance, $"Mean Statistic does not give the correct value!");
        }

        [TestMethod]
        public void Statistics_Mean_LotsOfNumbers()
        {
            var meanStatistic = new MeanStatistic();
            var data = new List<List<double>>();
            for (int i = 0; i < 25; i++)
            {
                data.Add(GetLotsOfNumbers(i));
            }

            var expected = 195.4;
            var actual = meanStatistic.Operate(data);

            Assert.AreEqual(expected, actual, _tolerance, $"Mean Statistic does not give the correct value!");
        }

        [TestMethod]
        public void Statistics_Median_TestOdd()
        {
            var medianStatistic = new MedianStatistic();
            var data = new List<List<double>>();
            for (int i = 0; i < 3; i++)
            {
                data.Add(new List<double>
                {
                    1, 1, 2, 3, 4
                });
            }
            // 1X, 1X, 1X, 1X, 1X, 1X, 2X, 2, 2X, 3X, 3X, 3X, 4X, 4X, 4X

            var expected = 2.0;
            var actual = medianStatistic.Operate(data);

            Assert.AreEqual(expected, actual, _tolerance);
        }

        [TestMethod]
        public void Statistics_Median_TestEven()
        {
            var medianStatistic = new MedianStatistic();
            var data = new List<List<double>>();
            for (int i = 0; i < 2; i++)
            {
                data.Add(new List<double>
                {
                    1, 2, 2, 3, 4, 5
                });
            }
            // 1X, 1X, 2X, 2X, 2X, 2, 3, 3X, 4X, 4X, 5X, 5X

            var expected = 2.5;
            var actual = medianStatistic.Operate(data);

            Assert.AreEqual(expected, actual, _tolerance);
        }

        [TestMethod]
        public void Statistics_Median_LotsOfNumbers()
        {
            var medianStatistic = new MedianStatistic();
            var data = new List<List<double>>();
            for (int i = 0; i < 25; i++)
            {
                data.Add(GetLotsOfNumbers(i));
            }
            // 1X, 1X, 2X, 2X, 2X, 2, 3, 3X, 4X, 4X, 5X, 5X

            var expected = 163;
            var actual = medianStatistic.Operate(data);

            Assert.AreEqual(expected, actual, _tolerance);
        }
    }
}
