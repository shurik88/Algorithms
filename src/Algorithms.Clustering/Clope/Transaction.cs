using System.Collections.Generic;

namespace Algorithms.Clustering.Clope
{
    /// <summary>
    /// Транзакция алгоритма CLOPE
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Идентификатор транзакции
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификаторы объектов(каткегорий, меток)
        /// </summary>
        public IEnumerable<long> Objects { get; set; }
    }
}
