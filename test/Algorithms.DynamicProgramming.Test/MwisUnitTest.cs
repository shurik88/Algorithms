using Algorithms.DynamicProgramming.Mwis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Algorithms.DynamicProgramming.Test
{
    [TestClass]
    public class MwisUnitTest
    {
        [TestMethod]
        public void MwisBruteForceTestMethod()
        {
            TestAlgorithm(new BruteForceMwisAlgorithm());
        }

        [TestMethod]
        public void MwisCacheBruteForceTestMethod()
        {
            TestAlgorithm(new CacheBruteForceMwisAlgorithm());
        }

        [TestMethod()]
        public void UpwardLinearMwisAlgorithmTest()
        {
            TestAlgorithm(new UpwardLinearMwisAlgorithm());
        }

        private void TestAlgorithm(IMwisAlgorithm algorithm)
        {
            SimpleCase1(algorithm);
            SimpleCase2(algorithm);
        }

        private void SimpleCase1(IMwisAlgorithm algorithm) =>
            TestCase(algorithm, new int[] { 3, 8, 2, 1, 9, 3 }, new int[] { 1, 4 });

        private void SimpleCase2(IMwisAlgorithm algorithm) =>
            TestCase(algorithm, new int[] { 3, 6, 2, 4, 1, 10, 2, 7, 8, 2, 4, 9 }, new int[] { 1, 3, 5, 7, 9, 11 });

        private void TestCase(IMwisAlgorithm algorithm, int[] input, int[] expected)
        {
            var res = algorithm.GetVetexesIndexes(input).ToArray();
            AssertAnswer(expected, res);
        }

        private void AssertAnswer(int[] expected, int[] actual)
        {
            var orderedExpected = expected.OrderBy(x => x).ToArray();
            var orderedActual = actual.OrderBy(x => x).ToArray();
            AssertCaseConstraint(orderedActual);
            Assert.AreEqual(expected.Length, actual.Length, "Answer does not include all vertexes or has extra");
            for(var i = 0; i < orderedExpected.Length; ++i)
                Assert.AreEqual(orderedExpected[i], orderedActual[i], $"Not expected vertex index in answer at position: {i}");
        }

        private void AssertCaseConstraint(int[] orderedAnswers)
        {
            for (var i = 0; i < orderedAnswers.Length - 1; ++i)
                Assert.IsTrue(orderedAnswers[i + 1] - orderedAnswers[i] >= 2, $"Index {orderedAnswers[i + 1]} should be greater or equal by 2 than {orderedAnswers[i]}");
        }


    }
}
