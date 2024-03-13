using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Algorithms.Structures.Hash
{
    public class BloomFilter<T>
        //where T : IEqualityComparer<T>
    {
        private readonly BitArray _array;
        private readonly IEnumerable<Func<T, int>> _hashFunctions;

        public BloomFilter(int expectedElements, double mistakesFreq)
        {
            var storageSize = Convert.ToInt32(-Math.Log(mistakesFreq) * expectedElements / Math.Pow(Math.Log(2), 2));
            _array = new BitArray(storageSize, false);
            var hashCount = Convert.ToInt32(storageSize * Math.Log(2) / expectedElements);
            _hashFunctions = Enumerable.Range(0, hashCount).Select((x, i) => new Func<T, int>(y => y.GetHashCode() * (i + 1) % storageSize)).ToList();
        }

        public BloomFilter(int storageSize, int expectedElements)
        {
            _array= new BitArray(storageSize);
            var hashCount = Convert.ToInt32(Math.Ceiling((storageSize * 1.0 / expectedElements) * Math.Log(2)));
            _hashFunctions = Enumerable.Range(0, hashCount).Select((x, i) => new Func<T, int>(y => y.GetHashCode() * (i + 1) % storageSize)).ToList();
        }

        public void Add(T item)
        {
            foreach(var func in _hashFunctions)
                _array[func(item)] = true;
        }

        public bool Contains(T item)
        {
            return _hashFunctions.All(func => _array[func(item)]);
        }
    }
}
