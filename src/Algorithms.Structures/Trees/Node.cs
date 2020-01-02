using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Structures.Trees
{
    /// <summary>
    /// Узел дерева
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    public class Node<TKey, TValue>:ComparableElement<TKey, TValue>
        where TKey : IComparable
    {
        /// <summary>
        /// Родитель
        /// </summary>
        public Node<TKey, TValue> Parent { get; set; }

        /// <summary>
        /// Левый ребенок
        /// </summary>
        public Node<TKey, TValue> Left { get; set; }

        /// <summary>
        /// Правый ребенок
        /// </summary>
        public Node<TKey, TValue> Right { get; set; }

        /// <summary>
        /// Крайний правый элемент
        /// </summary>
        public Node<TKey, TValue> MaxRight
        {
            get
            {
                var curr = this;
                while (curr.Right != null)
                    curr = curr.Right;
                return curr;
            }
        }

        /// <summary>
        /// Крайний левый элемент
        /// </summary>
        public Node<TKey, TValue> MaxLeft
        {
            get
            {
                var curr = this;
                while (curr.Left != null)
                    curr = curr.Left;
                return curr;
            }
        }

        /// <summary>
        /// Количество элементов в поддереве
        /// </summary>
        public int SubTreeNodesCount
        {
            get
            {
                var count = 0;
                var stack = new Stack<Node<TKey, TValue>>();
                stack.Push(this);
                while(stack.Any())
                {
                    var elem = stack.Pop();
                    count++;
                    if (elem.Left != null)
                        stack.Push(elem.Left);
                    if (elem.Right != null)
                        stack.Push(elem.Right);
                }
                return count;
            }
        }

        /// <summary>
        /// Высота поддерева
        /// </summary>
        public int Height
        {
            get
            {
                var max = 0;
                var dictHeights = new Dictionary<TKey, int>();
                var queue = new Queue<Node<TKey, TValue>>();
                queue.Enqueue(this);
                while (queue.Any())
                {
                    var elem = queue.Dequeue();
                    var value = elem == this ? 1 : dictHeights[elem.Parent.Key] + 1;
                    dictHeights.Add(elem.Key, value);
                    if (value > max)
                        max = value;
                    if (elem.Left != null)
                        queue.Enqueue(elem.Left);
                    if (elem.Right != null)
                        queue.Enqueue(elem.Right);
                }
                return max;
            }
        }

        /// <summary>
        /// Высота левого поддерева
        /// </summary>
        public int LeftSubTreeHeight => Left?.Height ?? 0;

        /// <summary>
        /// Высота правого поддерева
        /// </summary>
        public int RightSubTreeHeight => Right?.Height ?? 0;
    }
}
