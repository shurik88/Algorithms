using System;
using System.Collections.Generic;

namespace Algorithms.DynamicProgramming.Mwis
{
    /// <summary>
    /// Восходящий алгоритм реконтсрукции
    /// </summary>
    public class UpwardLinearMwisAlgorithm : IMwisAlgorithm
    {
        /// <inheritdoc/>
        public IEnumerable<int> GetVetexesIndexes(int[] vertexWieghts)
        {
            var maxWeights = new int[vertexWieghts.Length + 1];
            maxWeights[0] = 0;
            maxWeights[1] = vertexWieghts[0];

            for (var i = 1; i < vertexWieghts.Length; ++i)
                maxWeights[i + 1] = Math.Max(maxWeights[i - 1] + vertexWieghts[i], maxWeights[i]);

            var currentIndex = vertexWieghts.Length;
            while(currentIndex >= 2)
            {
                if (maxWeights[currentIndex - 1] >= maxWeights[currentIndex - 2] + vertexWieghts[currentIndex - 1])
                    currentIndex--;
                else
                {
                    yield return currentIndex - 1;
                    currentIndex -= 2;
                }
            }

            if (currentIndex == 1)
                yield return 0;
            
        }
    }
}
