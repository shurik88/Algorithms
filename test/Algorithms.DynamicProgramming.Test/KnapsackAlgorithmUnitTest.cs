using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Algorithms.DynamicProgramming.Knapsack.Tests
{
    [TestClass()]
    public class KnapsackAlgorithmUnitTest
    {
        [TestMethod()]
        public void KnapsackTest()
        {
            TestAlgorithm(new KnapsackAlgorithm());
        }

        private void TestAlgorithm(IKnapsackAlgorithm algorithm)
        {
            SimpleCase1(algorithm);
            //SimpleCase2(algorithm);
        }

        private void SimpleCase1(IKnapsackAlgorithm algorithm) =>
            TestCase(algorithm, new Thing[] { 
                new Thing { Value = 3, Size = 4 },
                new Thing { Value = 2, Size = 3 },
                new Thing { Value = 4, Size = 2 },
                new Thing { Value = 4, Size = 3 }
            }, 6, new int[] { 2, 3 });

        //private void SimpleCase2(IKnapsackAlgorithm algorithm) =>
        //    TestCase(algorithm, new int[] { 3, 6, 2, 4, 1, 10, 2, 7, 8, 2, 4, 9 }, new int[] { 1, 3, 5, 7, 9, 11 });

        private void TestCase(IKnapsackAlgorithm algorithm, Thing[] input, int maxSize, int[] expected)
        {
            var resIndexes = algorithm.Get(input, maxSize).ToArray();
            var resThings = resIndexes.Select(x => input[x]).ToArray();
            AssertCaseConstraint(resThings, maxSize);
            AssertAnswer(expected, resIndexes);
        }

        private void AssertAnswer(int[] expected, int[] actual)
        {
            var orderedExpected = expected.OrderBy(x => x).ToArray();
            var orderedActual = actual.OrderBy(x => x).ToArray();
            Assert.AreEqual(expected.Length, actual.Length, "Answer does not include all things or has extra");
            for (var i = 0; i < orderedExpected.Length; ++i)
                Assert.AreEqual(orderedExpected[i], orderedActual[i], $"Not expected thing index in answer at position: {i}");
        }

        private void AssertCaseConstraint(Thing[] answer, int maxSize)
        {
            Assert.IsTrue(!answer.Any() || maxSize >= answer.Sum(x => x.Size), "total size exceeded");
        }
    }
}