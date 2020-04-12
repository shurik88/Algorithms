using System;
using System.Collections.Generic;

namespace Algorithms.DynamicProgramming.Knapsack
{
    /// <summary>
    /// Решение задачи о рюкзаке при помощи динамического программирования
    /// </summary>
    public class KnapsackAlgorithm : IKnapsackAlgorithm
    {
        /// <inheritdoc/>
        public IEnumerable<int> Get(Thing[] things, int maxSize)
        {
            var solutions = new int[things.Length + 1, maxSize + 1];
            for (var c = 0; c <= maxSize; ++c)
                solutions[0, c] = 0;

            for(var i = 1; i <= things.Length; ++i)
            {
                for(var c = 0; c <= maxSize; ++c)
                {
                    if (things[i - 1].Size > c)
                        solutions[i, c] = solutions[i - 1, c];
                    else
                        solutions[i, c] = Math.Max(solutions[i - 1, c], solutions[i - 1, c - things[i - 1].Size] + things[i - 1].Value);
                }
            }

            var remainingSize = maxSize;
            for(var i = things.Length; i >= 1; --i)
            {
                if(things[i-1].Size <= remainingSize 
                    && solutions[i - 1, remainingSize - things[i - 1].Size] + things[i - 1].Value >= solutions[i - 1, remainingSize])
                {
                    yield return i - 1;
                    remainingSize -= things[i - 1].Size;
                }
            }
        }
    }
}
