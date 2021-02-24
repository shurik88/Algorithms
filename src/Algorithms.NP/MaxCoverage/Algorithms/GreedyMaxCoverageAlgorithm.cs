using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.NP.MaxCoverage.Algorithms
{
    /// <summary>
    ///     Жадный алгоритм максимального покрытия
    /// </summary>
    public class GreedyMaxCoverageAlgorithm : IMaxCoverageAlgorithm
    {
        /// <inheritdoc/>
        public AlgorithmOutput FindSolution(AlgorithmInput input)
        {
            var sets = input.Sets.ToList();
            if (sets.Count < input.UsedSetsCount)
                throw new ArgumentException("Invalid UsedSetsCount value", nameof(input.UsedSetsCount));

            var setElements = new HashSet<int>();
            var usedSets = new List<int[]>();
            for(var i = 0; i < input.UsedSetsCount; ++i)
            {
                var bestSetIndex = -1;
                var bestAddingElements = Array.Empty<int>();
                for(var j = 0; j < sets.Count; ++j)
                {
                    var set = sets[j];
                    var tempAddingElements = set.Where(x => !setElements.Contains(x)).ToArray();
                    if (tempAddingElements.Length <= bestAddingElements.Length)
                        continue;

                    bestAddingElements = tempAddingElements;
                    bestSetIndex = j;
                }
                if (bestSetIndex == -1)
                    break;

                foreach (var adding in bestAddingElements)
                    setElements.Add(adding);
                usedSets.Add(sets[bestSetIndex]);
                sets.RemoveAt(bestSetIndex);
            }

            return new AlgorithmOutput
            {
                Count = setElements.Count,
                UsedSets = usedSets
            };
        }
    }
}
