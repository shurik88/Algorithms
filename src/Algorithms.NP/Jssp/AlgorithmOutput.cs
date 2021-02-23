using System.Collections.Generic;

namespace Algorithms.NP.Jssp
{
    /// <summary>
    ///     Выходные данные алгоритма.
    /// </summary>
    public class AlgorithmOutput
    {
        /// <summary>
        ///     Список назначений.
        /// </summary>
        public IEnumerable<Assignment> Assignments { get; set; }

        /// <summary>
        ///     Суммарная длинная плана (продолжительность) - максимальная длительность среди машин.
        /// </summary>
        public int Makespan { get; set; }
    }
}
