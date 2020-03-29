using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Algorithms.Graphs.Model.AdjacencyLists
{
    /// <summary>
    /// Вершина смежности
    /// </summary>
    internal class AdjacencyVertex
    {
        /// <summary>
        /// Создание экземпляра класса <see cref="AdjacencyVertex"/>
        /// </summary>
        public AdjacencyVertex()
        {
            InComming = new Collection<AdjacencyEdge>();
            OutComming = new Collection<AdjacencyEdge>();
        }

        /// <summary>
        /// Вершина
        /// </summary>
        public Vertex Vertex { get; set; }

        /// <summary>
        /// Входящие ребра смежности
        /// </summary>
        public ICollection<AdjacencyEdge> InComming { get; set; }

        /// <summary>
        /// Исходящие ребра смежности
        /// </summary>
        public ICollection<AdjacencyEdge> OutComming { get; set; }
    }
}
