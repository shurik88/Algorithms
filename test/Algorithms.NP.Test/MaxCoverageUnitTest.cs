using Algorithms.NP.MaxCoverage;
using Algorithms.NP.MaxCoverage.Algorithms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Algorithms.NP.Test
{
    [TestClass]
    public class MaxCoverageUnitTest
    {
        [DataTestMethod]
        [DynamicData(nameof(Data), DynamicDataSourceType.Property)]
        public void GreedyMaxCoverageTest(IEnumerable<int[]> sets, int count, int expected)
        {
            var algorithm = new GreedyMaxCoverageAlgorithm();
            var input = new AlgorithmInput
            {
                Sets = sets,
                UsedSetsCount = count
            };
            var output = algorithm.FindSolution(input);

            output.Count.Should().Be(expected);
        }
        private static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] { new List<int[]> {
                    new int[] { 1, 2, 5, 6 },
                    new int[] { 1, 2, 3, 5, 6, 7 },
                    new int[] { 4 },
                    new int[] { 9, 10, 13, 14 },
                    new int[] { 8, 12 },
                    new int[] { 11, 12, 15, 16 }
                }, 4, 15 };
            }
        }
    }
}
