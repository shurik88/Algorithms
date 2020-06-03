using Algorithms.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Algorithm.Sorting.Test
{
    [TestClass]
    public class SelectUnitTest
    {
        //[TestMethod]
        //public void FailMethod()
        //{
        //    Assert.Fail("Fake fail test");
        //}

        [TestMethod]
        public void RSelectTestMethod()
        {
            var rand = new Random();
            var select = new RSelectAlgorithm(rand);

            void AssertStatistics(int[] arr, int statisticsNumber)
            {
                var statisticsI = select.FindIStatistics(arr, statisticsNumber);
                Assert.AreEqual(arr.OrderBy(x => x).ElementAt(statisticsNumber), statisticsI, 
                    $"Invalid statictics element for statistics:{statisticsNumber} for array: {string.Join(",", arr)}");
            }
            var constArray = new int[] { 2, 7, 1, 0, -5, 6, 4, 3, 1, 10, 19, 7, 9, 8, 2, 5 };
            AssertStatistics(constArray, 3);
            //Assert.AreEqual(2, statistics3);
            AssertStatistics(constArray, 0);
            //Assert.AreEqual(-5, statistics0);
            //var statistics5 = select.FindIStatistics(constArray, 5);
            AssertStatistics(constArray, 5);
            //Assert.AreEqual(2, statistics5);
            var array = Enumerable.Range(0, 20).Select(x => rand.Next(-10, 11)).ToArray();
            for (var i = 0; i < 10; ++i)
            {
                var statisticsNumber = rand.Next(0, array.Length);
                AssertStatistics(array, statisticsNumber);
            }




        }
    }
}
