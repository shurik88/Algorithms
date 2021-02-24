using System.Collections.Generic;

namespace Algorithms.NP.MaxCoverage
{
    public class AlgorithmOutput
    {
        /// <summary>
        ///     Список используемых множеств с элементами
        /// </summary>
        public IEnumerable<int[]> UsedSets { get; set; }

        /// <summary>
        ///     Кол-во охваченных элементов
        /// </summary>
        public int Count { get; set; }
    }
}
