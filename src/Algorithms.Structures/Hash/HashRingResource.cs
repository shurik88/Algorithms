namespace Algorithms.Structures.Hash
{
    /// <summary>
    ///     Ресурс для соглсованного хеширования.
    /// </summary>
    /// <typeparam name="TValue">Тип значения ресурса</typeparam>
    public class HashRingResource<TValue>
    {
        public int Key { get; set; }

        public TValue Value { get; set; }
    }
}
