namespace Algorithms.Structures.Hash
{
    /// <summary>
    ///     Функция хеширования.
    /// </summary>
    public interface IHashFunc
    {
        /// <summary>
        ///     Получить хеш.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        int GetHash(int key);

        /// <summary>
        ///     Максимальное значение ключа.
        /// </summary>
        int Max { get; }
    }
}
