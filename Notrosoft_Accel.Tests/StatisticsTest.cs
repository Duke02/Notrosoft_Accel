using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notrosoft_Accel.Statistics;
using System.Collections.Generic;

namespace Notrosoft_Accel.Tests
{
    [TestClass]
    public class StatisticsTest
    {
        private const double _tolerance = 0.001;

        [TestMethod]
        public void Statistics_Mode_Test1()
        {
            var modeStatistic = new ModeStatistic();

            var data = new List<List<double>>();
            for(int i = 0; i < 5; i++)
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


    }
}
