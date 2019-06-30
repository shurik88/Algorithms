namespace Algorithms.Clustering.Cure
{
    /// <summary>
    /// Точка кластера CURE
    /// </summary>
    public class CurePoint
    {
        /// <summary>
        /// Идентификатор точки
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Вес точки
        /// </summary>
        public int Weight { get; set; } = 1;

        /// <summary>
        /// Вектор координат точки
        /// </summary>
        public double[] Vector { get; set; }
    }
}
