using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notrosoft_Accel.Infrastructure;

namespace Notrosoft_Accel.Tests
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void Utilities_Combination()
        {
            const int top = 13;
            const int bottom = 2;
            const double expected = 78;

            var actual = Utilities.Combination(top, bottom);

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Utilities_Combination_BadInput()
        {
            const int top = 2;
            const int bottom = 13;

            Assert.ThrowsException<InvalidOperationException>(() => Utilities.Combination(top, bottom));
        }

        [TestMethod]
        public void Utilities_Factorial()
        {
            const int n = 5;
            const double expected = 120;

            var actual = Utilities.Factorial(n);
            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Utilities_Factorial_BaseCase()
        {
            const int n = 0;
            const double expected = 1;

            var actual = Utilities.Factorial(n);

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Utilities_Factorial_TooBig()
        {
            var n = Utilities.MaxFactorialInput + 1;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Utilities.Factorial(n));
        }

        [TestMethod]
        public void Utilities_Covariance()
        {
            var data = TestHelperFunctions.GetSmallData2d();
            var x = data[0];
            var y = data[1];

            const double expected = 0.5541125541;

            var actual = Utilities.GetCovariance(x, y);

            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Utilities_Covariance_LargeData()
        {
            var data = TestHelperFunctions.GetLargeData2d();
            var x = data[0];
            var y = data[1];

            const double expected = 19356.21264;
            var actual = Utilities.GetCovariance(x, y);
            Assert.AreEqual(expected, actual, TestHelperFunctions.Tolerance);
        }

        [TestMethod]
        public void Utilities_Covariance_EmptyData()
        {
            var data = TestHelperFunctions.GetEmpty2dData();
            var x = data[0];
            var y = data[1];

            Assert.ThrowsException<InvalidOperationException>(() => Utilities.GetCovariance(x, y));
        }

        [TestMethod]
        public void Utilities_Covariance_UnequalLengths()
        {
            var data = TestHelperFunctions.GetIrregularLength2dData();

            var x = data[0];
            var y = data[1];

            Assert.ThrowsException<InvalidOperationException>(() => Utilities.GetCovariance(x, y));
        }

        public void Utilities_Flatten_2d()
        {
            var data = TestHelperFunctions.GetSmallData2d();

            IEnumerable<double> expected = new List<double>
            {
                0.0, 1.0, 2.0, 0.0, 1.0, 2.0, 0.0, 1.0, 2.0, 0.0, 1.0, 2.0, 0.0, 1.0, 2.0, 5.0, 1.0, 3.0, 5.0, 1.0, 3.0,
                3.0, 1.0, 3.0, 5.0, 1.0, 3.0, 5.0, 1.0, 3.0, 5.0, 1.0, 3.0, 5.0, 1.0, 3.0, 5.0, 2.0, 0.0, 1.0, 2.0, 0.0,
                1.0,
                3.0
            };

            var actual = Utilities.Flatten(data);

            Assert.AreEqual(expected, actual);
        }
    }
}