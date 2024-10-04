using System;
using System.Linq;

namespace Algorithms.Structures.Trees
{
    public class KDTreeNode<TValue>
    {
        public int[] Coordinates { get; set; }

        public KDTreeNode<TValue> Parent { get; set; }

        public KDTreeNode<TValue> Left { get; set; }

        public KDTreeNode<TValue> Right { get; set; }

        public int Dimension { get; set; }

        public bool IsLeaf { get; set; }

        public TValue Value { get; set; }

        public KDTreePoint<TValue> Point => new() { Coordinates = Coordinates, Value = Value };

        //private KDTreeNode()
        //{
            
        //}

        //public static KDTreeNode<TValue> CreateNode(KDTreeNode<TValue> parent, int[] point)
        //{
        //    return new KDTreeNode<TValue> { Point = point, Parent = parent };
        //}

        //public static KDTreeNode<TValue> CreateLeaf(KDTreeNode<TValue> parent, int[] point, TValue value)
        //{
        //    return new KDTreeNode<TValue> { Point = point, IsLeaf = true, Value = value, Parent = parent };
        //}
    }
}
