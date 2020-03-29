using Algorithms.Graphs.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Algorithms.Graphs.Extensions
{
    /// <summary>
    /// Методы расширения для работы с <seealso cref="IGraph"/>
    /// </summary>
    public static class IGraphExtensions
    {
        /// <summary>
        /// Получение компонент связности для неориентированного графа
        /// </summary>
        /// <param name="graph">Граф</param>
        /// <returns>список компонент</returns>
        public static IEnumerable<ConnectiveComponent> GetConnectiveComponents(this IGraph graph)
        {
            if (graph.DirectionType == EdgeDirectionType.Directed)
                throw new InvalidOperationException("Current graph is directed");

            var visited = new HashSet<int>();
            
            foreach(var vertex in graph.Vertices)
            {
                if (visited.Contains(vertex.Id))
                    continue;
                var vertices = graph.BreadthFirstSearch(vertex).ToList();
                var cluster = new Collection<Vertex>();
                foreach(var found in vertices)
                {
                    if (visited.Contains(found.Id))
                        continue;

                    cluster.Add(found);
                    visited.Add(found.Id);
                }
                if (cluster.Any())
                    yield return new ConnectiveComponent { Vertices = cluster };
            }
        }

        /// <summary>
        /// Получение топологической сортировки для ациклического ориентированного графа
        /// </summary>
        /// <param name="graph">Граф</param>
        /// <returns>Список пар: ключ - идентификатор вершины, значение - значение топологического упорядочивания</returns>
        public static IDictionary<int, int> GetTopologySort(this IGraph graph)
        {
            return graph.GetTopologySortInternal();
        }

        private static IDictionary<int, int> GetTopologySortInternal(this IGraph graph, bool reverse = false)
        {
            var currValue = graph.Vertices.Count();
            var calculatedVertices = new Dictionary<int, int>();

            var visitedVertices = new HashSet<int>();
            void DeepthSearchTopology(Vertex vertex)
            {
                visitedVertices.Add(vertex.Id);
                foreach (var forwardVertex in reverse ? graph.GetInCommingVertices(vertex) : graph.GetOutCommingVertices(vertex))
                {
                    if (visitedVertices.Contains(forwardVertex.Id) || calculatedVertices.ContainsKey(forwardVertex.Id))
                        continue;
                    //visitedVertices.Add(forwardVertex.Id);
                    DeepthSearchTopology(forwardVertex);
                }
                calculatedVertices.Add(vertex.Id, currValue--);
            }

            foreach (var vertex in graph.Vertices)
            {
                if (calculatedVertices.ContainsKey(vertex.Id))
                    continue;

                DeepthSearchTopology(vertex);
            }

            return calculatedVertices;
        }

        /// <summary>
        /// Получение сильно-связных компонент связности для ориентированного графа
        /// Алгоритм Кюсарайо
        /// </summary>
        /// <param name="graph">Граф</param>
        /// <returns>список компонент</returns>
        public static IEnumerable<ConnectiveComponent> GetHiglyConnectiveComponents(this IGraph graph)
        {
            var verticesDict = graph.Vertices.ToDictionary(x => x.Id, x => x);
            var reverseOrder = graph.GetTopologySortInternal(true);
            var vertices = reverseOrder
                .Select(x => new { Vertex = verticesDict[x.Key], Order = x.Value })
                .OrderByDescending(x => x.Order)
                .Select(x => x.Vertex)
                .ToList();

            IEnumerable<Vertex> DeepthFirstSearchReverse(Vertex start)
            {
                var visitedVertices = new HashSet<int>();
                var stack = new Stack<Vertex>();
                stack.Push(start);
                while (stack.Any())
                {
                    var current = stack.Pop();
                    if (visitedVertices.Contains(current.Id))
                        continue;
                    visitedVertices.Add(current.Id);
                    yield return current;
                    foreach (var inVertex in graph.GetInCommingVertices(current))
                        stack.Push(inVertex);
                }
            }

            var visited = new HashSet<int>();
            foreach (var vertex in vertices)
            {
                if (visited.Contains(vertex.Id))
                    continue;
                var deepthList = DeepthFirstSearchReverse(vertex).Where(x => !visited.Contains(x.Id)).ToList();
                if (!deepthList.Any())
                    continue;

                foreach (var res in deepthList)
                    visited.Add(res.Id);
                yield return new ConnectiveComponent { Vertices = deepthList };

            }
        }
    }
}
