using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Algorithms.DynamicProgramming.Tests
{
    [TestClass()]
    public class SequenceAlignmentAlgorithmUnitTest
    {
        [TestMethod()]
        public void GetAlignmentTest()
        {
            Case1();
            Case2();
        }

        private class SequenceAlignmentInput
        {
            public string[] First { get; set; }

            public string[] Second { get; set; }

            public int Penalty { get; set; }

            public int GapPenalty { get; set; }
        }

        private class SequenceAlignmentOutput
        {
            public string[] First { get; set; }

            public string[] Second { get; set; }

            public int Penalty { get; set; }
        }

        private void TestAlgorithm(SequenceAlignmentInput input, SequenceAlignmentOutput expected)
        {
            var caseName = new StackTrace().GetFrame(1).GetMethod().Name;
            var algorithm = new SequenceAlignmentAlgorithm<string>();

            (var actualFirst, var actualSecond, var actualPenalty) = algorithm.GetAlignment(input.First, input.Second, input.Penalty, input.GapPenalty);

            Assert.AreEqual(expected.Penalty, actualPenalty, $"Invalid penalty for :{caseName}");
            
            void AssertArray(string[] expected, string[] actual, string message)
            {
                Assert.AreEqual(expected.Length, actual.Length, $"Invalid length for: {message} for case :{caseName}");
                for (var i = 0; i < expected.Length; ++i)
                    Assert.AreEqual(expected[i], actual[i], $"Invalid element for index:{i} for: {message} for case :{caseName}");
            }
            AssertArray(expected.First, actualFirst, "first");
            AssertArray(expected.Second, actualSecond, "second");
        }

        private void Case1()
            => TestAlgorithm(
                new SequenceAlignmentInput
                {
                    First = new string[] { "A", "G", "G", "G", "C", "T" },
                    Second = new string[] { "A", "G", "G", "C", "A" },
                    GapPenalty = 1,
                    Penalty = 1
                },
                new SequenceAlignmentOutput
                {
                    First = new string[] { "A", "G", "G", "G", "C", "T" },
                    Second = new string[] { "A", null, "G", "G", "C", "A" },
                    Penalty = 2
                }
                );

        private void Case2()
            => TestAlgorithm(
                new SequenceAlignmentInput
                {
                    First = new string[] { "A", "G", "T", "A", "C", "G" },
                    Second = new string[] { "A", "C", "A", "T", "A", "G" },
                    GapPenalty = 1,
                    Penalty = 2
                },
                new SequenceAlignmentOutput
                {
                    First = new string[] { "A", null, "G", "T", "A", "C", "G" },
                    Second = new string[] { "A", "C", "A", "T", "A", null, "G" },
                    Penalty = 4
                }
                );
    }
}