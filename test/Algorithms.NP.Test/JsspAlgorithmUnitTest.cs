using Algorithms.NP.Jssp;
using Algorithms.NP.Jssp.Algorithms;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Algorithms.NP.Test
{
    [TestClass]
    public class JsspAlgorithmUnitTest
    {
        [DataTestMethod]
        [DataRow(new int[] { 2, 2, 3, 1}, 2, 4, DisplayName = "1")]
        [DataRow(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5 }, 5, 5, DisplayName = "2")]
        [DataRow(new int[] { 5, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9 }, 5, 19, DisplayName = "3")]
        public void GrahamTest(int[] jobs, int machinesCount, int expectedMakespan)
        {
            var algorithm = new GrahamAlgorithm();
            var input = new AlgorithmInput
            {
                Jobs = jobs.Select((x, index) => new Job { Id = index + 1, Duration = x }),
                Machines = Enumerable.Range(0, machinesCount).Select((x, index) => new Machine { Id = index })
            };
            var output = algorithm.FindSolution(input);

            output.Makespan.Should().Be(expectedMakespan);
            
        }
    }
}
