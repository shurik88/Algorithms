namespace Algorithms.Clustering.Distances
{
    /// <summary>
    /// Интерфейс получения растояния между точками
    /// </summary>
    public interface IPointDistance
    {
        /// <summary>
        ///  Расчет расстояния между точками
        /// </summary>
        /// <param name="point1Coordinates">Координаты первой точки</param>
        /// <param name="point2Coordinates">Координаты второй точки</param>
        /// <returns>расстояние</returns>
        double GetDistance(double[] point1Coordinates, double[] point2Coordinates);
    }
}
