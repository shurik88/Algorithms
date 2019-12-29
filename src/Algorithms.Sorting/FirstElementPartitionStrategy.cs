namespace Algorithms.Sorting
{
    /// <summary>
    /// Стратегия выбора опорного элемента
    /// всегда выбирается первый элемент
    /// </summary>
    public class FirstElementPartitionStrategy : IElementPartitionStrategy
    {
        /// <inheritdoc/>
        public int GetIndex(int arrayLength) => 0;
    }
}
