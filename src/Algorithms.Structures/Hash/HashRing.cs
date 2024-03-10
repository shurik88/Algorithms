using Algorithms.Structures.Trees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Algorithms.Structures.Hash
{
    /// <summary>
    ///     Согласованное хеширование.
    ///     Хеш-кольцо
    /// </summary>
    /// <typeparam name="TNode">Тип узла</typeparam>
    /// <typeparam name="TResource">Тип ресурса</typeparam>
    public class HashRing<TNode, TResource>
    {
        private readonly LinkedList<HashRingNode<TNode, TResource>> _nodes = new LinkedList<HashRingNode<TNode, TResource>>();

        private readonly IHashFunc _hashFunc;
        public HashRing(IHashFunc hashFunc)
        {
            _hashFunc= hashFunc;
        }

        /// <summary>
        ///     Узлы.
        /// </summary>
        public IEnumerable<HashRingNode<TNode, TResource>> OrderedList => _nodes;

        /// <summary>
        ///     Ресурсы.
        /// </summary>
        public IEnumerable<int[]> Resources => OrderedList.Select(x => x.Resources.Select(y => y.Key).ToArray()).ToList();

        internal int GetDistance(int from, int to)
        {
            if (from == to)
                return 0;
            if (from < to)
                return to - from;
            else
                return _hashFunc.Max - from + to;
        }

        internal bool IsLegalHashValue(int value) => value >= 0 && value < _hashFunc.Max;

        private int GetNodeHash(HashRingNode<TNode, TResource> node) => _hashFunc.GetHash(node.Key);

        private LinkedListNode<HashRingNode<TNode, TResource>> LookupNode(int key)
        {
            if (!IsLegalHashValue(key))
                throw new ArgumentException("invalid key",nameof(key));

            if (_nodes.Count == 0)
                return null;

            var curr = _nodes.First;
            var hashValue = _hashFunc.GetHash(key);
            while(curr != null)
            {
                var currValue = GetNodeHash(curr.Value);
                if (currValue > hashValue)
                    return curr;

                curr = curr.Next;
            }

            return _nodes.First;
        }
        private void MoveResources(HashRingNode<TNode, TResource> fromNode, HashRingNode<TNode, TResource> toNode)
        {
            var toHash = GetNodeHash(toNode);
            var resourcesToMove = fromNode.Resources.Where(x => _hashFunc.GetHash(x.Key) < toHash).ToList();
            foreach(var r  in resourcesToMove)
            {
                toNode.AddResource(r);
                fromNode.RemoveResource(r);
            }
        }

        /// <summary>
        ///     Добавление узла.
        /// </summary>
        /// <param name="node">Узел</param>
        public void AddNode(HashRingNode<TNode, TResource> node)
        {
            var nearestNode = LookupNode(node.Key);
            if(nearestNode == null)
            {
                _nodes.AddFirst(node);
                return;
            }

            if (_hashFunc.GetHash(nearestNode.Value.Key) > _hashFunc.GetHash(node.Key))
                _nodes.AddBefore(nearestNode, node);
            else
                _nodes.AddLast(node);

            var resources = nearestNode.Value.Resources.ToList();
            foreach(var resource in resources)
            {
                var nearestNodeForResource = LookupNode(resource.Key);
                if(nearestNodeForResource.Value == node)
                {
                    nearestNode.Value.RemoveResource(resource);
                    node.AddResource(resource);
                }
            }
            
            //MoveResources(nearestNode.Value, node);
            
        }

        /// <summary>
        ///     Удаление узла.
        /// </summary>
        /// <param name="node">Узел</param>
        /// <exception cref="InvalidOperationException">Узел не найден</exception>
        public void RemoveNode(HashRingNode<TNode, TResource> node)
        {
            if (!_nodes.Any(x => x == node))
                throw new InvalidOperationException("Node not exists. Please call add before.");


            if (_nodes.Count == 1)
            {
                _nodes.RemoveFirst();
                return;
            }

            var nearestNode = LookupNode(node.Key);
            _nodes.Remove(node);
            if (!_nodes.Any())
            {
                return;
            }
            var resourcesList = node.Resources.ToList();
            foreach(var resource in resourcesList)
            {
                node.RemoveResource(resource);
                nearestNode.Value.AddResource(resource);
            }
        }


        /// <summary>
        ///     Добавление ресурса.
        /// </summary>
        /// <param name="resource">Ресурс</param>
        /// <exception cref="InvalidOperationException">Нет узлов.</exception>
        public void AddResource(HashRingResource<TResource> resource)
        {
            if(!_nodes.Any())
                throw new InvalidOperationException("Nodes list is empty. Please call AddNode before");
            
            var node = LookupNode(resource.Key);               

            node.Value.AddResource(resource);
        }

        /// <summary>
        ///     Удаление ресурса.
        /// </summary>
        /// <param name="resource">Ресурс</param>
        /// <exception cref="InvalidOperationException">Нет узлов</exception>
        public void RemoveResource(HashRingResource<TResource> resource)
        {
            if (!_nodes.Any())
                throw new InvalidOperationException("Nodes list is empty. Please call AddNode before");

            var node = LookupNode(resource.Key);

            node.Value.RemoveResource(resource);
        }
    }
}
