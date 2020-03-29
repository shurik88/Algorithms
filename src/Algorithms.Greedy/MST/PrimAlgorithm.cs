using Algorithms.Graphs.Model;
using Algorithms.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Greedy.MST
{
    /// <summary>
    /// Алгоритм Прима для поиска минимального оставеного дерева
    /// </summary>
    public class PrimAlgorithm : IMinimalSpanTreeSearch
    {
        /// <inheritdoc/>
        public IEnumerable<Edge> GetMST(IGraphDefinition graphDefinition)
        {
            if (graphDefinition == null)
                throw new ArgumentNullException(nameof(graphDefinition));

            var allVertexes = graphDefinition.Vertices.ToList();
            if (allVertexes.Count <= 1)
                throw new ArgumentException("Minimal vertexes count is 2", nameof(graphDefinition));

            var firstVertex = allVertexes[0];
            var notDistributedVertexes = new List<VertextExt>();
            for(var i = 1; i < allVertexes.Count; ++i)
            {
                var edges = graphDefinition.GetEdges(firstVertex, allVertexes[i]).ToList();
                notDistributedVertexes.Add(new VertextExt { Vertex = allVertexes[i], Winner = edges.Any() ? edges.OrderBy(x => x.Weight).Min() : null });
            }
            var heap = new VertextHeap(notDistributedVertexes);
            while(!heap.IsEmpty)
            {
                var heapElement = heap.Extract();
                var closestVertex = heapElement.Value;
                if (closestVertex.Winner == null)
                    throw new InvalidOperationException("Graph is not connected");

                for (var i = 0; i < heap.Count; ++i)
                {
                    var vertex = heap.Heap[i].Value.Vertex;
                    if(graphDefinition.EdgeExists(closestVertex.Vertex, vertex))
                    {
                        var closestEdge = graphDefinition.GetEdges(closestVertex.Vertex, vertex).OrderBy(x => x.Weight).First();
                        if (heap.Heap[i].Value.Winner == null || heap.Heap[i].Key > closestEdge.Weight)
                            heap.EditElementKey(vertex, closestEdge);
                    }
                }

                yield return closestVertex.Winner;

            }
        }

        private class VertextExt
        {
            public Vertex Vertex { get; set; }

            public Edge Winner { get; set; }
        }

        private class VertextHeap
        {
            public readonly ComparableElement<int, VertextExt>[] Heap;

            private bool IsChildCorrect(int childIndex) => Heap[GetParentIndex(childIndex)].Key.CompareTo(Heap[childIndex].Key) <= 0;

            private int SelectElementSubstituation(int firstIndex, int secondIndex) =>
                    Heap[firstIndex].Key.CompareTo(Heap[secondIndex].Key) <= 0 ? firstIndex : secondIndex;

            private void DecreaseKey(int index, Edge edge)
            {
                var newValue = edge.Weight;
                if (Heap[index].Key.CompareTo(newValue) < 0)
                    throw new InvalidOperationException("Min heap does not support increase key.");

                Heap[index].Key = newValue;
                Heap[index].Value.Winner = edge;
                while (index != 0 && Heap[GetParentIndex(index)].Key.CompareTo(Heap[index].Key) > 0)
                {
                    Swap(index, GetParentIndex(index));
                    index = GetParentIndex(index);
                }
            }


            private readonly IDictionary<int, int> _vertexesIndexes;



            /// <summary>
            /// Создание экземпляра класса <see cref="VertextHeap"/>
            /// </summary>
            /// <param name="vertexes">Веришины графа</param>
            public VertextHeap(IEnumerable<VertextExt> vertexes)
            {
                if (!vertexes.Any())
                    throw new ArgumentException($"Vertexes list is empty", nameof(vertexes));

                var list = vertexes.ToList();
                Heap = new ComparableElement<int, VertextExt>[list.Count];
                _vertexesIndexes = list.ToDictionary(x => x.Vertex.Id, x => 0);
                foreach (var vertex in list)
                    Insert(new ComparableElement<int, VertextExt> { Value = vertex, Key = vertex.Winner != null ? vertex.Winner.Weight : int.MaxValue });
            }

            /// <summary>
            /// Количество элементов
            /// </summary>
            public int Count { get; private set; } = 0;

            /// <summary>
            /// Список элементов
            /// </summary>
            public IEnumerable<ComparableElement<int, VertextExt>> Elements => Heap.Take(Count);

            /// <summary>
            /// Является ли куча пустой
            /// </summary>
            public bool IsEmpty => Count == 0;

            /// <summary>
            /// Получить корневой элемент
            /// </summary>
            public ComparableElement<int, VertextExt> First => !IsEmpty
                ? Heap[0]
                : throw new InvalidOperationException("Heap is empty. Try to insert first");

            /// <summary>
            /// Вставка элемента в кучу
            /// </summary>
            /// <param name="element">Элемента</param>
            private void Insert(ComparableElement<int, VertextExt> element)
            {
                if (Count == Heap.Length)
                    throw new InvalidOperationException("Too many elements in heap. Try to extract elment before");

                Heap[Count] = element;
                _vertexesIndexes[element.Value.Vertex.Id] = Count;
                HeapifyUp();
                Count++;
            }

            /// <summary>
            /// Изменение ключа элемента
            /// </summary>
            /// <param name="vertex">Вершина</param>
            /// <param name="winner">Новое значение ключа(ребро)</param>
            public void EditElementKey(Vertex vertex, Edge winner)
            {
                if (!_vertexesIndexes.ContainsKey(vertex.Id))
                    throw new ArgumentException($"Vettext not found with id:{vertex.Id}", nameof(vertex));               

                DecreaseKey(_vertexesIndexes[vertex.Id], winner);
            }

            private void HeapifyBottom()
            {
                var currIndex = 0;
                while (GetLeftChildIndex(currIndex) < Count)
                {
                    var leftIndex = GetLeftChildIndex(currIndex);
                    var rightIndex = GetRightChildIndex(currIndex);
                    var bestIndex = rightIndex < Count ? SelectElementSubstituation(leftIndex, rightIndex) : leftIndex;
                    var bestWithParentIndex = SelectElementSubstituation(currIndex, bestIndex);
                    if (bestWithParentIndex == currIndex)
                        break;
                    Swap(currIndex, bestWithParentIndex);
                    currIndex = bestWithParentIndex;
                }
            }

            private void HeapifyUp()
            {
                var currIndex = Count;
                while (currIndex != 0)
                {
                    if (currIndex == 0 || IsChildCorrect(currIndex))
                        return;
                    var parentIndex = GetParentIndex(currIndex);
                    Swap(currIndex, parentIndex);
                    currIndex = parentIndex;
                }
            }

            private void Swap(int first, int second)
            {
                if (first == second)
                    return;

                var temp = Heap[first];
                Heap[first] = Heap[second];
                Heap[second] = temp;

                var firstVertex = Heap[first].Value.Vertex;
                var secondVertex = Heap[second].Value.Vertex;

                _vertexesIndexes[firstVertex.Id] = first;
                _vertexesIndexes[secondVertex.Id] = second;
            }


            /// <summary>
            /// Извлечь первый элемент
            /// </summary>
            /// <returns>Элемент в вершине кучи</returns>
            public ComparableElement<int, VertextExt> Extract()
            {
                if (IsEmpty)
                    throw new InvalidOperationException("Heap is empty. Try to insert first");

                var extracred = First;
                Count--;
                if (!IsEmpty)
                {
                    Heap[0] = Heap[Count];
                    _vertexesIndexes[Heap[0].Value.Vertex.Id] = 0;
                    Heap[Count] = null;
                    HeapifyBottom();
                }
                return extracred;
            }

            private static int GetLeftChildIndex(int parentIndex) => 2 * parentIndex + 1;
            private static int GetRightChildIndex(int parentIndex) => 2 * parentIndex + 2;

            private static int GetParentIndex(int childIndex) => (childIndex - 1) / 2;
        }
    }
}
