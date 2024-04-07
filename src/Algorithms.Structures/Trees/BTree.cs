using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace Algorithms.Structures.Trees
{
    public class BTree<TKey, TValue>
        where TKey: IComparable<TKey>
        where TValue: class
    {
        private readonly int _dSize;
        private BTreeNode root;

        public int Count { get; private set; }
        public BTree(int dSize)
        {
            if (dSize <= 0 || dSize % 2 != 0)
                throw new ArgumentException("d should be even and greater than 0", nameof(dSize));

            _dSize = dSize;
        }

        /// <summary>
        ///     Найти элемент в дереве. 
        /// </summary>
        /// <param name="key">Ключ элемента</param>
        /// <returns>Значение по ключу, если найдено, иначе null</returns>
        /// <exception cref="InvalidOperationException">Если дерево пустое</exception>
        public TValue Search(TKey key)
        {
            if (root == null || root.First == null)
                throw new InvalidOperationException($"Tree is empty. Please add item before, calling {nameof(Add)}.");

            var item = SearchItem(root, key);

            return item?.Value;
        }

        private BTreeItem SearchItem(BTreeNode node, TKey key)
        {
            if (node == null || node.First == null)
                return null;

            var curr = node.First;
            BTreeItem prev = null;
            while(curr != null)
            {
                var compareRes = curr.CompareTo(key);
                if (compareRes == 0)
                    return curr;
                else if (compareRes > 0)
                {
                    return SearchItem(curr.ChildrenLeft, key);
                }
                prev = curr;
                curr = curr.Right;     
            }
            return SearchItem(prev.ChildrenRight, key);
        }
        


        private (BTreeItem Left, BTreeItem right) SearchToInsert(BTreeNode node, TKey key)
        {
            var curr = node.First;
            BTreeItem prev = null;
            while (curr != null)
            {
                var compareRes = curr.CompareTo(key);
                if (compareRes == 0)
                    throw new InvalidOperationException("Item already added");
                else if (compareRes > 0)
                {
                    if (curr.ChildrenLeft == null)
                        return (prev, curr);
                    return SearchToInsert(curr.ChildrenLeft, key);
                }
                prev = curr;
                curr = curr.Right;
            }
            if (prev.ChildrenRight == null)
                return (prev, null);
            return SearchToInsert(prev.ChildrenRight, key);
        }

        public void Add(TKey key, TValue value)
        {
            if(root == null)
            {
                root = new BTreeNode { 
                    //Count = 1, 
                    Tree = this, 
                    IsLeaf = true 
                };
                root.First = new BTreeItem { Container = root, Key = key, Value = value } ;
            }
            else
            {
                (var left, var right) = SearchToInsert(root, key);
                var container = left?.Container ?? right.Container;
                var newItem = new BTreeItem { Key = key, Value = value };

                newItem.Container = container;
                newItem.Left = left;
                if(left == null)
                    container.First = newItem;
                newItem.Right = right;
                if (left != null)
                    left.Right = newItem;
                if (right != null)
                    right.Left = newItem;

                //container.Count++;

                NormalizeContainer(container);
            }
            Count++;
        }

        private void ResetContainerForList(BTreeItem item)
        {
            var container = item.Container;
            var curr = item;
            while (curr != null)
            {
                curr.Container = container;
                curr = curr.Right;
            }
        }

        private void NormalizeContainer(BTreeNode container)
        {
            if (container.Count <= 2 * _dSize)
                return;

            var middle = container.GetMiddle();
            var left = middle.Left;
            var right = middle.Right;
            var parentLeft = container.ParentLeft;
            var parentRight = container.ParentRight;

            //split container: left is curr container with left part, right is new container with right part, middle up  to parent container
            var newCount = container.Count / 2;
            middle.ChildrenLeft = container;
            left.Right = null;
            //left.Container.Count = newCount;
            right.Left = null;
            var newRightContainer = new BTreeNode { 
                IsLeaf = container.IsLeaf, 
                First = right, Tree = this, 
                //Count = newCount, 
                ParentLeft = middle 
            };
            middle.ChildrenRight = newRightContainer;
            right.Container = newRightContainer;
            ResetContainerForList(right);
            container.ParentRight = middle;
            //right.Container = newRightContainer;

            if (parentLeft == null && parentRight == null) //when lead is root node 
            {
                //create new root
                var newRoot = new BTreeNode { 
                    Tree = this, 
                    First = middle, 
                    //Count = 1 
                };
                root = newRoot;
                middle.Left = null;
                middle.Right = null;
                middle.Container = newRoot;
            }
            else
            {
                var newContainerForMiddle = parentLeft.Container; //parentRight; 
                var newLeft = parentLeft;
                var newRight = parentRight;
                if(parentLeft != null)
                    parentLeft.Right = middle;
                if(parentRight != null)
                    parentRight.Left = middle;
                middle.Left = parentLeft;
                middle.Right = parentRight;
                middle.Container = newContainerForMiddle;
                //middle.Container.Count++;

                NormalizeContainer(newContainerForMiddle);
            }
        }

        public void Remove(TKey key)
        {
            if (root == null || root.First == null)
                throw new InvalidOperationException($"Tree is empty. Please add item before, calling {nameof(Add)}.");

            var item = SearchItem(root, key);
            if(item == null)
                throw new ArgumentException($"Item not found.");

            if (Count == 1)
            {
                // if delete last element  another condition if containerFromDelete == root && containerFromDelete.IsLeaf && containerFromDelete.Count == 0
                item.ClearLinks();
                item = null;
                root = null;
                return;
            }

            var itemRight = item.Right;
            var itemLeft = item.Left;
            var itemLeftContainer = item.ChildrenLeft;
            var itemRightContainer = item.ChildrenRight;

            var containerFromDelete = item.Container;
            if (item.Container.IsLeaf)
            {
                //delete links
                //if first elem change first item of node
                if (itemLeft == null && itemRight != null)
                    item.Container.First = itemRight;

                if(itemRight != null)
                    itemRight.Left = itemLeft;
                if (itemLeft != null)
                    itemLeft.Right = itemRight;

            }
            else
            {
                var elemToSubstitute = item.GetPredeccesorForDelete();
                containerFromDelete = elemToSubstitute.Container;
                var elemToSubstituteRight = elemToSubstitute.Right;
                if (elemToSubstituteRight != null)
                {
                    elemToSubstituteRight.Container.First = elemToSubstituteRight;
                    elemToSubstituteRight.Left = null;

                }
                elemToSubstitute.Right = null;
                //delete item and mode elemToSubstitute at its position
                if (itemRight != null)
                {
                    itemRight.Left = elemToSubstitute;
                    elemToSubstitute.Right = itemRight;
                }
                if(itemLeft != null)
                {
                    itemLeft.Right = elemToSubstitute;
                    elemToSubstitute.Left = itemLeft;
                }
                if (item.Container.First == item)
                    item.Container.First = elemToSubstitute;
                if(itemLeftContainer != null)
                {
                    itemLeftContainer.ParentRight = elemToSubstitute;
                    elemToSubstitute.ChildrenLeft = itemLeftContainer;
                }
                if(itemRightContainer != null)
                {
                    itemRightContainer.ParentLeft = elemToSubstitute;
                    elemToSubstitute.ChildrenRight = itemRightContainer;
                }
                elemToSubstitute.Container = item.Container;
            }
            item.ClearLinks();
            item = null;
            Count--;



            if (containerFromDelete.Count < _dSize)
            {
                //BTreeNode donorContainer = null;
                //try to get items from neighbours
                var rightNeighbour = containerFromDelete.RightNeighbour;
                var leftNeighbour = containerFromDelete.LeftNeighbour;
                var rightNeighbourCount = rightNeighbour?.Count;
                var leftNeighbourCount = leftNeighbour?.Count;
                //var donorContainer = rightNeighbourCount > _dSize && rightNeighbourCount >= leftNeighbourCount ? rightNeighbour
                //    : leftNeighbourCount > _dSize && leftNeighbourCount > rightNeighbourCount ? leftNeighbour
                //    : null;
                if (rightNeighbourCount > _dSize && rightNeighbourCount >= leftNeighbourCount)
                {
                    // shift left of right neighbour to parent left and parent left to item(delete of substitute)
                    var firstRight = rightNeighbour.First;
                    firstRight.Right.Left = null;
                    var elemToShift = firstRight.Container.ParentLeft;
                    var leftOfElemToShift = elemToShift.Left;
                    if (leftOfElemToShift != null)
                        leftOfElemToShift.Right = firstRight;
                    else
                        elemToShift.Container.First = firstRight;
                    var rightOfElemToShift = elemToShift.Right;
                    if (rightOfElemToShift != null)
                        rightOfElemToShift.Left = firstRight;

                    firstRight.ClearLinks();
                    firstRight.Left = leftOfElemToShift;
                    firstRight.Right = rightOfElemToShift;
                    firstRight.Container = elemToShift.Container;
                    firstRight.ChildrenLeft = containerFromDelete;
                    firstRight.ChildrenRight = rightNeighbour;
                    containerFromDelete.ParentRight = firstRight;
                    rightNeighbour.ParentLeft = firstRight;

                    elemToShift.ClearLinks();
                    elemToShift.Container = containerFromDelete;
                    var currFirst = containerFromDelete.First;
                    currFirst.Left = elemToShift;
                    elemToShift.Right = currFirst;

                    containerFromDelete.First = elemToShift;

                    //donorContainer = rightNeighbour;
                }
                else if (leftNeighbourCount > _dSize && leftNeighbourCount > rightNeighbourCount)
                {
                    //donorContainer = leftNeighbour;

                    // shift right of left neighbour to parent right and parent right to item(delete of substitute)
                    var lastLeft = leftNeighbour.Last;
                    lastLeft.Left.Right = null;
                    var elemToShift = lastLeft.Container.ParentRight;
                    var leftOfElemToShift = elemToShift.Left;
                    if (leftOfElemToShift != null)
                        leftOfElemToShift.Right = lastLeft;
                    else
                        elemToShift.Container.First = lastLeft;

                    var rightOfElemToShift = elemToShift.Right;
                    if (rightOfElemToShift != null)
                        rightOfElemToShift.Left = lastLeft;
                    lastLeft.ClearLinks();
                    lastLeft.Left = leftOfElemToShift;
                    lastLeft.Right = rightOfElemToShift;
                    lastLeft.Container = elemToShift.Container;
                    lastLeft.ChildrenLeft = leftNeighbour;
                    lastLeft.ChildrenRight = containerFromDelete;
                    containerFromDelete.ParentLeft = lastLeft;
                    leftNeighbour.ParentRight = lastLeft;

                    elemToShift.ClearLinks();
                    elemToShift.Container = containerFromDelete;
                    var currFirst = containerFromDelete.First;
                    currFirst.Left = elemToShift;
                    elemToShift.Right = currFirst;
                    containerFromDelete.First = elemToShift;

                }
                else
                    ConcatenateNodes(containerFromDelete);


            }
        }


        private void ConcatenateNodes(BTreeNode smallNode)
        {
            if (smallNode.Count >= _dSize)
                return;
            if (smallNode == root)
                return;

            BTreeNode nodeDonor = null;
            //try left
            if(smallNode.ParentLeft != null && smallNode.ParentLeft.ChildrenRight != null)
            {
                //nodeDonor = smallNode.ParentLeft.Container;
                //var last = smallNode.ParentLeft.ChildrenRight.Last;
                //var donorItem = smallNode.ParentLeft;
                //if(donorItem.Left != null)
                //    donorItem.Left.Right = donorItem.Right;
                //if (donorItem.Right != null)
                //    donorItem.Right.Left = donorItem.Left;
                //if (nodeDonor.First == donorItem)
                //    nodeDonor.First = donorItem.Left;

                //last.Right = smallNode.ParentLeft;
            }
            else if (smallNode.ParentRight != null && smallNode.ParentRight.ChildrenLeft != null)
            {
                nodeDonor = smallNode.ParentRight.Container;
            }
            else
            {
                throw new InvalidOperationException("Invalid concatenate call. Nodes concatenation is impossible.");
            }
            ConcatenateNodes(nodeDonor);


        }

        public int Depth => GetDepthInternal(root);

        private int GetDepthInternal(BTreeNode node)
        {
            if (node == null)
                return 0;

            var max = 0;
            var curr = node.First;
            while(curr != null)
            {
                var depthLeft = GetDepthInternal(curr.ChildrenLeft);
                if(depthLeft > max)
                    max = depthLeft;
                var depthRight = GetDepthInternal(curr.ChildrenRight);
                if(depthRight > max)
                    max = depthRight;

                curr = curr.Right;
                //max = Math.Max()
            }
            return max + 1;
        }


        public IEnumerable<IEnumerable<TKey>> KeysInNodes
        {
            get
            {
                if (root == null)
                    return Array.Empty<IEnumerable<TKey>>();

                var nodesWithKeys = new List<List<TKey>>();
                var queue = new Queue<BTreeNode>(new[] { root });
                while (queue.Any())
                {
                    var node = queue.Dequeue();
                    var keys = new List<TKey>();
                    var curr = node.First;
                    while (curr != null)
                    {
                        keys.Add(curr.Key);

                        if (curr.ChildrenLeft != null)
                            queue.Enqueue(curr.ChildrenLeft);
                        if (curr.Right == null && curr.ChildrenRight != null)
                            queue.Enqueue(curr.ChildrenRight);

                        curr = curr.Right;
                    }
                    nodesWithKeys.Add(keys);

                }
                return nodesWithKeys;
            }
        }

        //public void Remove(TKey key)
        //{
        //    throw new NotImplementedException();
        //}

        private class BTreeNode
        {
            //public List<BTreeItem> Items { get; set; }

            public BTree<TKey, TValue> Tree { get; set; }

            public BTreeItem First { get; set; }

            public BTreeItem ParentLeft { get; set; }

            public BTreeItem ParentRight { get; set; }

            public bool IsLeaf { get; set; }

            //public int Count { get; set; }

            public BTreeItem GetMiddle()
            {
                var number = Count / 2;
                var middle = First;
                for (var i = 0; i < number; ++i)
                    middle = middle.Right;

                return middle;
            }

            public int Count
            {
                get
                {
                    var count = 0;
                    var curr = First;
                    while (curr != null)
                    {
                        count++;
                        curr = curr.Right;
                    }
                    return count;
                }
            }

            public BTreeItem Last
            {
                get
                {
                    var curr = First;
                    while(true)
                    {
                        if (curr.Right == null)
                            return curr;
                        curr= curr.Right;
                    }
                }
            }

            public BTreeNode LeftNeighbour => ParentLeft?.ChildrenLeft;

            public BTreeNode RightNeighbour => ParentRight?.ChildrenRight;
        }

        private class BTreeItem
        {
            public int CompareTo(TKey key) => Key.CompareTo(key);
            public TKey Key { get; set; }

            public TValue Value { get; set; }

            public BTreeItem Left { get; set; }

            public BTreeItem Right { get; set; }

            //public BTreeItem Parent { get; set; }

            public BTreeNode Container { get; set; }

            public BTreeNode ChildrenLeft { get; set; }

            public BTreeNode ChildrenRight { get; set; }


            public void ClearLinks()
            {
                //delete;
                Right = null;
                Left = null;
                Container = null;
                ChildrenRight = null;
                ChildrenLeft = null;
            }

            public BTreeItem GetPredeccesorForDelete()
            {
                if (Container.IsLeaf)
                    throw new InvalidOperationException("Impossbile to get predeccesor for leaf.");

                var rightItem = ChildrenRight.First;
                while (rightItem.ChildrenLeft != null)
                    rightItem = rightItem.ChildrenLeft.First;
                return rightItem;

            }
        }
    }
}
