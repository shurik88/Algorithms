using System;

namespace Algorithms.Structures.Heaps
{
    public class HeapElement<TKey, TValue>
        where TKey: IComparable
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public TValue Value { get; set; }
    }
}
