using System.Collections.Generic;

namespace Algorithms.DynamicProgramming.Knapsack
{
    /// <summary>
    /// Задача о рюкзаке
    /// </summary>
    public interface IKnapsackAlgorithm
    {
        /// <summary>
        /// Получение списка вещей с маскимальной ценностью, удовлетворяющих ограничению <paramref name="maxSize"/>
        /// </summary>
        /// <param name="things">Вещей</param>
        /// <param name="maxSize">Суммарный максимальный вес вещей</param>
        /// <returns>Список вещей(индексы)</returns>
        IEnumerable<int> Get(Thing[] things, int maxSize);
    }
}
