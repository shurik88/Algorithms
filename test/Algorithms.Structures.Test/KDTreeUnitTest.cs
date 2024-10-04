using Algorithms.Structures.Trees;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.Structures.Test
{
    [TestClass]
    public class KDTreeUnitTest
    {
        [TestMethod]
        public void KDTreeConstrcution()
        {
            var points = new KDTreePoint<string>[]
            {
                new KDTreePoint<string>{ Coordinates = new int[]{ 1, 1}, Value = "1" },
                new KDTreePoint<string>{ Coordinates = new int[]{ 5, 10}, Value = "2" },
                new KDTreePoint<string>{ Coordinates = new int[]{ 7, 2}, Value = "3" },
                new KDTreePoint<string>{ Coordinates = new int[]{ 8, 2}, Value = "4" },
                new KDTreePoint<string>{ Coordinates = new int[]{ 14, 6}, Value = "5" },
                new KDTreePoint<string>{ Coordinates = new int[]{ 6, 5}, Value = "6" },
                new KDTreePoint<string>{ Coordinates = new int[]{ 13, 11}, Value = "7" },
                new KDTreePoint<string>{ Coordinates = new int[]{ 10, 7}, Value = "8" },
                new KDTreePoint<string>{ Coordinates = new int[]{ 3, 8}, Value = "9" },
                new KDTreePoint<string>{ Coordinates = new int[]{ 9, 4}, Value = "10" },
            };
            var sut = KDTree<string>.BuildTree(points);
            sut.Root.Should().NotBeNull();
            sut.Root.Value.Should().Be("4");
            sut.Root.Left.Value.Should().Be("6");
            sut.Root.Right.Value.Should().Be("8");

            void AssertMin(int dimension, string expected)
            {
                var min0 = sut.FindMin(dimension);
                min0.Value.Should().Be(expected);
            }

            void AssertMax(int dimension, string expected)
            {
                var min0 = sut.FindMax(dimension);
                min0.Value.Should().Be(expected);
            }

            void AssertNearestNeighbor(int[] coordinates, string expected)
            {
                var nearest = sut.FindNearestNeighbor(coordinates);
                nearest.Value.Should().Be(expected);
            }

            AssertMin(0, "1");
            AssertMin(1, "1");
            AssertMax(0, "5");
            AssertMax(1, "7");

            void AssertFound(int[] coordinates, string expectedValue)
            {
                var found = sut.FindNode(coordinates);
                if(string.IsNullOrEmpty(expectedValue))
                    found.Should().BeNull();
                else
                {
                    found.Should().NotBeNull();
                    found.Value.Should().Be(expectedValue);
                }               
            }

            AssertFound(new int[] { 7, 2 }, "3");
            AssertFound(new int[] { 3, 8 }, "9");
            AssertFound(new int[] { 14, 7 }, "");

            AssertNearestNeighbor(new int[] { 1, 1 }, "1");
            AssertNearestNeighbor(new int[] { 6, 11 }, "2");
        }
    }
}
