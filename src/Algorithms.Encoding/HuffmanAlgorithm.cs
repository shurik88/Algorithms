using Algorithms.Structures.Heaps;
using Algorithms.Structures.Trees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Encoding
{
    /// <summary>
    ///     Алгоритм кодирования Хаффмана
    /// </summary>
    public class HuffmanAlgorithm
    {
        public IDictionary<char, string> Encode(IDictionary<char, int> alphabetFrequences)
        {
            if (alphabetFrequences == null)
                throw new ArgumentNullException(nameof(alphabetFrequences));
            if (alphabetFrequences.Count <= 2)
                throw new ArgumentException("Alphabet imin size is 2", nameof(alphabetFrequences));

            //init each symbol as tree in forest;
            var forest = new Forest(alphabetFrequences.Count);
            foreach (var pair in alphabetFrequences)
                forest.PushTree(new Node<int, char> { Key = pair.Value, Value = pair.Key });

            while(forest.Count > 1)
            {
                var tree1 = forest.PopMin();
                var tree2 = forest.PopMin();
                var tree = forest.Concat(tree1, tree2);
                forest.PushTree(tree);
            }

            var node = forest.First();

            return GetCodes(node);
        }

        private class VisitedNode
        {
            public IEnumerable<bool> CumulativeCode { get; set; }

            public Node<int, char> CurrentNode { get; set; }
        }

        private IDictionary<char, string> GetCodes(Node<int, char> node)
        {
            var codes = new Dictionary<char, string>();
            var stack = new Stack<VisitedNode>();
            stack.Push(new VisitedNode { CumulativeCode = new List<bool>(), CurrentNode = node });
            while(stack.Any())
            {
                var vertex = stack.Pop();
                if (vertex.CurrentNode.Left != null)
                {
                    var childCode = vertex.CumulativeCode.ToList();
                    childCode.Add(false);
                    stack.Push(new VisitedNode { CumulativeCode = childCode, CurrentNode = vertex.CurrentNode.Left });
                }
                if (vertex.CurrentNode.Right != null)
                {
                    var childCode = vertex.CumulativeCode.ToList();
                    childCode.Add(true);
                    stack.Push(new VisitedNode { CumulativeCode = childCode, CurrentNode = vertex.CurrentNode.Right });
                }
                if (vertex.CurrentNode.Left == null && vertex.CurrentNode.Right == null)
                    codes.Add(vertex.CurrentNode.Value, string.Join("", vertex.CumulativeCode.Select(x => x ? "1" : "0").ToArray()));
            }

            return codes;
        }

        private class Forest
        {
            private readonly Heap<int, Node<int, char>> _heap;
            public Forest(int treesCount) 
            {
                _heap = new Heap<int, Node<int, char>>(HeapType.Min, treesCount);
            }

            public int Count => _heap.Count;

            public Node<int, char> First() => _heap.First.Value;

            public void PushTree(Node<int, char> node)
            {
                _heap.Insert(new Structures.ComparableElement<int, Node<int, char>> { Key = node.Key, Value = node });
            }

            public Node<int, char> PopMin()
            {
                if (_heap.Count == 0)
                    throw new InvalidOperationException($"Forest is empty. Try to call {nameof(PushTree)} before.");

                return _heap.Extract().Value;
            }

            public Node<int, char> Concat(Node<int, char> first, Node<int, char> second)
            {
                var root = new Node<int, char> { Key = first.Key + second.Key, Value = default };
                first.Parent = root;
                second.Parent = root;
                root.Right = first.Key <= second.Key ? first : second;
                root.Left = first.Key <= second.Key ? second : first;
                return root;
            }

           
        }
    }
}
