using Algorithms.Graphs.Model;
using Algorithms.Graphs.Model.AdjacencyLists;
using Algorithms.Greedy.MST;
using Algorithms.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Algorithms.Greedy.Test
{
    [TestClass]
    public class MinimalSpanTreeUnitTest
    {
        [TestMethod]
        public void TestPrimAlgorithm()
        {
            SimpleCase(new PrimAlgorithm());
            SimpleCase2(new PrimAlgorithm());
        }

        private readonly ISortAlgorithm _sortAlgorithm = new QuickSortAlgorithm(new RandElementPartitionStrategy(new System.Random()));

        [TestMethod]
        public void TestKrusculAlgorithm()
        {
            SimpleCase(new KruskalAlgorithm(_sortAlgorithm));
            SimpleCase2(new KruskalAlgorithm(_sortAlgorithm));
        }

        private void SimpleCase(IMinimalSpanTreeSearch algorithm)
        {
            var graph = new Graph(EdgeDirectionType.Undirected);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);

            graph.AddEdge(1, 2, 1, 1, EdgeDirectionType.Undirected);
            graph.AddEdge(2, 3, 2, 2, EdgeDirectionType.Undirected);
            graph.AddEdge(3, 4, 3, 5, EdgeDirectionType.Undirected);
            graph.AddEdge(4, 1, 4, 4, EdgeDirectionType.Undirected);
            graph.AddEdge(1, 3, 5, 3, EdgeDirectionType.Undirected);

            AssertEdges(algorithm, graph, 1, 2, 4);
        }

        private void SimpleCase2(IMinimalSpanTreeSearch algorithm)
        {
            var graph = new Graph(EdgeDirectionType.Undirected);
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);
            graph.AddVertex(5);
            graph.AddVertex(6);
            graph.AddVertex(7);

            graph.AddEdge(1, 2, 1, 4, EdgeDirectionType.Undirected);
            graph.AddEdge(1, 5, 2, 7, EdgeDirectionType.Undirected);
            graph.AddEdge(2, 3, 3, 2, EdgeDirectionType.Undirected);
            graph.AddEdge(2, 4, 4, 6, EdgeDirectionType.Undirected);
            graph.AddEdge(2, 7, 5, 8, EdgeDirectionType.Undirected);
            graph.AddEdge(3, 7, 6, 1, EdgeDirectionType.Undirected);
            graph.AddEdge(3, 4, 7, 5, EdgeDirectionType.Undirected);
            graph.AddEdge(7, 5, 8, 9, EdgeDirectionType.Undirected);
            graph.AddEdge(7, 6, 9, 4, EdgeDirectionType.Undirected);
            graph.AddEdge(5, 6, 10, 3, EdgeDirectionType.Undirected);

            AssertEdges(algorithm, graph, 1, 3, 7, 6, 9, 10);
        }

        private void AssertEdges(IMinimalSpanTreeSearch algorithm, IGraphDefinition graph, params int[] edgeIds)
        {
            var edges = algorithm.GetMST(graph).ToList();
            Assert.AreEqual(edgeIds.Length, edges.Count);

            foreach (var edgeId in edgeIds)
                Assert.IsTrue(edges.Any(x => x.Id == edgeId), $"Edge: {edgeId} not found in : {string.Join(",", edges.Select(x => x.Id))}");
        }
    }
}
