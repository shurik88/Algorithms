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
        private readonly HeapElement<TKey, TValue>[] _heap;
        private int _currElementIndex = 0;

        private readonly Func<int, bool> _isChildCorrect;
        private readonly Func<int, int, int> _elementCandidateSubstitution;

        /// <summary>
        /// Создание экземпляра класса <see cref="Heap{TKey, TValue}"/>
        /// </summary>
        /// <param name="heapType">Тип кучи</param>
        /// <param name="maxSize">Максимальный размер кучи</param>
        public Heap(HeapType heapType, int maxSize)
        {
            if (maxSize <= 0)
                throw new ArgumentException($"{nameof(maxSize)} should be greater than 0", nameof(maxSize));

            _heap = new HeapElement<TKey, TValue>[maxSize];
            switch(heapType)
            {
                case HeapType.Min:
                    _isChildCorrect = (index) => index == 0 || _heap[GetParentIndex(index)].Key.CompareTo(_heap[index].Key) <= 0;
                    _elementCandidateSubstitution = (int first, int second) => _heap[first].Key.CompareTo(_heap[second].Key) <= 0 ? first : second;
                    break;
                case HeapType.Max:
                    _isChildCorrect = (index) => index == 0 || _heap[GetParentIndex(index)].Key.CompareTo(_heap[index].Key) >= 0;
                    _elementCandidateSubstitution = (int first, int second) => _heap[first].Key.CompareTo(_heap[second].Key) >= 0 ? first : second;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(heapType), $"unhandled value: {heapType}");
            }
        }

        /// <summary>
        /// Количество элементов
        /// </summary>
        public int Count => _currElementIndex;

        /// <summary>
        /// Список элементов
        /// </summary>
        public IEnumerable<HeapElement<TKey, TValue>> Elements => _heap.Take(_currElementIndex);

        /// <summary>
        /// Является ли куча пустой
        /// </summary>
        public bool IsEmpty => _currElementIndex == 0;

        /// <summary>
        /// Получить корневой элемент
        /// </summary>
        public HeapElement<TKey, TValue> First => !IsEmpty
            ? _heap[0] 
            : throw new InvalidOperationException("Heap is empty. Try to insert first");

        /// <summary>
        /// Вставка элемента в кучу
        /// </summary>
        /// <param name="element">Элемента</param>
        public void Insert(HeapElement<TKey, TValue> element)
        {
            if (_currElementIndex == _heap.Length)
                throw new InvalidOperationException("Too many elements in heap. Try to extract elment before");

            _heap[_currElementIndex] = element;
            HeapifyUp();
            _currElementIndex++;
        }

        private void HeapifyBottom()
        {
            var currIndex = 0;
            while(GetLeftChildIndex(currIndex) < _currElementIndex)
            {
                var leftIndex =  GetLeftChildIndex(currIndex);
                var rightIndex = GetRightChildIndex(currIndex);
                var bestIndex = rightIndex < _currElementIndex ? _elementCandidateSubstitution(leftIndex, rightIndex) : leftIndex;
                var bestWithParentIndex = _elementCandidateSubstitution(currIndex, bestIndex);
                if (bestWithParentIndex == currIndex)
                    break;
                Swap(_heap, currIndex, bestWithParentIndex);
                currIndex = bestWithParentIndex;
            }
        }

        private void HeapifyUp()
        {
            var currIndex = _currElementIndex;
            while (currIndex != 0)
            {
                if (_isChildCorrect(currIndex))
                    return;
                var parentIndex = GetParentIndex(currIndex);
                Swap(_heap, currIndex, parentIndex);
                currIndex = parentIndex;
            }
        }

        private static void Swap(HeapElement<TKey, TValue>[] array, int first, int second)
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
        public HeapElement<TKey, TValue> Extract()
        {
            if(IsEmpty)
                throw new InvalidOperationException("Heap is empty. Try to insert first");

            var extracred = First;
            _currElementIndex--;
            if (!IsEmpty)
            {
                _heap[0] = _heap[_currElementIndex];
                _heap[_currElementIndex] = null;
                HeapifyBottom();
            }
            return extracred;
        }

        public static Heap<TKey, TValue> CreateMinHeap(HeapElement<TKey, TValue>[] data)
        {
            throw new NotImplementedException();
        }

        public static Heap<TKey, TValue> CreateMaxHeap(HeapElement<TKey, TValue>[] data)
        {
            throw new NotImplementedException();
        }

        private static int GetLeftChildIndex(int parentIndex) => 2 * parentIndex + 1;
        private static int GetRightChildIndex(int parentIndex) => 2 * parentIndex + 2;

        private static int GetParentIndex(int childIndex) => (childIndex - 1) / 2;
    }
}
