using System;

namespace Algorithms.Sorting
{
    /// <summary>
    /// Алгоритм быстрой сортировки
    /// </summary>
    public class QuickSortAlgorithm : ISortAlgorithm
    {
        private readonly IElementPartitionStrategy _strategy;

        /// <summary>
        /// Создание экземпляра класса <see cref="QuickSortAlgorithm"/>
        /// </summary>
        /// <param name="strategy">Стратегия выбора опорного элемента</param>
        public QuickSortAlgorithm(IElementPartitionStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }
        /// <inheritdoc/>
        public void Sort<TElement>(TElement[] array) where TElement : IComparable
        {
            SortInternal(array, 0, array.Length - 1);
        }

        private void SortInternal<TElement>(TElement[] array, int left, int right) where TElement : IComparable
        {
            if (left >= right)
                return;

            var partiionElementIndex = _strategy.GetIndex(right - left + 1) + left;
            var newPartitionElementIndex = array.Partition(left, right, partiionElementIndex);
            SortInternal(array, left, newPartitionElementIndex - 1);
            SortInternal(array, newPartitionElementIndex + 1,right);
        }
    }
}
