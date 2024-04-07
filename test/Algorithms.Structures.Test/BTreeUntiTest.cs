using Algorithms.Structures.Trees;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Algorithms.Structures.Test
{
    [TestClass]
    public class BTreeUntiTest
    {
        [TestMethod]
        public void BTreeConstruction()
        {
            var sut = new BTree<int, string>(2);
            //Assert.ThrowsException<InvalidOperationException>(() => sut.Search(1));

            void AssertAddElement(int key, string value, IEnumerable<IEnumerable<int>> distribution, string distributionExpected, int expectedDepth)
            {
                sut.Add(key, value);
                sut.Search(key).Should().Be(value);
                var distributions = sut.KeysInNodes.ToList();
                distributions.SelectMany(x => x).Should().HaveCount(sut.Count, "total elements");

                var expectedDistributions = distribution.ToList();
                distributions.Should().HaveCount(expectedDistributions.Count(), "nodes distribution");
                distributions.Should().BeEquivalentTo(expectedDistributions, options => options.WithStrictOrderingFor(x => x.RuntimeType == typeof(int[])), distributionExpected);

                var currDepth = sut.Depth;
                currDepth.Should().Be(expectedDepth, "invalid depth after add");
            }

            void AssertDeleteElement(int key, IEnumerable<IEnumerable<int>> distribution, string distributionExpected, int expectedDepth)
            {
                sut.Remove(key);
                sut.Search(key).Should().Be(null, $"after remove elem with {key}");

                var distributions = sut.KeysInNodes.ToList();
                distributions.SelectMany(x => x).Should().HaveCount(sut.Count, "total elements");

                var expectedDistributions = distribution.ToList();
                distributions.Should().HaveCount(expectedDistributions.Count(), "nodes distribution");
                distributions.Should().BeEquivalentTo(expectedDistributions, options => options.WithStrictOrderingFor(x => x.RuntimeType == typeof(int[])), distributionExpected);

                var currDepth = sut.Depth;
                currDepth.Should().Be(expectedDepth, "invalid depth after add");
            }

            AssertAddElement(1, "1", new List<int[]> { new int[] { 1 } }, "root 1", 1);
            AssertAddElement(5, "5", new List<int[]> { new int[] { 1, 5 } }, "root 1-5", 1);
            AssertAddElement(2, "2", new List<int[]> { new int[] { 1, 2, 5 } }, "root 1-2-5", 1);
            AssertAddElement(4, "4", new List<int[]> { new int[] { 1, 2, 4, 5 } }, "root 1-2-4-5",1);
            AssertAddElement(3, "3", new List<int[]> { new int[] { 3 }, new int[] { 1, 2 }, new int[] { 4, 5 } }, "root 3, left(3) 1-2, right(3) 4-5", 2);

            AssertAddElement(6, "6", new List<int[]> { new int[] { 3 }, new int[] { 1, 2 }, new int[] { 4, 5, 6 } }, "root 3, left(3) 1-2, right(3) 4-5-6", 2);
            AssertAddElement(7, "7", new List<int[]> { new int[] { 3 }, new int[] { 1, 2 }, new int[] { 4, 5, 6, 7 } }, "root 3, left(3) 1-2, right(3) 4-5-6-7", 2);
            AssertAddElement(8, "8", new List<int[]> { new int[] { 3, 6 }, new int[] { 1, 2 }, new int[] { 4, 5 }, new int[] { 7, 8 } }, "root 3-6, left(3) 1-2, right(3) 4-5, right(6) 7-8", 2);

            AssertAddElement(0, "0", new List<int[]> { new int[] { 3, 6 }, new int[] { 0, 1, 2 }, new int[] { 4, 5 }, new int[] { 7, 8 } },
                "root 3-6, left(3) 0-1-2, right(3) 4-5, right(6) 7-8", 2);
            AssertAddElement(-3, "-3", new List<int[]> { new int[] { 3, 6 }, new int[] { -3, 0, 1, 2 }, new int[] { 4, 5 }, new int[] { 7, 8 } }, 
                "root 3-6, left(3) -3-0-1-2, right(3) 4-5, right(6) 7-8", 2);

            AssertDeleteElement(1, new List<int[]> { new int[] { 3, 6 }, new int[] { -3, 0, 2 }, new int[] { 4, 5 }, new int[] { 7, 8 } }, 
                "root 3-6, left(3) -3-0-2, right(3) 4-5, right(6) 7-8", 2);
            AssertAddElement(9, "9", new List<int[]> { new int[] { 3, 6 }, new int[] { -3, 0, 2 }, new int[] { 4, 5 }, new int[] { 7, 8, 9 } }, 
                "root 3-6, left(3) -3-0-2, right(3) 4-5, right(6) 7-8-9", 2); 
            AssertDeleteElement(6, new List<int[]> { new int[] { 3, 7 }, new int[] { -3, 0, 2 }, new int[] { 4, 5 }, new int[] { 8, 9 } },
                "root 3-7, left(3) -3-0-2, right(3) 4-5, right(7) 8-9", 2);

            AssertDeleteElement(5, new List<int[]> { new int[] { 2, 7 }, new int[] { -3, 0 }, new int[] {3, 4 }, new int[] { 8, 9 } },
                "root 2-7, left(2) -3-0, right(2) 3-4, right(7) 8-9", 2);
            AssertAddElement(10, "10", new List<int[]> { new int[] { 2, 7 }, new int[] { -3, 0 }, new int[] { 3, 4 }, new int[] { 8, 9, 10 } },
                "root 2-7, left(2) -3-0, right(2) 3-4, right(7) 8-9-10", 2);
            AssertDeleteElement(4, new List<int[]> { new int[] { 2, 8 }, new int[] { -3, 0 }, new int[] { 3, 7 }, new int[] { 9, 10 } },
                "root 2-8, left(2) -3-0, right(2) 3-7, right(8) 9-10", 2);

            AssertDeleteElement(9, new List<int[]> { new int[] { 2 }, new int[] { -3, 0 }, new int[] { 3, 7, 8, 10 } },
                "root 2, left(2) -3-0, right(2) 3-7-8-10", 2);
            //AssertDeleteElement(3, new List<int[]> { new int[] { 4, 6 }, new int[] { 1, 2 }, new int[] { 5 }, new int[] { 7, 8 } }, "root 4-6, left(4) 1-2, right(4) 5, right(6) 7-8");
            //AssertDeleteElement(1, new List<int[]> { new int[] { 4, 6 }, new int[] { 2 }, new int[] { 5 }, new int[] { 7, 8 } }, "root 4-6, left(4) 2, right(4) 5, right(6) 7-8");

            //AssertAddElement(1, "1", new List<int[]> { new int[] { 3 }, new int[] { 1, 2 }, new int[] { 4, 5 } }, "root 3, left(3) 1-2, right(3) 4-5"); //root 3, left(3) 1-2, right(3) 4-5
        }
    }
}
