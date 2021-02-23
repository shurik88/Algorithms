using System.Collections.Generic;

namespace Algorithms.NP.Jssp
{
    /// <summary>
    ///     Входные данные для алгоритма.
    /// </summary>
    public class AlgorithmInput
    {
        /// <summary>
        ///     Список работ.
        /// </summary>
        public IEnumerable<Job> Jobs { get; set; }

        /// <summary>
        ///     Список машин.
        /// </summary>
        public IEnumerable<Machine> Machines { get; set; }
    }
}
