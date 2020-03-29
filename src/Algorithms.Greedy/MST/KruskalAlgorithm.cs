using Algorithms.Graphs.Model;
using Algorithms.Sorting;
using Algorithms.Structures;
using Algorithms.Structures.UnionFind;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Greedy.MST
{
    /// <summary>
    /// Алгоритм Краскала для поиска минимального оставеного дерева
    /// </summary>
    public class KruskalAlgorithm : IMinimalSpanTreeSearch
    {
        private readonly ISortAlgorithm _sortAlgorithm;

        /// <summary>
        /// Создание экземпляра класса <see cref="KruskalAlgorithm"/>
        /// </summary>
        /// <param name="sortAlgorithm">Алгоритм сортировки</param>
        public KruskalAlgorithm(ISortAlgorithm sortAlgorithm)
        {
            _sortAlgorithm = sortAlgorithm ?? throw new ArgumentNullException(nameof(sortAlgorithm));
        }

        /// <inheritdoc/>
        public IEnumerable<Edge> GetMST(IGraphDefinition graphDefinition)
        {
            var edges = graphDefinition.Edges.ToArray();
            var vertexes = graphDefinition.Vertices.Select(x => new VertexUnionFindItem { Id = x.Id, Vertex = x }).ToList();
            var vertexesDict = vertexes.ToDictionary(x => x.Id, x => x);
            _sortAlgorithm.Sort(edges);
            var unionFind = new UnionFind<int>(vertexes);
            foreach(var edge in edges)
            {
                var (from, to) = graphDefinition.GetVertexPair(edge);
                var firstParent = unionFind.Find(vertexesDict[from.Id]);
                var secondParent = unionFind.Find(vertexesDict[to.Id]);
                if(firstParent != secondParent)
                {
                    unionFind.Union(vertexesDict[from.Id], vertexesDict[to.Id]);
                    yield return edge;
                }
            }
        }

        private class VertexUnionFindItem: UnionFindItem<int>
        {
            /// <summary>
            /// Вершина
            /// </summary>
            public Vertex Vertex { get; set; }
        }
    }
}
