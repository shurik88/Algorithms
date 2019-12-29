using System;

namespace Algorithms.Sorting
{
    /// <summary>
    /// Алгоритм линейного поиска с использованием случайного выбранного опорного элемента.
    /// </summary>
    public class RSelectAlgorithm
    {
        private readonly Random _rand;

        /// <summary>
        /// Создание экземпляра класса <see cref="RSelectAlgorithm"/>
        /// </summary>
        /// <param name="rand">Генератор случайных чисел</param>
        public RSelectAlgorithm(Random rand)
        {
            _rand = rand;
        }

        /// <summary>
        /// Найти i-статитику массива элементов
        /// </summary>
        /// <typeparam name="TElement">Тип элемента</typeparam>
        /// <param name="array">Массив</param>
        /// <param name="index">Номер статитики i</param>
        /// <returns>i-статистика(элемент массива)</returns>
        public TElement FindIStatistics<TElement>(TElement[] array, int index)
            where TElement : IComparable
        {
            if (index < 0 || index >= array.Length)
                throw new ArgumentException($"Index should be greater or equal than 0 and lesser than {nameof(array)} size", nameof(index));

            if (array.Length == 1)
                return array[0];

            var copy = new TElement[array.Length];
            array.CopyTo(copy, 0);

            return FindIStatisticsInternal(copy, index, 0, array.Length - 1);
        }

        private TElement FindIStatisticsInternal<TElement>(TElement[] array, int index, int left, int right)
            where TElement : IComparable
        {
            if (right == left)
                return array[left];

            var partitionElemIndex = _rand.Next(left, right + 1);
            var newPartitionElementIndex = array.Partition(left, right, partitionElemIndex);
            if (newPartitionElementIndex == index)
                return array[newPartitionElementIndex];
            else if (newPartitionElementIndex > index)
                return FindIStatisticsInternal(array, index, left, newPartitionElementIndex - 1);
            else
                return FindIStatisticsInternal(array, index, newPartitionElementIndex + 1, right);

        }
    }
}
