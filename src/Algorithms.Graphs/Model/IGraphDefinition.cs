using System.Collections.Generic;

namespace Algorithms.Graphs.Model
{
    /// <summary>
    /// Описание графа
    /// </summary>
    public interface IGraphDefinition
    {
        /// <summary>
        /// Получение списка соседних вершин, в которые исходят ребра из <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">Вершина</param>
        /// <returns>Вершины и ребра</returns>
        IEnumerable<(Edge Edge, Vertex Vertex)> GetOutComming(Vertex vertex);

        /// <summary>
        /// Вершины
        /// </summary>
        IEnumerable<Vertex> Vertices { get; }

        /// <summary>
        /// Ребра
        /// </summary>
        IEnumerable<Edge> Edges { get; }

        /// <summary>
        /// Существует ли ребро, исходящие из вершины <paramref name="from"/> в <paramref name="to"/>
        /// </summary>
        /// <param name="from">Вершина из</param>
        /// <param name="to">Вершина в</param>
        /// <returns>Да/нет</returns>
        bool EdgeExists(Vertex from, Vertex to);

        /// <summary>
        /// Получение ребер, исходящих из вершины <paramref name="from"/> в <paramref name="to"/>
        /// </summary>
        /// <param name="from">Вершина из</param>
        /// <param name="to">Вершина в</param>
        /// <returns>Ребра</returns>
        IEnumerable<Edge> GetEdges(Vertex from, Vertex to);

        /// <summary>
        /// Получение пары вершин, соединенных ребром
        /// </summary>
        /// <param name="edge">Ребро</param>
        /// <returns>Пара вершин</returns>
        (Vertex from, Vertex to) GetVertexPair(Edge edge);
    }
}
