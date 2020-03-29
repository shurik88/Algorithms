using System.Collections.Generic;

namespace Algorithms.Graphs.Model
{
    public interface IGraph : IGraphBuilder, IGraphSearch
    {
        /// <summary>
        /// Ориентация графа
        /// </summary>
        EdgeDirectionType DirectionType { get; }

        /// <summary>
        /// Получение списка соседних вершин, в которые исходят ребра из <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">Вершина</param>
        /// <returns>Вершины</returns>
        IEnumerable<Vertex> GetOutCommingVertices(Vertex vertex);

        /// <summary>
        /// Получение списка соседних вершин, из которых исходлят ребра в <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">Вершина</param>
        /// <returns>Вершины</returns>
        IEnumerable<Vertex> GetInCommingVertices(Vertex vertex);
    }
}
