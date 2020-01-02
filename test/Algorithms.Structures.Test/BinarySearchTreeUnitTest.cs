using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Algorithms.Structures.Trees;
using System.Linq;

namespace Algorithms.Structures.Test
{
    [TestClass]
    public class BinarySearchTreeUnitTest
    {
        [TestMethod]
        public void BinarySearchTreeCrudOperationsTest()
        {
            var rand = new Random();
            var tree = new BinarySearchTree<int, int>();
            var list = new List<int>();
            Assert.IsTrue(tree.IsEmpty, "tree is not empty");
            Insert(12);
            Assert.ThrowsException<ArgumentException>(() => tree.Insert(new ComparableElement<int, int> { Key = 12, Value = 12 }));
            Insert(20);
            Assert.IsNull(tree.Find(34), "34 not exists in tree");
            Insert(3);
            Insert(7);
            Insert(4);
            Insert(9);
            Insert(11);
            Assert.IsNotNull(tree.Find(7), "7 exists in tree");
            Insert(10);
            Insert(18);
            Insert(8);
            Insert(17);
            Insert(15);
            Insert(19);
            Assert.AreEqual(11, tree.FindPredecessor(12).Key, "invalid predecessor for 12");
            Assert.AreEqual(15, tree.FindSuccessor(12).Key, "invalid successor for 12");
            Assert.AreEqual(list.Count, tree.Count, "invalid tree count");
            Assert.AreEqual(list.Count, tree.OrderedList.Count(), "invalid orderedlist count");
            Assert.AreEqual(list.Max(), tree.Max.Key, "invalid tree max");
            Assert.AreEqual(list.Min(), tree.Min.Key, "invalid tree min");

            Assert.AreEqual(6, tree.Height, "tree height");
            Assert.AreEqual(5, tree.LeftHeight, "left subtree height");
            Assert.AreEqual(4, tree.RightHeight, "right subtree height");

            //delete
            Assert.ThrowsException<ArgumentException>(() => tree.Delete(34));
            Delete(4);
            Delete(20);
            Delete(9);
            //delete

            void Insert(int value)
            {
                var count = tree.Count;
                tree.Insert(new ComparableElement<int, int> { Key = value, Value = value });
                list.Add(value);
                Assert.AreEqual(count + 1, tree.Count, "Invalid tree count when insert");
                ValidateBinarySearchTree();
            }

            void Delete(int value)
            {
                var count = tree.Count;
                tree.Delete(value);
                list.Remove(value);
                Assert.AreEqual(count - 1, tree.Count, "Invalid tree count when insert");
                ValidateBinarySearchTree();
            }

            void ValidateBinarySearchTree()
            {
                var orderedPlainList = list.OrderBy(x => x).ToList();
                var orderedTreeList = tree.OrderedList.ToList();
                Assert.AreEqual(list.Count, orderedTreeList.Count, "Invalid tree count");
                Assert.AreEqual(orderedPlainList.Last(), tree.Max.Key, "Invalid tree max");
                Assert.AreEqual(orderedPlainList.First(), tree.Min.Key, "Invalid tree min");
                
                for (var i = 0; i < orderedTreeList.Count; ++i)
                {
                    Assert.AreEqual(orderedPlainList[i], orderedTreeList[i].Key, $"Invalid tree nodes order in index:{i}, value:{orderedTreeList[i].Key}");
                }
            }
        }

        [TestMethod]
        public void BinarySearchTreeTurnOperationsTest()
        {
            var rand = new Random();
            var tree = new BinarySearchTree<int, int>();
            var list = new List<int>();

            void Insert(int value)
            {
                var count = tree.Count;
                tree.Insert(new ComparableElement<int, int> { Key = value, Value = value });
                list.Add(value);
            }

            void ValidateBinarySearchTree()
            {
                var orderedPlainList = list.OrderBy(x => x).ToList();
                var orderedTreeList = tree.OrderedList.ToList();
                Assert.AreEqual(list.Count, orderedTreeList.Count, "Invalid tree count");
                Assert.AreEqual(orderedPlainList.Last(), tree.Max.Key, "Invalid tree max");
                Assert.AreEqual(orderedPlainList.First(), tree.Min.Key, "Invalid tree min");

                for (var i = 0; i < orderedTreeList.Count; ++i)
                {
                    Assert.AreEqual(orderedPlainList[i], orderedTreeList[i].Key, $"Invalid tree nodes order in index:{i}, value:{orderedTreeList[i].Key}");
                }
            }
            Insert(12);
            Insert(20);
            Insert(3);
            Insert(7);
            Insert(4);
            Insert(9);
            Insert(11);
            Insert(10);
            Insert(18);
            Insert(8);
            Insert(14);
            Insert(13);
            Insert(19);

            Assert.ThrowsException<ArgumentException>(() => tree.TurnLeft(11), "impossible to do left turn");
            tree.TurnLeft(3);
            ValidateBinarySearchTree();

            Assert.ThrowsException<ArgumentException>(() => tree.TurnRight(3), "impossible to do right turn");
            tree.TurnRight(20);
            ValidateBinarySearchTree();

            Insert(26);
            Insert(30);
            Insert(23);
            Insert(24);
            Insert(21);
            Assert.ThrowsException<ArgumentException>(() => tree.TurnBigLeft(3), "impossible to do big left turn");
            tree.TurnBigLeft(20);
            ValidateBinarySearchTree();

            Insert(16);
            Insert(17);
            Insert(15);

            Assert.ThrowsException<ArgumentException>(() => tree.TurnBigRight(3), "impossible to do big right turn");
            tree.TurnBigRight(18);
            ValidateBinarySearchTree();
        }
    }
}
