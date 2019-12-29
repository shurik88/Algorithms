using System;

namespace Algorithms.Clustering.Distances
{
    /// <summary>
    /// Манхетоновское расстояние
    /// </summary>
    public class ManhattanPointDistance : IPointDistance
    {
        /// <inheritdoc/>
        public double GetDistance(double[] point1Coordinates, double[] point2Coordinates)
        {
            if (point1Coordinates.Length != point2Coordinates.Length)
                throw new ArgumentException($"Not equal vector sizes: size1: {point1Coordinates.Length} size2: {point2Coordinates.Length}");

            var distance = 0.0;
            for(var i = 0; i < point1Coordinates.Length; ++i)
                distance += System.Math.Abs(point1Coordinates[i] - point2Coordinates[i]);

            return distance;
        }
    }
}
