using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Structures.Heaps
{
    /// <summary>
    /// Куча
    /// </summary>
    /// <typeparam name="TKey">Тип ключа кучи</typeparam>
    /// <typeparam name="TValue">Тип значения кучи</typeparam>
    public class Heap<TKey, TValue>
        where TKey: IComparable
    {
        private readonly ComparableElement<TKey, TValue>[] _heap;


        //private readonly Func<int, bool> _isChildCorrect;
        //private readonly Func<int, int, int> _elementCandidateSubstitution;
        //private readonly Action<int, TKey> _editKey;

        private readonly IHeapType _heapType;

        private interface IHeapType
        {
            bool IsChildCorrect(ComparableElement<TKey, TValue>[] heap, int childIndex);

            int SelectElementSubstituation(ComparableElement<TKey, TValue>[] heap, int firstIndex, int secondIndex);

            void EditKey(ComparableElement<TKey, TValue>[] heap, int index, TKey newValue);
        }

        private class MinHeapType : IHeapType
        {
            public void EditKey(ComparableElement<TKey, TValue>[] heap, int index, TKey newValue)
            {
                if (heap[index].Key.CompareTo(newValue) < 0)
                    throw new InvalidOperationException("Min heap does not support increase key.");

                heap[index].Key = newValue;
                while(index != 0 && heap[GetParentIndex(index)].Key.CompareTo(heap[index].Key) > 0)
                {
                    Swap(heap, index, GetParentIndex(index));
                    index = GetParentIndex(index);
                }
            }

            public bool IsChildCorrect(ComparableElement<TKey, TValue>[] heap, int childIndex) =>
                heap[GetParentIndex(childIndex)].Key.CompareTo(heap[childIndex].Key) <= 0;

            public int SelectElementSubstituation(ComparableElement<TKey, TValue>[] heap, int firstIndex, int secondIndex) =>
                heap[firstIndex].Key.CompareTo(heap[secondIndex].Key) <= 0 ? firstIndex : secondIndex;
        }

        private class MaxHeapType : IHeapType
        {
            public void EditKey(ComparableElement<TKey, TValue>[] heap, int index, TKey newValue)
            {
                if (heap[index].Key.CompareTo(newValue) > 0)
                    throw new InvalidOperationException("Max heap does not support decrease key.");

                heap[index].Key = newValue;
                while (index != 0 && heap[GetParentIndex(index)].Key.CompareTo(heap[index].Key) < 0)
                {
                    Swap(heap, index, GetParentIndex(index));
                    index = GetParentIndex(index);
                }
            }

            public bool IsChildCorrect(ComparableElement<TKey, TValue>[] heap, int childIndex) =>
                heap[GetParentIndex(childIndex)].Key.CompareTo(heap[childIndex].Key) >= 0;


            public int SelectElementSubstituation(ComparableElement<TKey, TValue>[] heap, int firstIndex, int secondIndex) =>
                heap[firstIndex].Key.CompareTo(heap[secondIndex].Key) >= 0 ? firstIndex : secondIndex;
        }

        /// <summary>
        ///     Список всех значений.
        /// </summary>
        public IEnumerable<TValue> Values => _heap.Take(Count).Select(x => x.Value);



        /// <summary>
        /// Создание экземпляра класса <see cref="Heap{TKey, TValue}"/>
        /// </summary>
        /// <param name="heapType">Тип кучи</param>
        /// <param name="maxSize">Максимальный размер кучи</param>
        public Heap(HeapType heapType, int maxSize)
        {
            if (maxSize <= 0)
                throw new ArgumentException($"{nameof(maxSize)} should be greater than 0", nameof(maxSize));

            _heap = new ComparableElement<TKey, TValue>[maxSize];
            switch(heapType)
            {
                case HeapType.Min:
                    _heapType = new MinHeapType();
                    //_isChildCorrect = (index) => index == 0 || _heap[GetParentIndex(index)].Key.CompareTo(_heap[index].Key) <= 0;
                    //_elementCandidateSubstitution = (int first, int second) => _heap[first].Key.CompareTo(_heap[second].Key) <= 0 ? first : second;
                    break;
                case HeapType.Max:
                    _heapType = new MaxHeapType();
                    //_isChildCorrect = (index) => index == 0 || _heap[GetParentIndex(index)].Key.CompareTo(_heap[index].Key) >= 0;
                    //_elementCandidateSubstitution = (int first, int second) => _heap[first].Key.CompareTo(_heap[second].Key) >= 0 ? first : second;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(heapType), $"unhandled value: {heapType}");
            }
        }

        /// <summary>
        /// Количество элементов
        /// </summary>
        public int Count { get; private set; } = 0;

        /// <summary>
        /// Список элементов
        /// </summary>
        public IEnumerable<ComparableElement<TKey, TValue>> Elements => _heap.Take(Count);

        /// <summary>
        /// Является ли куча пустой
        /// </summary>
        public bool IsEmpty => Count == 0;

        /// <summary>
        /// Получить корневой элемент
        /// </summary>
        public ComparableElement<TKey, TValue> First => !IsEmpty
            ? _heap[0] 
            : throw new InvalidOperationException("Heap is empty. Try to insert first");

        /// <summary>
        /// Вставка элемента в кучу
        /// </summary>
        /// <param name="element">Элемента</param>
        public void Insert(ComparableElement<TKey, TValue> element)
        {
            if (Count == _heap.Length)
                throw new InvalidOperationException("Too many elements in heap. Try to extract elment before");

            _heap[Count] = element;
            HeapifyUp();
            Count++;
        }

        /// <summary>
        /// Изменение ключа элемента по индексу
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        /// <param name="newKey">Новое значение ключа</param>
        public void EditElementKey(int index, TKey newKey)
        {
            if (IsEmpty)
                throw new InvalidOperationException("Heap is empty");

            if (index >= Count)
                throw new ArgumentException($"Invalid index. Max value is: {Count - 1}", nameof(index));

            _heapType.EditKey(_heap, index, newKey);
        }

        private void HeapifyBottom()
        {
            var currIndex = 0;
            while(GetLeftChildIndex(currIndex) < Count)
            {
                var leftIndex =  GetLeftChildIndex(currIndex);
                var rightIndex = GetRightChildIndex(currIndex);
                var bestIndex = rightIndex < Count ? _heapType.SelectElementSubstituation(_heap, leftIndex, rightIndex) : leftIndex;
                var bestWithParentIndex = _heapType.SelectElementSubstituation(_heap, currIndex, bestIndex);
                if (bestWithParentIndex == currIndex)
                    break;
                Swap(_heap, currIndex, bestWithParentIndex);
                currIndex = bestWithParentIndex;
            }
        }

        private void HeapifyUp()
        {
            var currIndex = Count;
            while (currIndex != 0)
            {
                if (currIndex == 0 || _heapType.IsChildCorrect(_heap, currIndex))
                    return;
                var parentIndex = GetParentIndex(currIndex);
                Swap(_heap, currIndex, parentIndex);
                currIndex = parentIndex;
            }
        }

        private static void Swap(ComparableElement<TKey, TValue>[] array, int first, int second)
        {
            if (first == second)
                return;

            var temp = array[first];
            array[first] = array[second];
            array[second] = temp;
        }


        /// <summary>
        /// Извлечь первый элемент
        /// </summary>
        /// <returns>Элемент в вершине кучи</returns>
        public ComparableElement<TKey, TValue> Extract()
        {
            if(IsEmpty)
                throw new InvalidOperationException("Heap is empty. Try to insert first");

            var extracred = First;
            Count--;
            if (!IsEmpty)
            {
                _heap[0] = _heap[Count];
                _heap[Count] = null;
                HeapifyBottom();
            }
            return extracred;
        }

        private static int GetLeftChildIndex(int parentIndex) => 2 * parentIndex + 1;
        private static int GetRightChildIndex(int parentIndex) => 2 * parentIndex + 2;

        private static int GetParentIndex(int childIndex) => (childIndex - 1) / 2;
    }
}
