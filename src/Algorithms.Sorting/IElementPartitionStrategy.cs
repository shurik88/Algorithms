namespace Algorithms.Sorting
{
    /// <summary>
    /// Стратегия выбора опорного элемента
    /// </summary>
    public interface IElementPartitionStrategy
    {
        /// <summary>
        /// Получение индекса опорного элемента для разбиения массива
        /// </summary>
        /// <param name="arrayLength">Размер массива</param>
        /// <returns>Индекс элемента</returns>
        int GetIndex(int arrayLength);
    }
}
