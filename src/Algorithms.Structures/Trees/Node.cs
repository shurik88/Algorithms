using System;

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
    }
}
