using System;

namespace Algorithms.Structures.Trees
{
    public class KDTreePoint<TValue>
    {
        public int[] Coordinates { get; set; }

        public TValue Value { get; set; }
    }
}
