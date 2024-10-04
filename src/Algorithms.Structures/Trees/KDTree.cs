using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Algorithms.Structures.Trees
{
    /// <summary>
    ///     К-мерное дерево(KD Дерево).
    /// </summary>
    /// <typeparam name="int">Тип координат</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    public class KDTree<TValue>
    {
        private KDTree()
        {
            
        }

        public bool IsEmpty => Root == null;

        public int Count { get; private set; }

        /// <summary>
        ///     Корень дерева.
        /// </summary>
        public KDTreeNode<TValue> Root { get; private set; }

        /// <summary>
        ///     Количество измерений.
        /// </summary>
        public int DimensionCount => Root.Coordinates.Length;

        /// <summary>
        ///     Построить дерево.
        /// </summary>
        /// <param name="points">Точки</param>
        /// <returns>Дерево</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static KDTree< TValue> BuildTree(KDTreePoint< TValue>[] points)
        {
            if(points == null) 
                throw new ArgumentNullException(nameof(points));
            if (!points.Any())
                throw new ArgumentException("empty array", nameof(points));

            if (points.Any(x => x.Coordinates == null || !x.Coordinates.Any()))
                throw new ArgumentException("coordinates is required not empty", nameof(points));

            var groupByDimension = points.GroupBy(x => x.Coordinates.Length);
            if(groupByDimension.Count() != 1)
                throw new ArgumentException("different dimesions of points, required the same", nameof(points));

            var dimesion = groupByDimension.First().Key;
            var root = BuildTreeNode(points, 0, dimesion, null);
            return new KDTree< TValue> { Root = root, Count = points.Length };
        }

        /// <summary>
        ///     Добавить новую точку.
        /// </summary>
        /// <param name="point">Точка</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void InsertNode(KDTreePoint< TValue> point)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point));
            if (point.Coordinates.Length != Root.Coordinates.Length)
                throw new ArgumentException("Dimension differs from root", nameof(point));

            var currParent = Root;
            Count++;
            while(true)
            {
                if (currParent.Coordinates[currParent.Dimension] < point.Coordinates[currParent.Dimension])
                {
                    if(currParent.Left == null)
                    {
                        currParent.Left = new KDTreeNode< TValue> { Dimension = (currParent.Dimension + 1) % currParent.Coordinates.Length, IsLeaf = true, Parent = currParent, Coordinates = point.Coordinates, Value = point.Value };
                        return;
                    }
                    else
                    {
                        currParent = currParent.Left;
                    }
                }
                else
                {
                    if(currParent.Right == null)
                    {
                        currParent.Right = new KDTreeNode< TValue> { Dimension = (currParent.Dimension + 1) % currParent.Coordinates.Length, IsLeaf = true, Parent = currParent, Coordinates = point.Coordinates, Value = point.Value };
                        return;
                    }
                    else
                    {
                        currParent = currParent.Right;
                    }
                }
            }
        }

        /// <summary>
        ///     Найти минимальный элемент по измерению.
        /// </summary>
        /// <param name="dimension">Измерение</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public KDTreeNode< TValue> FindMin(int dimension)
        {
            if (IsEmpty)
                return null;
            if (Root.Coordinates.Length <= dimension || dimension < 0)
                throw new ArgumentException("invalid dimension value", nameof(dimension));

            return FindMin(Root, dimension);
        }


        private static KDTreeNode< TValue> FindMin(KDTreeNode< TValue> node, int dimension)
        {
            if(node.Dimension == dimension)
            {
                if (node.Left == null)
                    return node;
                else
                    return FindMin(node.Left, dimension);
            }
            else
            {
                if (node.Left == null && node.Right == null)
                    return node;

                var leftSubTreeMinPoint = node.Left != null ? FindMin(node.Left, dimension) : null;
                var rightSubTreeMinPoint = node.Right != null ? FindMin(node.Right, dimension) : null;

                if (leftSubTreeMinPoint != null && leftSubTreeMinPoint.Coordinates[dimension] < node.Point.Coordinates[dimension] && (rightSubTreeMinPoint == null || leftSubTreeMinPoint.Coordinates[dimension] < rightSubTreeMinPoint.Coordinates[dimension]))
                    return leftSubTreeMinPoint;
                else if (rightSubTreeMinPoint != null && rightSubTreeMinPoint.Coordinates[dimension] < node.Point.Coordinates[dimension] && (leftSubTreeMinPoint == null || rightSubTreeMinPoint.Coordinates[dimension] < leftSubTreeMinPoint.Coordinates[dimension]))
                    return rightSubTreeMinPoint;
                else
                    return node;
                //var nodesToCompare = new List<KDTreePoint< TValue>> { }
            }
        }

        /// <summary>
        ///     Найти максимальный элемент по измерению.
        /// </summary>
        /// <param name="dimension">Измерение</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public KDTreeNode< TValue> FindMax(int dimension)
        {
            if (IsEmpty)
                return null;
            if (Root.Coordinates.Length <= dimension || dimension < 0)
                throw new ArgumentException("invalid dimension value", nameof(dimension));

            return FindMax(Root, dimension);
        }

        //private 

        private static KDTreeNode< TValue> FindMax(KDTreeNode< TValue> node, int dimension)
        {
            if (node.Dimension == dimension)
            {
                if (node.Right == null)
                    return node;
                else
                    return FindMax(node.Right, dimension);
            }
            else
            {
                if (node.Left == null && node.Right == null)
                    return node;

                var leftSubTreeMaxPoint = node.Left != null ? FindMax(node.Left, dimension) : null;
                var rightSubTreeMaxPoint = node.Right != null ? FindMax(node.Right, dimension) : null;

                if (leftSubTreeMaxPoint != null && leftSubTreeMaxPoint.Coordinates[dimension] > node.Point.Coordinates[dimension] && (rightSubTreeMaxPoint == null || leftSubTreeMaxPoint.Coordinates[dimension] > rightSubTreeMaxPoint.Coordinates[dimension]))
                    return leftSubTreeMaxPoint;
                else if (rightSubTreeMaxPoint != null && rightSubTreeMaxPoint.Coordinates[dimension] > node.Point.Coordinates[dimension] && (leftSubTreeMaxPoint == null || rightSubTreeMaxPoint.Coordinates[dimension] > leftSubTreeMaxPoint.Coordinates[dimension]))
                    return rightSubTreeMaxPoint;
                else
                    return node;
                //var nodesToCompare = new List<KDTreePoint< TValue>> { }
            }
        }

        private static double GetDistance(int[] point1, int[] point2) => Math.Sqrt(Enumerable.Range(0, point1.Length).Select(index => (point1[index] - point2[index]) * (point1[index] - point2[index])).Sum());

        private static bool IsEqualCoordinates(int[] point1, int[] point2) => point1.Where((x, i) => x == point2[i]).Count() == point1.Length;

        /// <summary>
        ///     Найти узел по координатам.
        /// </summary>
        /// <param name="coordinates">Координаты</param>
        /// <returns>Узел</returns>
        /// <exception cref="ArgumentException"></exception>
        public KDTreeNode< TValue> FindNode(int[] coordinates)
        {
            if (IsEmpty)
                return null;
            if (DimensionCount != coordinates.Length)
                throw new ArgumentException("dimension conflict");

            var curr = Root;
            while(curr != null)
            {
                if(IsEqualCoordinates(curr.Coordinates, coordinates))
                    return curr;
                else
                {
                    if (coordinates[curr.Dimension] < curr.Coordinates[curr.Dimension])
                        curr = curr.Left;
                    else
                        curr = curr.Right;
                }    

            }
            return null;
        }



        public KDTreeNode< TValue> FindNearestNeighbor(int[] coordinates)
        {
            if (IsEmpty)
                return null;
            if (DimensionCount != coordinates.Length)
                throw new ArgumentException("dimension conflict");

            var curr = Root;
            KDTreeNode<TValue> location = null; 
            while(curr != null)
            {
                if (IsEqualCoordinates(curr.Coordinates, coordinates))
                    return curr;

                location = curr;
                if (curr.Coordinates[curr.Dimension] >= coordinates[curr.Dimension])
                {
                    curr = curr.Left;                    
                }
                else
                {
                    curr = curr.Right;
                }
            }

            var nearestNode = location;
            var minDistance = GetDistance(nearestNode.Coordinates, coordinates);

            var queue = new Queue<KDTreeNode<TValue>>();
            queue.Enqueue(Root);
            while (queue.Any())
            {
                var item = queue.Dequeue();
                var distance = GetDistance(item.Coordinates, coordinates);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    nearestNode = item;
                }
                if(item.Left != null)
                {
                    if (coordinates[item.Dimension] <= item.Coordinates[item.Dimension] || (coordinates[item.Dimension] - item.Coordinates[item.Dimension]) < distance)
                        queue.Enqueue(item.Left);
                }
                if(item.Right != null)
                {
                    if (coordinates[item.Dimension] >= item.Coordinates[item.Dimension] || (item.Coordinates[item.Dimension] - coordinates[item.Dimension]) < distance)
                        queue.Enqueue(item.Right);
                }
            }


            return nearestNode;
        }

        /// <summary>
        ///     Удаление узла.
        /// </summary>
        /// <param name="coordinates">Координаты</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void DeleteNode(int[] coordinates)
        {
            if(IsEmpty) 
                throw new InvalidOperationException("tree is empty. try to add before to call method.s");
            if (DimensionCount != coordinates.Length)
                throw new ArgumentException("dimension conflict");


            var foundNode = FindNode(coordinates) ?? throw new ArgumentException("node not found");

            DeleteNodeInternal(foundNode, foundNode.Dimension);
            Count--;
        }

        private void SwapNodesForDelete(KDTreeNode< TValue> nodeToDelete, KDTreeNode< TValue> node)
        {
            var par1 = nodeToDelete.Parent;
            var dim1 = nodeToDelete.Dimension;
            var left1 = nodeToDelete.Left;
            var right1 = nodeToDelete.Right;
            var isLeaf1 = nodeToDelete.IsLeaf; 

            nodeToDelete.IsLeaf = node.IsLeaf;
            nodeToDelete.Dimension = node.Dimension;
            nodeToDelete.Parent = node.Parent;

            nodeToDelete.Left = node.Left;
            if (node.Left != null)
                node.Left.Parent = nodeToDelete;
            nodeToDelete.Right = node.Right;
            if(node.Right != null)
                node.Right.Parent = nodeToDelete;

            node.IsLeaf = isLeaf1;
            node.Dimension = dim1;
            node.Left = left1;
            node.Right = right1;
            node.Parent = par1;
            if (par1 == null)
                Root = node;
            if (left1 != null)
                node.Left.Parent = node;
            if(right1 != null)
                node.Right.Parent = node;

            var par2 = node.Parent;
        }

        private void DeleteNodeInternal(KDTreeNode< TValue> node, int dimension)
        {
            if (node.Right != null)
            {
                //findmin in right;
                var min = FindMin(node.Right, dimension);
                var minDimension = min.Dimension;
                SwapNodesForDelete(node, min);
                DeleteNodeInternal(node, minDimension);
            }
            else if (node.Left != null)
            {
                //findmax in left;
                var max = FindMax(node.Left, dimension);
                var maxDimension = max.Dimension;
                SwapNodesForDelete(node, max);
                DeleteNodeInternal(node, maxDimension);
            }
            else
            {
                //without children;
                if (node.Parent.Left == node)
                    node.Parent.Left = null;
                else
                    node.Parent.Right = null;

                node.Parent = null;
            }
        }






        private static KDTreeNode< TValue> BuildTreeNode(KDTreePoint< TValue>[] restPoints, int depth, int k, KDTreeNode< TValue> parent)
        {
            if (!restPoints.Any())
                return null;

            var dimension = depth == 0 ? 0 : depth % k;

            if (restPoints.Count() == 1)
                return new KDTreeNode< TValue> { IsLeaf = true, Dimension = dimension, Parent = parent, Coordinates = restPoints[0].Coordinates, Value = restPoints[0].Value };


            var ordered = restPoints.OrderBy(x => x.Coordinates[dimension]).ToArray();

            var median = ordered.Length / 2;
            var node = new KDTreeNode< TValue> { IsLeaf = false, Parent = parent, Coordinates = ordered[median].Coordinates, Dimension = dimension, Value = ordered[median].Value };
            node.Left = BuildTreeNode(ordered.Take(median).ToArray(), depth + 1, k, node);
            node.Right = BuildTreeNode(ordered.Skip(median + 1).ToArray(), depth + 1, k, node);
            return node;
        }
    }
}
