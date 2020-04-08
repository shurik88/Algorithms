using System.Collections.Generic;

namespace Algorithms.DynamicProgramming.Mwis
{
    /// <summary>
    /// Алгоритм грубой силы поиска Mwis с использованием кеша
    /// </summary>
    public class CacheBruteForceMwisAlgorithm: BruteForceMwisAlgorithm
    {
        private IDictionary<int, IEnumerable<int>> _cache = new Dictionary<int, IEnumerable<int>>();

        protected override IEnumerable<int> GetIndexes(int[] vertexWieghts, int current)
        {
            if(current == vertexWieghts.Length)
                _cache = new Dictionary<int, IEnumerable<int>>();

            if (!_cache.ContainsKey(current))
                _cache[current] = base.GetIndexes(vertexWieghts, current);

            return _cache[current];
        }
    }
}
