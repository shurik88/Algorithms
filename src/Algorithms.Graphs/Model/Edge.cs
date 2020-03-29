using System;

namespace Algorithms.Graphs.Model
{
    /// <summary>
    /// Ребро
    /// </summary>
    public class Edge: IComparable<Edge>, IComparable
    {
        /// <summary>
        /// Создание экземпляра класса <see cref="Edge"/>
        /// </summary>
        public Edge()
        {
            Weight = 1;
        }
        /// <summary>
        /// Номер
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ориентация
        /// </summary>
        public EdgeDirectionType DirectionType { get; set; }

        /// <summary>
        /// Вес
        /// </summary>
        public int Weight { get; set; }

        /// <inheritdoc/>
        public int CompareTo(Edge other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Weight > other.Weight ? 1 : Weight == other.Weight ? 0 : -1;
        }

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as Edge);
        }
    }
}
