namespace Algorithms.NP.Jssp
{
    /// <summary>
    ///     Алгоритм Job shop scheduling problem
    /// </summary>
    public interface IJsspAlgorithm
    {
        /// <summary>
        ///     Найти решение.
        /// </summary>
        /// <param name="input">Входные данные</param>
        /// <returns>Результат</returns>
        AlgorithmOutput FindSolution(AlgorithmInput input);
    }
}
