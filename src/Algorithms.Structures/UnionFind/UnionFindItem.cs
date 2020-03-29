namespace Algorithms.Structures.UnionFind
{
    /// <summary>
    ///     Элемент системы непересекающихъся множеств.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    public abstract class UnionFindItem<TKey>
    {
        /// <summary>
        /// Идентификатор элемента
        /// </summary>
        public TKey Id { get; set; }
    }
}
