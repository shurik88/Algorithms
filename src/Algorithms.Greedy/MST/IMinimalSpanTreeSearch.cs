using Algorithms.Graphs.Model;
using System.Collections.Generic;

namespace Algorithms.Greedy.MST
{
    /// <summary>
    /// Алгоритм поиска минимального оставного дерева
    /// </summary>
    public interface IMinimalSpanTreeSearch
    {
        /// <summary>
        ///  Получение списка ребер минимального оставного дерева
        /// </summary>
        /// <param name="graphDefinition">Описание графа</param>
        /// <returns>Список ребер</returns>
        IEnumerable<Edge> GetMST(IGraphDefinition graphDefinition);
    }
}
