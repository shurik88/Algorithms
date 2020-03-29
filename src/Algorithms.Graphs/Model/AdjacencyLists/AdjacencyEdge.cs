namespace Algorithms.Graphs.Model.AdjacencyLists
{
    /// <summary>
    /// Ребро смежности
    /// </summary>
    internal class AdjacencyEdge
    {
        /// <summary>
        /// Ребро
        /// </summary>
        public Edge Edge { get; set; }

        /// <summary>
        /// Куда входит ребро
        /// </summary>
        public AdjacencyVertex To { get; set; }

        /// <summary>
        /// Откуда исходит ребро
        /// </summary>
        public AdjacencyVertex From { get; set; }
    }
}
