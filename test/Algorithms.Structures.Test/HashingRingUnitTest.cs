using Algorithms.Structures.Hash;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Algorithms.Structures.Test
{
    [TestClass]
    public class HashingRingUnitTest
    {
        [TestMethod]
        public void HashinRingTestMethod()
        {
            var ring = new HashRing<string, string>(new HashFunc(32));

            void AssertRing(int[] nodes, IEnumerable<int[]> resources)
            {
                var orderredList = ring.OrderedList.ToList();
                var ordered = orderredList.Select(x => x.Key).ToList();

                ordered.Should().HaveCount(nodes.Length, $"adding {nodes.Length} nodes")
                    .And.ContainInOrder(nodes);

                var resourcesList = resources.ToList();
                orderredList.Should().HaveCount(resourcesList.Count, "nodes count not equal to resources sets.");
                for(var i = 0; i < orderredList.Count; ++i)
                {
                    var node = orderredList[i];
                    var nodeResources = node.ResorucesKeys.ToList();
                    nodeResources.Should().HaveCount(resourcesList[i].Length).And.ContainInOrder(resourcesList[i]);
                }

            }
            var node1 = new TestNode(4, "node1");
            ring.AddNode(node1);
            var resource1 = new TestResource(3, "res1");
            ring.AddResource(resource1);
            ring.AddResource(new TestResource(26, "res2"));
            AssertRing(new int[] { 4}, new int[1][] { new int[] { 3, 26 } });

            var node2 = new TestNode(27, "node2");
            ring.AddNode(node2);
            AssertRing(new int[] { 4, 27 }, new int[2][] { new int[] { 3 }, new int[] { 26 } });

            ring.AddNode(new TestNode(15, "node3"));
            AssertRing(new int[] { 4, 15, 27 }, new int[3][] { new int[] { 3 }, new int[] { }, new int[] { 26 } });

            ring.RemoveNode(node2);
            AssertRing(new int[] { 4, 15 }, new int[2][] { new int[] { 3, 26 }, new int[] { } });

            ring.RemoveResource(resource1);
            AssertRing(new int[] { 4, 15 }, new int[2][] { new int[] { 26 }, new int[] { } });

            ring.RemoveNode(node1);
            AssertRing(new int[] { 15 }, new int[1][] { new int[] { 26 } });
            //ring.AddNode(new TestNode(20, "node2"));
            //ring.AddNode(new TestNode(2, "node2"));

            

        }

        private class TestResource: HashRingResource<string>
        {
            public TestResource(int key, string name)
            {
                Key = key;
                Value = name;
            }
        }

        private class TestNode : HashRingNode<string, string>
        {
            public TestNode(int key, string value) : base(key, value)
            {
            }
        }

        private class HashFunc : IHashFunc
        {
            public HashFunc(int maxValue)
            {
                Max = maxValue;
            }
            public int Max { get; private set; }

            public int GetHash(int key)
            {
                if(key < 0 || key >= Max)
                    throw new ArgumentException("Please specify key between 0 and 32", nameof(key));

                return key;
            }
        }
    }
}
