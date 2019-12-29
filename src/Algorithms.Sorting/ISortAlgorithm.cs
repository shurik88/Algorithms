using System;

namespace Algorithms.Sorting
{
    /// <summary>
    /// Интерфейс алгоритма сортировки
    /// </summary>
    public interface ISortAlgorithm
    {
        /// <summary>
        /// Сортировка элементов по возрастанию
        /// </summary>
        /// <typeparam name="TElement">Тип элемента</typeparam>
        /// <param name="array">Массив</param>
        void Sort<TElement>(TElement[] array)
            where TElement: IComparable;
    }
}
