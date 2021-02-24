using System.Collections.Generic;

namespace Algorithms.NP.MaxCoverage
{
    /// <summary>
    ///     Входные данные для алгоритма максимального покрытия
    /// </summary>
    public class AlgorithmInput
    {
        /// <summary>
        ///     Список множеств с элементами
        /// </summary>
        public IEnumerable<int[]> Sets { get; set; }

        /// <summary>
        ///     Разрешенное кол-во испоьлзуемых множеств 
        /// </summary>
        public int UsedSetsCount { get; set; }
    }
}
