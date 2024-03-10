using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Structures.Hash
{
    /// <summary>
    ///     Узел для согласованного хеширования.
    /// </summary>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    /// <typeparam name="TResource">Тип ресурса</typeparam>
    public class HashRingNode<TValue, TResource>
    {
        public HashRingNode(int key, TValue value)
        {
            Key = key;
            Value = value;
        }
        public int Key { get; private set; }

        public TValue Value { get; private set; }

        private readonly IList<HashRingResource<TResource>> _resources = new List<HashRingResource<TResource>>();

        public IEnumerable<HashRingResource<TResource>> Resources { get { return _resources; } }

        public IEnumerable<int> ResorucesKeys => Resources.Select(x => x.Key);

        public void RemoveResource(HashRingResource<TResource> resource)
        {
            var delRes = _resources.Remove(resource);
            if (!delRes)
                throw new ArgumentException("Resource not found", nameof(resource));
        }

        public void AddResource(HashRingResource<TResource> resource)
        {
            _resources.Add(resource);
        }


    }
}
