using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Structures.Trees
{
    /// <summary>
    /// Бинарное дерево поиска
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    public class BinarySearchTree<TKey, TValue>
        where TKey : IComparable
    {

        private Node<TKey, TValue> _root = null;
        private int _count = 0;

        /// <summary>
        /// Количество элементов в дереве
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Является ли дерево пустым
        /// </summary>
        public bool IsEmpty => _root == null;

        /// <summary>
        /// Найти элемент по ключу
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ComparableElement<TKey, TValue> Find(TKey key) => FindInternal(key);


        /// <summary>
        /// Минимальный элемент по ключу
        /// </summary>
        public ComparableElement<TKey, TValue> Min => _root?.MaxLeft;

        /// <summary>
        /// Максимальный элемент по ключу
        /// </summary>
        public ComparableElement<TKey, TValue> Max => _root?.MaxRight;

        /// <summary>
        /// Отсортированный список элементов по возрастанию
        /// </summary>
        public IEnumerable<ComparableElement<TKey, TValue>> OrderedList
        {
            get
            {
                return _root == null 
                    ? new List<ComparableElement<TKey, TValue>>() 
                    : GetOrderedItems(_root).Cast<ComparableElement<TKey, TValue>>();
            }
        }

        private IEnumerable<Node<TKey, TValue>> GetOrderedItems(Node<TKey, TValue> node)
        {
            if (node.Left != null)
            {
                foreach (var item in GetOrderedItems(node.Left))
                    yield return item;
            }
            yield return node;
            if(node.Right != null)
            {
                foreach (var item in GetOrderedItems(node.Right))
                    yield return item;
            }
        }

        /// <summary>
        /// Найти предшественика элемента
        /// </summary>
        /// <param name="key">Ключ элемента</param>
        /// <returns>Элемент</returns>
        public ComparableElement<TKey, TValue> FindPredecessor(TKey key)
        {
            var node = FindInternal(key);
            return node == null || node.Left == null ? null : node.Left.MaxRight;
        }

        /// <summary>
        /// Найти преемника элемента
        /// </summary>
        /// <param name="key">Ключ элемента</param>
        /// <returns>Элемент</returns>
        public ComparableElement<TKey, TValue> FindSuccessor(TKey key)
        {
            var node = FindInternal(key);
            return node == null || node.Right == null ? null : node.Right.MaxLeft;
        }

        /// <summary>
        /// Удалить элемент по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        public void Delete(TKey key)
        {
            var node = FindInternal(key);
            if (node == null)
                throw new ArgumentException($"Element with key: {key} not found.", nameof(key));

            DeleteInternal(node);
            _count--;
        }

        public void DeleteInternal(Node<TKey, TValue> node)
        {
            var parent = node.Parent;
            if (node.Left == null && node.Right == null)
            {
                if (parent == null)
                    _root = null;
                else if (parent.Left == node)
                    parent.Left = null;
                else
                    parent.Right = null;
            }
            else if (node.Left != null && node.Right == null || node.Right != null && node.Left == null)
            {
                var child = node.Left ?? node.Right;
                if (parent == null)
                    _root = child;
                else if (parent.Left == node)
                    parent.Left = child;
                else
                    parent.Right = child;
                child.Parent = parent;
            }
            else
            {
                var newNode = node.Left.MaxRight;
                SwapNodes(node, newNode);
                DeleteInternal(node);
            }
        }

        /// <summary>
        /// Поменять местами два узла
        /// </summary>
        /// <remarks>
        /// Выполнение данного метода может привести к нарушению условия двоичного дерева поиска
        /// </remarks>
        /// <param name="first">Узел 1</param>
        /// <param name="second">Узел 2</param>
        private static void SwapNodes(Node<TKey, TValue> first, Node<TKey, TValue> second)
        {
            if (first == second)
                return;
            var tempParent = first.Parent;
            var tempLeft = first.Left;
            var tempRight = first.Right;
            var firstIsLeftChild = tempParent != null && tempParent.Left == first;

            if (second.Left != first)
            {
                first.Left = second.Left;
                if (first.Left != null)
                    first.Left.Parent = first;
            }
            else
            {
                first.Left = second;
            }

            if (second.Right != first)
            {
                first.Right = second.Right;
                if (first.Right != null)
                    first.Right.Parent = first;
            }
            else
                first.Right = second;

            if (second.Parent != first)
            {
                first.Parent = second.Parent;
                if (first.Parent != null)
                {
                    if (first.Parent.Left == second)
                        first.Parent.Left = first;
                    else
                        first.Parent.Right = first;
                }
            }
            else
                first.Parent = second;

            if (tempLeft != second)
            {
                second.Left = tempLeft;
                if (second.Left != null)
                    second.Left.Parent = second;
            }
            else
                second.Left = first;
            if (tempRight != second)
            {
                second.Right = tempRight;
                if (second.Right != null)
                    second.Right.Parent = second;
            }
            else
                second.Right = first;
            if (tempParent != second)
            {
                second.Parent = tempParent;
                if (second.Parent != null)
                {
                    if (firstIsLeftChild)
                        second.Parent.Left = second;
                    else
                        second.Parent.Right = second;
                }
            }
            else
                second.Parent = first;
        }

        /// <summary>
        /// Вставить элемент в дерево
        /// </summary>
        /// <param name="element">Элемент</param>
        public void Insert(ComparableElement<TKey, TValue> element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            Node<TKey, TValue> prev = null;
            var curr = _root;
            while(curr != null)
            {
                prev = curr;
                if (curr.Key.CompareTo(element.Key) < 0)
                    curr = curr.Right;
                else if (curr.Key.CompareTo(element.Key) > 0)
                    curr = curr.Left;
                else
                    throw new ArgumentException($"element with key:{element.Key} already exists.", nameof(element));
            }
            if(prev == null)
                _root = new Node<TKey, TValue> { Key = element.Key, Value = element.Value };
            else
            {
                var node = new Node<TKey, TValue> { Key = element.Key, Value = element.Value, Parent = prev };
                if (prev.Key.CompareTo(node.Key) < 0)
                    prev.Right = node;
                else
                    prev.Left = node;
            }
            _count++;
        }

        private Node<TKey, TValue> FindInternal(TKey key)
        {
            var curr = _root;
            while (curr != null)
            {
                if (curr.Key.CompareTo(key) == 0)
                    return curr;
                if (curr.Key.CompareTo(key) < 0)
                    curr = curr.Right;
                else
                    curr = curr.Left;
            }
            return null;
        }

        //private Node<TKey, TValue> FindMaxRight(Node<TKey, TValue>)
    }
}
