using System;
using System.Collections.Generic;
using System.Linq;
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

        [TestMethod]
        public void Utilities_Flatten_2d()
        {
            var data = TestHelperFunctions.GetSmallData2d();

            var expected = new List<double>
            {
                0.0, 1.0, 2.0, 0.0, 1.0, 2.0, 0.0, 1.0, 2.0, 0.0, 1.0, 2.0, 0.0, 1.0, 2.0, 5.0, 1.0, 3.0, 5.0, 1.0, 3.0,
                3.0, 1.0, 3.0, 5.0, 1.0, 3.0, 5.0, 1.0, 3.0, 5.0, 1.0, 3.0, 5.0, 1.0, 3.0, 5.0, 2.0, 0.0, 1.0, 2.0, 0.0,
                1.0,
                3.0
            };

            var actual = Utilities.Flatten(data).ToList();
            TestHelperFunctions.AssertListsAreEqual(expected, actual);
        }

        [TestMethod]
        public void Utilities_ConvertToFrequencyValues()
        {
            var data = TestHelperFunctions.GetStringData();

            var expected = new Dictionary<string, int>
            {
                {"cat", 3},
                {"rat", 2},
                {"dog", 2},
                {"doggo", 1},
                {"meow", 1},
                {"cow", 1},
                {"parrot", 1},
                {"johnson", 1}
            };

            Dictionary<string, int> actual = Utilities.ConvertToFrequencyValues(data)
                .ToDictionary(kv => kv.Key.ToString(), kv => kv.Value)!;

            TestHelperFunctions.AssertDictionariesAreEqual(expected, actual);
        }

        [TestMethod]
        public void Utilities_ConvertToIntervalData()
        {
            var data = Utilities.Flatten(TestHelperFunctions.GetSmallData1d());

            var intervalDefinitions = TestHelperFunctions.GetSmallDataIntervalDefinitions();

            var actual = Utilities.ConvertToIntervalData(data, intervalDefinitions);

            var expected = new Dictionary<string, IEnumerable<double>>
            {
                {
                    "one", new List<double>
                    {
                        1.0, 1.0, 1.0, 1.0, 1.0
                    }
                },
                {
                    "zero", new List<double>
                    {
                        0, 0, 0, 0, 0, 0
                    }
                },
                {
                    "two", new List<double>
                    {
                        2, 2, 2, 2, 2
                    }
                }
            };


            Assert.IsTrue(actual.Count == expected.Count,
                $"The actual length ({actual.Count}) and expected length ({expected.Count}) are different!");

            foreach (var (expectedKey, expectedValue) in expected)
            {
                Assert.IsTrue(actual.ContainsKey(expectedKey),
                    $"Actual data does not contain expected key {expectedKey}");
                TestHelperFunctions.AssertListsAreEqual(expectedValue.ToList(), actual[expectedKey].ToList());
            }

            foreach (var (actualKey, actualValue) in actual)
            {
                Assert.IsTrue(expected.ContainsKey(actualKey),
                    $"Expected data does not contain actual key {actualKey}");
                TestHelperFunctions.AssertListsAreEqual(expected[actualKey].ToList(), actualValue.ToList());
            }
        }

        [TestMethod]
        public void Utilities_ConvertFromIntervalDataToFrequencyData()
        {
            var data = TestHelperFunctions.GetSmallData1d();
            var intervalDefinitions = TestHelperFunctions.GetSmallDataIntervalDefinitions();

            var intervalData = Utilities.ConvertToIntervalData(Utilities.Flatten(data), intervalDefinitions);
            var actual = new FrequencyData<string>(Utilities
                .ConvertFromIntervalDataToFrequencyValues<string>(intervalData)
                .ToDictionary(kv => kv.Key.ToString()!, kv => kv.Value)!);


            var expected = new Dictionary<string, int>
            {
                {"zero", 6},
                {"one", 5},
                {"two", 5}
            };

            TestHelperFunctions.AssertDictionariesAreEqual(expected, actual);
        }

        [TestMethod]
        public void Utilities_ConvertFromIntervalToOrdinalData()
        {
            var data = TestHelperFunctions.GetSmallData1d();
            var intervalDefinitions = TestHelperFunctions.GetSmallDataIntervalDefinitions();

            var intervalData = Utilities.ConvertToIntervalData(Utilities.Flatten(data), intervalDefinitions);
            var actual = Utilities.ConvertFromIntervalDataToOrdinalData(intervalData).ToList();

            var expected = Utilities.Flatten(data).ToList();

            TestHelperFunctions.AssertListsAreEqual(expected, actual);
        }
    }
}