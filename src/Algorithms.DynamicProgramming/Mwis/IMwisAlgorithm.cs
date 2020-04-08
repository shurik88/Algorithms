using System;
using System.Collections.Generic;

namespace Algorithms.DynamicProgramming.Mwis
{
    /// <summary>
    ///  Интерфейс алгоритма нахождения независимого множества графа с максимальным весом
    /// </summary>
    /// <remarks>
    /// Для простоты рассматривается только путевые графы
    /// </remarks>
    public interface IMwisAlgorithm
    {
        /// <summary>
        /// Получение перечня вершин(индексов) MWIS
        /// </summary>
        /// <param name="vertexWieghts">Веса вершин</param>
        /// <returns>Индексы вершин MWIS</returns>
        IEnumerable<int> GetVetexesIndexes(int[] vertexWieghts);
    }
}
