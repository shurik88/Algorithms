using Algorithms.Structures.Hash;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.Structures.Test
{
    [TestClass]
    public class CountMinTest
    {
        [TestMethod]
        public void CountMin()
        {
            var countMin = new CountMin<int>(0.0001, 0.01);
            countMin.Update(3, 2);
            countMin.GetCount(3).Should().Be(2, "added 2");
        }

    }
}
