using Algorithms.Graphs.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Algorithms.Graphs.Test
{
    [TestClass]
    public class GraphBuilderUnitTest
    {
        [TestMethod]
        public void AdjacencyListsBuilder()
        {
            TestGraphBuilder(new Algorithms.Graphs.Model.AdjacencyLists.Graph(EdgeDirectionType.Directed));       
        }

        private void TestGraphBuilder(IGraphBuilder graph)
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);
            Assert.AreEqual(3, graph.Vertices.Count(), "Adding vertices(3)");

            graph.AddEdge(1, 3, 1);
            graph.AddEdge(2, 1, 2, EdgeDirectionType.Directed);

            Assert.AreEqual(2, graph.Edges.Count(), "Adding edges(2)");

            graph.RemoveVertex(1);
            Assert.AreEqual(2, graph.Vertices.Count(), "Remove vertex(2)");
            Assert.AreEqual(0, graph.Edges.Count(), "After remove vertex");

            Assert.ThrowsException<ArgumentException>(() => graph.RemoveVertex(1));
        }
    }


}
