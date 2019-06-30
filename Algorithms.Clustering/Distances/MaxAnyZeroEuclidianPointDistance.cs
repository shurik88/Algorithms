using System;
using System.Linq;

namespace Algorithms.Clustering.Distances
{
    /// <summary>
    /// Кастомное метрика расстояния специфичная для алгоритма кластеризации
    /// Аналогична евклидову расстоянию, но
    /// если одна из координат равно 0.0,то будет подставлена вместо нее максимальное значение из вектора координат
    /// </summary>
    public class MaxAnyZeroEuclidianPointDistance : IPointDistance
    {
        /// <inheritdoc/>
        public double GetDistance(double[] point1Coordinates, double[] point2Coordinates)
        {
            if (point1Coordinates.Length != point2Coordinates.Length)
                throw new ArgumentException($"Not equal vector sizes: size1: {point1Coordinates.Length} size2: {point2Coordinates.Length}");

            var distance = 0.0;
            var max1 = point1Coordinates.Max();
            var max2 = point2Coordinates.Max();
            for (var i = 0; i < point1Coordinates.Length; ++i)
            {
                distance += Math.Abs(point1Coordinates[i]) < 0.000001 ? max1 * max1 : 
                    Math.Abs(point2Coordinates[i]) < 0.0000001 ? max2 * max2 :
                    ((point1Coordinates[i] - point2Coordinates[i]) * (point1Coordinates[i] - point2Coordinates[i]));
            }

            return System.Math.Sqrt(distance);
        }
    }
}
