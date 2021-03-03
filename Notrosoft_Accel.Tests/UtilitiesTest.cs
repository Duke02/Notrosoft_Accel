using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Tests
{
    [TestClass]
    public class UtilitiesTest
    {
        private const double Tolerance = 0.001;

        [TestMethod]
        public void Utilities_Flatten_Test1()
        {
            var data = new List<List<double>>();

            for (var i = 0; i < 2; i++)
                data.Add(new List<double>
                {
                    0, 1
                });

            var expected = new double[]
            {
                0, 1, 0, 1
            }.ToList();
            var actual = Utilities.Flatten(data).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Utilities_Covariance_Test1()
        {
            var x = new List<double>
            {
                0, 1, 2, 3
            };
            var y = new List<double>
            {
                1, 2, 3, 4
            };

            var expected = 1.25;
            var actual = Utilities.GetCovariance(x, y);

            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void Utilities_Covariance_Test2()
        {
            var data = new List<double>
            {
                0, 1, 2, 3
            };

            var expected = 1.25;
            var actual = Utilities.GetCovariance(data, data);

            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void Utilities_Variance_Test1()
        {
            var data = new List<double>();

            for (var i = 0; i < 5; i++) data.AddRange(new double[] {0, 1, 2});

            var expected = 0.666666667;
            var actual = Utilities.GetVariance(data);

            Assert.AreEqual(expected, actual, Tolerance);
        }
    }
}