using Algorithms.Graphs.Model;

namespace Algorithms.Greedy.Test
{
    public static class IGraphBuilderExtensions
    {
        public static void AddEdge(this IGraphBuilder graph, int from, int to, int edge, EdgeDirectionType directionType = EdgeDirectionType.Undirected)
        {
            graph.AddEdge(new Vertex { Id = from }, new Vertex { Id = to }, new Edge { Id = edge, DirectionType = directionType });
        }

        public static void AddEdge(this IGraphBuilder graph, int from, int to, int edge, int weight, EdgeDirectionType directionType = EdgeDirectionType.Directed)
        {
            graph.AddEdge(new Vertex { Id = from }, new Vertex { Id = to }, new Edge { Id = edge, Weight = weight, DirectionType = directionType });
        }

        public static void RemoveVertex(this IGraphBuilder graph, int vertex)
        {
            graph.RemoveVertex(new Vertex { Id = vertex });
        }

        public static void AddVertex(this IGraphBuilder graph, int vertex)
        {
            graph.AddVertex(new Vertex { Id = vertex });
        }
    }
}
