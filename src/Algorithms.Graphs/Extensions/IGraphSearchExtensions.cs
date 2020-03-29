using Algorithms.Graphs.Model;

namespace Algorithms.Graphs.Extensions
{
    /// <summary>
    /// Методы расширения для работы с <seealso cref="IGraphSearch"/>
    /// </summary>
    public static class IGraphSearchExtensions
    {
        /// <summary>
        /// Существует ли путь из вершины <paramref name="from"/> в <paramref name="to"/>
        /// </summary>
        /// <param name="graphSearch">Граф поиска</param>
        /// <param name="from">Вершина из</param>
        /// <param name="to">Вершина в</param>
        /// <returns>Путь есть или нет</returns>
        public static bool PathExists(this IGraphSearch graphSearch, Vertex from, Vertex to)
        {
            foreach (var item in graphSearch.BreadthFirstSearch(from))
                if (item.Id == to.Id)
                    return true;

            return false;
        }


    }
}
