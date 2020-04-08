using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.DynamicProgramming.Mwis
{
    /// <summary>
    /// Алгоритм грубой силы поиска Mwis
    /// </summary>
    public class BruteForceMwisAlgorithm : IMwisAlgorithm
    {
        /// <inheritdoc/>
        public IEnumerable<int> GetVetexesIndexes(int[] vertexWieghts)
        {
            return GetIndexes(vertexWieghts, vertexWieghts.Length);
        }

        protected virtual IEnumerable<int> GetIndexes(int[] vertexWieghts, int current)
        {
            if (current == 0)
                return new List<int>();
            if (current == 1)
                return new List<int> { 0 };

            var gPrev1 = GetIndexes(vertexWieghts, current - 1);
            var gPrev2 = GetIndexes(vertexWieghts, current - 2);
            var weightPrev1 = GetWeight(vertexWieghts, gPrev1);
            var weightPrev2 = GetWeight(vertexWieghts, gPrev2);


            return weightPrev1 >= weightPrev2 + vertexWieghts[current - 1]
                ? gPrev1 
                : gPrev2.Concat(new List<int> { current - 1 });
        }

        private int GetWeight(int[] vertexWieghts, IEnumerable<int> indexes)
        {
            var sum = 0;
            foreach (var index in indexes)
                sum += vertexWieghts[index];

            return sum;
        }
    }
}
