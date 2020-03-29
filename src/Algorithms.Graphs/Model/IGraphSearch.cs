using System.Collections.Generic;

namespace Algorithms.Graphs.Model
{
    /// <summary>
    /// Примитива поиска в графе
    /// </summary>
    public interface IGraphSearch
    {
        /// <summary>
        /// Поиск в ширину
        /// </summary>
        /// <param name="start">Стартовая вершина</param>
        /// <returns>Путь следования от стартовой вершины</returns>
        IEnumerable<Vertex> BreadthFirstSearch(Vertex start);

        /// <summary>
        /// Поиск кратчайших путей (вес ребра равен 1) от <see cref=""/> до всех остальных
        /// </summary>
        /// <param name="start">Стартовая вершина</param>
        /// <returns>Набор пар:ключ - идентификатор вершины, значение - длина пути</returns>
        IEnumerable<KeyValuePair<int, int>> GetClosestPaths(Vertex start);

        /// <summary>
        /// Поиск кратчайших путей (учитывается вес ребра) от <see cref=""/> до всех остальных
        /// Алгоритм Дейкстры
        /// </summary>
        /// <param name="start">Стартовая вершина</param>
        /// <returns>Набор пар:ключ - идентификатор вершины, значение - длина пути</returns>
        IEnumerable<KeyValuePair<int, int>> GetClosestPathsByWeight(Vertex start);

        /// <summary>
        /// Поиск в глубину
        /// </summary>
        /// <param name="start">Стартовая вершина</param>
        /// <returns>Путь следования от стартовой вершины</returns>
        IEnumerable<Vertex> DeepthFirstSearch(Vertex start);
    }
}
