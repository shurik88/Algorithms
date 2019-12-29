using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Algorithms.Structures.Test
{
    [TestClass]
    public class HeapUnitTest
    {
        [TestMethod]
        public void MinHeapTest()
        {
            var rand = new Random();
            var heap = new Heap<int, int>(Heaps.HeapType.Min, 100);
            void Insert(int value)
            {
                var count = heap.Count;
                heap.Insert(new Heaps.HeapElement<int, int> { Key = value, Value = value });
                Assert.AreEqual(count + 1, heap.Count, "Invalid heap count when insert");
                ValidateMinHeap();
            }
            void Extract()
            {
                var min = heap.Elements.OrderBy(x => x.Key).First().Key;
                var elem = heap.Extract();
                Assert.AreEqual(min, elem.Value, "extract min from heap");
                ValidateMinHeap();
            }
            void ValidateMinHeap()
            {
                var elements = heap.Elements.ToList();
                void ValidateMinHeapPair(int parent, int child)
                {
                    if (child >= heap.Count)
                        return;
                    Assert.IsTrue(elements[parent].Key <= elements[child].Key,
                        $"Invalid parent value:{elements[parent].Key} in index:{parent}, child value:{elements[child].Key} in index:{child} in min heap");
                }
                for (var i = 0; i < heap.Count; ++i)
                {
                    ValidateMinHeapPair(i, 2 * i + 1);
                    ValidateMinHeapPair(i, 2 * i + 2);
                }
            }

            Assert.IsTrue(heap.IsEmpty, "heap is not empty");
            Insert(3);
            Insert(-2);
            Insert(0);
            Insert(7);
            Insert(8);
            Insert(10);
            Insert(4);
            Insert(-3);
            Extract();
            Extract();
            Extract();

        }



        [TestMethod]
        public void MaxHeapTest()
        {
            var rand = new Random();
            var heap = new Heap<int, int>(Heaps.HeapType.Max, 100);
            void Insert(int value)
            {
                var count = heap.Count;
                heap.Insert(new Heaps.HeapElement<int, int> { Key = value, Value = value });
                Assert.AreEqual(count + 1, heap.Count, "Invalid heap count when insert");
                ValidateMaxHeap();
            }
            void Extract()
            {
                var max = heap.Elements.OrderByDescending(x => x.Key).First().Key;
                var elem = heap.Extract();
                Assert.AreEqual(max, elem.Value, "extract max from heap");
                ValidateMaxHeap();
            }
            void ValidateMaxHeap()
            {
                var elements = heap.Elements.ToList();
                void ValidateMinHeapPair(int parent, int child)
                {
                    if (child >= heap.Count)
                        return;
                    Assert.IsTrue(elements[parent].Key >= elements[child].Key, 
                        $"Invalid parent value:{elements[parent].Key} in index:{parent}, child value:{elements[child].Key} in index:{child} in max heap");
                }
                for (var i = 0; i < heap.Count; ++i)
                {
                    ValidateMinHeapPair(i, 2 * i + 1);
                    ValidateMinHeapPair(i, 2 * i + 2);
                }
            }

            Assert.IsTrue(heap.IsEmpty, "heap is not empty");
            Insert(3);
            Insert(-2);
            Insert(0);
            Insert(7);
            Insert(8);
            Insert(10);
            Insert(4);
            Insert(-3);
            Extract();
            Extract();
            Extract();
        }
    }
}
