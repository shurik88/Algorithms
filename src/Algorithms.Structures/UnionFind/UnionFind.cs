using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Structures.UnionFind
{
    /// <summary>
    ///  Система непересекающихся множеств
    /// </summary>
    /// <typeparam name="TKey">тип идентификатора элемента</typeparam>
    public class UnionFind<TKey>
        where TKey: struct
    {
        private readonly UnionFindItemInternal[] _data;
        private readonly IDictionary<TKey, int> _indexes = new Dictionary<TKey, int>();

        /// <summary>
        /// Создание экземпляра класса <see cref="UnionFind{TItem}"/>
        /// </summary>
        /// <param name="items">Исходные элементы множества</param>
        public UnionFind(IEnumerable<UnionFindItem<TKey>> items)
        {
            var itemsList = items.ToList();
            _data = new UnionFindItemInternal[itemsList.Count];
            for(var i = 0; i < itemsList.Count; ++i)
            {
                _data[i] = new UnionFindItemInternal { Data = itemsList[i], Count = 1, ParentIndex = i };
                _indexes.Add(itemsList[i].Id, i);
            }
        }

        /// <summary>
        /// Получение родительского элемента множества, в котором находится <paramref name="item"/>
        /// </summary>
        /// <param name="item">Элемент поиска</param>
        /// <returns>Родитльеский элемент множества</returns>
        public UnionFindItem<TKey> Find(UnionFindItem<TKey> item)
        {
            return FindInternal(item).Data;
        }

        private UnionFindItemInternal FindInternal(UnionFindItem<TKey> item)
        {
            if (!_indexes.ContainsKey(item.Id))
                throw new ArgumentException($"Item with code: {item.Id} not found", nameof(item));

            var currIndex = _indexes[item.Id];
            while (_data[currIndex].ParentIndex != currIndex)
                currIndex = _data[currIndex].ParentIndex;

            return _data[currIndex];
        }

        /// <summary>
        /// Объединение элементов с их подмножествами в одно множество
        /// </summary>
        /// <param name="first">Первый элемент</param>
        /// <param name="second">Второй элемент</param>
        public void Union(UnionFindItem<TKey> first, UnionFindItem<TKey> second)
        {
            var firstRootElement = FindInternal(first);
            var secondRootElement = FindInternal(second);
            if (firstRootElement.ParentIndex == secondRootElement.ParentIndex)
                return;
            //throw new InvalidOperationException("Elements already exist in the same set");

            if (firstRootElement.Count >= secondRootElement.Count)
            {
                secondRootElement.ParentIndex = firstRootElement.ParentIndex;
                firstRootElement.Count += secondRootElement.Count;
            }
            else
            {
                firstRootElement.ParentIndex = secondRootElement.ParentIndex;
                secondRootElement.Count += firstRootElement.Count;
            }
        }

        /// <summary>
        /// Элемент структуры
        /// </summary>
        private class UnionFindItemInternal
        {
            /// <summary>
            /// Индекс родительского элемента
            /// </summary>
            public int ParentIndex { get; set; }

            /// <summary>
            /// Количество элементов в подмножестве
            /// </summary>
            public int Count { get; set; } = 1;

            /// <summary>
            /// Данные
            /// </summary>
            public UnionFindItem<TKey> Data { get; set; }
        }
    }
}
