using Algorithms.Structures.UnionFind;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Algorithms.Structures.Test
{
    [TestClass]
    public class UnionFindUnitTest
    {
        [TestMethod]
        public void UnionFindTest()
        {
            var items = Enumerable.Range(1, 10).Select(x => new UnionFindElementImpl { Id = x }).ToList();
            var unionFind = new UnionFind<int>(items);


            void AssertFind(UnionFindElementImpl item, UnionFindElementImpl parent)
            {
                var findParent = unionFind.Find(item);
                Assert.AreSame(parent, findParent, $"Root element for element with id: {item.Id} is {findParent.Id} but not {parent.Id}");
            }

            void AssertUnion(UnionFindElementImpl first, UnionFindElementImpl second, UnionFindElementImpl newParent)
            {
                unionFind.Union(first, second);
                var findParent1 = unionFind.Find(first);
                var findParent2 = unionFind.Find(second);
                Assert.AreSame(findParent1,  newParent, $"Root element for element with id: {first.Id} is {findParent1.Id} but not {newParent.Id}");
                Assert.AreSame(findParent2, newParent, $"Root element for element with id: {second.Id} is {findParent1.Id} but not {newParent.Id}");
            }

            // test init unionfind
            items.ForEach(x =>
            {
                AssertFind(x, x);
            });

            AssertUnion(items[0], items[1], items[0]);
            AssertUnion(items[3], items[4], items[3]);
            AssertUnion(items[3], items[5], items[3]);
            AssertUnion(items[6], items[7], items[6]);
            AssertUnion(items[8], items[9], items[8]);
            AssertUnion(items[6], items[8], items[6]);
            AssertUnion(items[6], items[2], items[6]);
            AssertUnion(items[0], items[6], items[6]);
            AssertUnion(items[3], items[6], items[6]);
        }

        private class UnionFindElementImpl: UnionFindItem<int>
        {

        }
    }
}
