using System;

namespace Algorithms.Sorting
{
    /// <summary>
    /// Сортировка слиянием
    /// </summary>
    public class MergeSortAlgorithm : ISortAlgorithm
    {
        /// <inheritdoc/>
        public void Sort<TElement>(TElement[] array) where TElement : IComparable
        {
            var sortedArray = SortInternal(array, 0, array.Length - 1);
            sortedArray.CopyTo(array, 0);
        }

        private static TElement[] SortInternal<TElement>(TElement[] array, int left, int right) where TElement : IComparable
        {
            if (left >= right)
                return new TElement[1] { array[left] };
            var middle = (left + right) / 2;
            var leftPart = SortInternal(array, left, middle);
            var rightPart = SortInternal(array, middle + 1, right);
            return MergeInternal(leftPart, rightPart);
        }

        private static TElement[] MergeInternal<TElement>(TElement[] leftPart, TElement[] rightPart) where TElement : IComparable
        {
            var currLeftI = 0;
            var currRightI = 0;
            var merged = new TElement[leftPart.Length + rightPart.Length];
            var currI = 0;
            while (currLeftI < leftPart.Length || currRightI < rightPart.Length)
            {
                if(currLeftI != leftPart.Length && (currRightI == rightPart.Length || leftPart[currLeftI].CompareTo(rightPart[currRightI]) <= 0))
                {
                    merged[currI] = leftPart[currLeftI];
                    currLeftI++;
                }
                else
                {
                    merged[currI] = rightPart[currRightI];
                    currRightI++;
                }
                currI++;
            }
            return merged;
        }
    }
}
