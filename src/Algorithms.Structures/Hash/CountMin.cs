using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Structures.Hash
{
    public class CountMin<T>
    {
        private readonly int[,] matrix;
        private readonly Func<T, int>[] _hashFunctions;
        public CountMin(double eps, double delta)
        {
            var width = Convert.ToInt32(Math.Ceiling(Math.E / eps));
            var depth = Convert.ToInt32(Math.Ceiling(Math.Log(1.0/ delta)));
            matrix = new int[depth, width];
            _hashFunctions = Enumerable.Range(0, depth).Select((x, i) => new Func<T, int>(y => y.GetHashCode() * (i + 1) % width)).ToArray();
        }

        public void Update(T item, int count = 1)
        {
            for(var d = 0; d < _hashFunctions.Length; ++d)
            {
                var hash = _hashFunctions[d](item);
                matrix[d, hash] += count;
            }
        }

        public int GetCount(T item)
        {
            return _hashFunctions.Select((x, index) => matrix[index, x(item)]).Min();
        }
    }
}
