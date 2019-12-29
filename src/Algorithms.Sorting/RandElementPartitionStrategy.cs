using System;

namespace Algorithms.Sorting
{
    /// <summary>
    /// Выбор случайного элемента в качестве опорного
    /// </summary>
    public class RandElementPartitionStrategy : IElementPartitionStrategy
    {
        private readonly Random _rand;
        /// <summary>
        /// Создание экземпляра класса <see cref="RandElementPartitionStrategy"/>
        /// </summary>
        /// <param name="rand">Генератор случайных чисел</param>
        public RandElementPartitionStrategy(Random rand)
        {
            _rand = rand;
        }

        /// <inheritdoc/>
        public int GetIndex(int arrayLength) => _rand.Next(0, arrayLength);
    }
}
