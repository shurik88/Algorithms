using Algorithms.Graphs.Extensions;
using Algorithms.Graphs.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Algorithms.Graphs.Test
{
    [TestClass]
    public class GraphConnectiveComponentsUnitTest
    {
        [TestMethod]
        public void AdjacencyGraphUndirectedConnectiveComponentsTest()
        {
            var graph = new Model.AdjacencyLists.Graph(EdgeDirectionType.Undirected);
            UndirectedConnectiveComponentsTest(graph);
        }

        [TestMethod]
        public void AdjacencyGraphTopologySortTest()
        {
            var graph = new Model.AdjacencyLists.Graph(EdgeDirectionType.Directed);
            TopologySortTest(graph);

            var graph2 = new Model.AdjacencyLists.Graph(EdgeDirectionType.Directed);
            BuildDirectedCyclicGraph(graph2);
            var topology = graph2.GetTopologySort();
        }

        [TestMethod]
        public void AdjacencyGrapDirectedHighlyConnectiveComponentsTest()
        {
            var graph = new Model.AdjacencyLists.Graph(EdgeDirectionType.Directed);
            DirectedHighlyConnectiveComponentsTest(graph);
        }

        private void DirectedHighlyConnectiveComponentsTest(IGraph graph)
        {
            BuildDirected4CyclicGraph(graph);

            var components = graph.GetHiglyConnectiveComponents().ToList();
            Assert.AreEqual(4, components.Count, "components count");

            void AssertElemsExistsInComponent(params int[] ids)
            {
                var firstElem = ids[0];
                var componentWithVertex1 = components.First(x => x.Vertices.Any(y => y.Id == firstElem));
                Assert.AreEqual(ids.Length, componentWithVertex1.Vertices.Count(), $"vertices count from component with elem {firstElem}");
                foreach(var elem in ids)
                    Assert.IsTrue(componentWithVertex1.Vertices.Any(x => x.Id == elem), $"vertice {elem} from component with elem {firstElem}");
            }

            AssertElemsExistsInComponent(1, 3, 5);
            AssertElemsExistsInComponent(11);
            AssertElemsExistsInComponent(2, 9, 4, 7);
            AssertElemsExistsInComponent(6, 10, 8);
        }

        private static void Build3ComponentsUndirectedGraph(IGraphBuilder graph)
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);
            graph.AddVertex(5);
            graph.AddVertex(6);
            graph.AddVertex(7);
            graph.AddVertex(8);
            graph.AddVertex(9);
            graph.AddVertex(10);
            graph.AddVertex(11);
            graph.AddVertex(12);

            var edgeId = new IntIdGenerator();

            graph.AddEdge(1, 2, edgeId.Next);
            graph.AddEdge(2, 4, edgeId.Next);
            graph.AddEdge(4, 5, edgeId.Next);
            graph.AddEdge(5, 1, edgeId.Next);
            graph.AddEdge(3, 1, edgeId.Next);

            graph.AddEdge(6, 7, edgeId.Next);
            graph.AddEdge(7, 8, edgeId.Next);
            graph.AddEdge(8, 6, edgeId.Next);

            graph.AddEdge(9, 10, edgeId.Next);
            graph.AddEdge(10, 11, edgeId.Next);
            graph.AddEdge(11, 12, edgeId.Next);
            graph.AddEdge(12, 9, edgeId.Next);
        }

        private static void UndirectedConnectiveComponentsTest(IGraph graph)
        {
            Build3ComponentsUndirectedGraph(graph);
            var components = graph.GetConnectiveComponents().ToList();
            Assert.AreEqual(3, components.Count, "components count");
            Assert.AreEqual(graph.Vertices.Count(), components.SelectMany(x => x.Vertices).Select(x => x.Id).Distinct().Count(), "all vertices found in components");

            var componentWithVertex1 = components.First(x => x.Vertices.Any(y => y.Id == 1));
            Assert.AreEqual(5, componentWithVertex1.Vertices.Count(), "vertices from component 1");

            var componentWithVertex6 = components.First(x => x.Vertices.Any(y => y.Id == 6));
            Assert.AreEqual(3, componentWithVertex6.Vertices.Count(), "vertices from component 2");

            var componentWithVertex9 = components.First(x => x.Vertices.Any(y => y.Id == 9));
            Assert.AreEqual(4, componentWithVertex9.Vertices.Count(), "vertices from component 3");
        }

        private void TopologySortTest(IGraph graph)
        {
            BuildDirectedAcyclicGraph(graph);

            var topology = graph.GetTopologySort();
            Assert.AreEqual(graph.Vertices.Count(), topology.Count, "topology equal vertices count");

            void AssertTopologyOrder(int greater, int lesser)
            {
                Assert.IsTrue(topology[greater] > topology[lesser], $"{greater} elem topology value greater than {lesser}");
            }
            AssertTopologyOrder(11, 10);
            AssertTopologyOrder(10, 9);
            AssertTopologyOrder(9, 1);
            AssertTopologyOrder(4, 1);
            AssertTopologyOrder(7, 1);
            AssertTopologyOrder(7, 8);
            AssertTopologyOrder(5, 8);
            AssertTopologyOrder(6, 8);
            AssertTopologyOrder(2, 5);
            AssertTopologyOrder(3, 2);
            AssertTopologyOrder(4, 2);
            AssertTopologyOrder(4, 3);

            graph.Clear();
            BuildDirectedCyclicGraph(graph);
            graph.GetTopologySort();
        }

        private static void BuildDirectedCyclicGraph(IGraphBuilder graph)
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            var edgeId = new IntIdGenerator();
            graph.AddEdge(1, 2, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(2, 3, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(3, 1, edgeId.Next, EdgeDirectionType.Directed);
        }

        private static void BuildDirected4CyclicGraph(IGraphBuilder graph)
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);
            graph.AddVertex(5);
            graph.AddVertex(6);
            graph.AddVertex(7);
            graph.AddVertex(8);
            graph.AddVertex(9);
            graph.AddVertex(10);
            graph.AddVertex(11);

            var edgeId = new IntIdGenerator();
            graph.AddEdge(1, 3, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(3, 5, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(5, 1, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(3, 11, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(11, 6, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(11, 8, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(6, 10, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(10, 8, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(8, 6, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(5, 9, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(5, 7, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(7, 9, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(9, 2, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(2, 4, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(4, 7, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(9, 8, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(2, 10, edgeId.Next, EdgeDirectionType.Directed);
        }



        private static void BuildDirectedAcyclicGraph(IGraphBuilder graph)
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            graph.AddVertex(4);
            graph.AddVertex(5);
            graph.AddVertex(6);
            graph.AddVertex(7);
            graph.AddVertex(8);
            graph.AddVertex(9);
            graph.AddVertex(10);
            graph.AddVertex(11);

            var edgeId = new IntIdGenerator();
            graph.AddEdge(1, 4, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(1, 7, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(1, 11, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(1, 9, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(2, 3, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(2, 4, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(3, 4, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(5, 2, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(8, 5, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(8, 6, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(8, 7, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(9, 10, edgeId.Next, EdgeDirectionType.Directed);
            graph.AddEdge(10, 11, edgeId.Next, EdgeDirectionType.Directed);
        }
    }
}
