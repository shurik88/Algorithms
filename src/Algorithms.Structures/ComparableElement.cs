using System;

namespace Algorithms.Structures
{
    /// <summary>
    /// Элемента хранения в структурах, 
    /// ключ которого поддерживает сравнение
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">тип значения</typeparam>
    public class ComparableElement<TKey, TValue>
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
