using Algorithms.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Algorithm.Sorting.Test
{
    [TestClass]
    public class SortUnitTest
    {

        private static void AssertSort(int[] array, ISortAlgorithm sortAlgorithm)
        {
            var copy = new int[array.Length];
            array.CopyTo(copy, 0);
            sortAlgorithm.Sort(copy);
            var orderedArray = array.OrderBy(x => x).ToArray();
            for(var i = 0; i < orderedArray.Length; ++i)
                Assert.AreEqual(orderedArray[i], copy[i], $"Invalid element in position: {i} of array: {string.Join(",", array)}s");
        }

        [TestMethod]
        public void QuickSortWithFirstElementPartitionTest()
        {
            var rand = new Random();
            var array = Enumerable.Range(0, 20).Select(x => rand.Next(-20, 21)).ToArray();
            var algorithm = new QuickSortAlgorithm(new FirstElementPartitionStrategy());
            AssertSort(array, algorithm);
        }

        [TestMethod]
        public void QuickSortWithRandElementPartitionTest()
        {
            var rand = new Random();
            var array = Enumerable.Range(0, 20).Select(x => rand.Next(-20, 21)).ToArray();
            var algorithm = new QuickSortAlgorithm(new RandElementPartitionStrategy(rand));
            AssertSort(array, algorithm);
        }

        [TestMethod]
        public void MergeSorTest()
        {
            var rand = new Random();
            var array = Enumerable.Range(0, 21).Select(x => rand.Next(-20, 21)).ToArray();
            var algorithm = new MergeSortAlgorithm();
            AssertSort(array, algorithm);
        }
    }
}
