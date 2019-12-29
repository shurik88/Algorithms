using System;

namespace Algorithms.Sorting
{
    /// <summary>
    /// Методы расширения для работы с массивом
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Поменять элементы массива местами
        /// </summary>
        /// <typeparam name="TElement">Тип элемента массива</typeparam>
        /// <param name="array">Массив</param>
        /// <param name="first">Первый элемент</param>
        /// <param name="second">Второй элемент</param>
        public static void Swap<TElement>(this TElement[] array, int first, int second)
        {
            if (first == second)
                return;

            var temp = array[first];
            array[first] = array[second];
            array[second] = temp;
        }

        /// <summary>
        /// Разбить элементы массива относительно опорного элемента
        /// </summary>
        /// <remarks>
        /// Все элементы, которые меньше опорного, будут находиться слева, больше - справа
        /// </remarks>
        /// <typeparam name="TElement">Тип элементов массива</typeparam>
        /// <param name="array">Массив</param>
        /// <param name="left">Левая граница</param>
        /// <param name="right">Правая граница</param>
        /// <param name="index">Индекс опорного элемента</param>
        /// <returns>Новый индекс опорного элемента, полученный в результате перестановок</returns>
        public static int Partition<TElement>(this TElement[] array, int left, int right, int index) where TElement : IComparable
        {
            if (left < 0 || left >= array.Length)
                throw new ArgumentException("Index outside array boundaries", nameof(left));
            if (right < 0 || right >= array.Length)
                throw new ArgumentException("Index outside array boundaries", nameof(right));
            if (left > right)
                throw new ArgumentException($"{nameof(right)} should be greater or equal than {nameof(left)}");

            var element = array[index];
            if (index != left)
                Swap(array, index, left);

            var currRight = left + 1;
            for (var i = left + 1; i <= right; ++i)
            {
                if (array[i].CompareTo(element) > 0)
                    continue;

                Swap(array, currRight, i);
                currRight++;
            }
            Swap(array, left, currRight - 1);
            return currRight - 1;
        }
    }
}
