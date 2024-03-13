using Algorithms.Structures.Hash;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.Structures.Test
{
    [TestClass]
    public class BloomFilterTest
    {
        [TestMethod]
        public void BloomFilter() 
        {
            var filter = new BloomFilter<int>(100, 0.01);
            filter.Contains(100).Should().BeFalse("100 not added before");
            filter.Add(100);
            filter.Contains(100).Should().BeTrue("100 added before");
        }
    }
}
