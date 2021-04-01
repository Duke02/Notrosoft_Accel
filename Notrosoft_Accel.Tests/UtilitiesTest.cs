﻿using System;
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
    }
}