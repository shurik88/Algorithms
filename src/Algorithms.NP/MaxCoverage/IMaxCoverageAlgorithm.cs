namespace Algorithms.NP.MaxCoverage
{
    /// <summary>
    ///     Алгоритм максимального покрытия
    /// </summary>
    public interface IMaxCoverageAlgorithm
    {
        /// <summary>
        ///     Найти решение.
        /// </summary>
        /// <param name="input">Входные данные</param>
        /// <returns>Результат</returns>
        AlgorithmOutput FindSolution(AlgorithmInput input);
    }
}
