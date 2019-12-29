using System;
using System.Collections.Generic;
using System.Linq;
using Algorithms.Clustering.Distances;

namespace Algorithms.Clustering.Cure
{
    /// <summary>
    /// Алгоритм кластеризации CURE
    /// </summary>
    public class CureAlgorithm
    {
        private readonly int _c;
        private readonly double _a;
        private readonly IPointDistance _distanceCalculator;

        private int _averageCount;
        private int _targetClustersCount;

        /// <summary>
        /// Создание экземпляра класса <see cref="CureAlgorithm"/>
        /// </summary>
        /// <param name="pointDistance">Функция расчета расстояния между точками</param>
        /// <param name="c">Количество точек, формирующих контур кластера</param>
        /// <param name="a">Степень сжатия кластера 1 - центроид, 0 - множество всех точек</param>
        public CureAlgorithm(IPointDistance pointDistance, int c = 5, double a = 0.7)
        {
            if (c < 1)
                throw new ArgumentException("At least one point should represent cluster");
            if (a > 1.0 || a < 0.0)
                throw new ArgumentException("Fraction should be between 0.0 and 1.0");
            _c = c;
            _a = a;
            _distanceCalculator = pointDistance ?? throw new ArgumentNullException(nameof(pointDistance));
        }

        /// <summary>
        /// Группировка множества точек пространства на в k-кластеров
        /// </summary>
        /// <param name="points">Точки пространства</param>
        /// <param name="k">Конечное количество кластеров</param>
        /// <returns>Кластеры</returns>
        public IEnumerable<CureCluster> GetClusters(IEnumerable<CurePoint> points, int k = 10)
        {
            _targetClustersCount = k;
            var curePoints = points.ToList();
            _averageCount = curePoints.Sum(x => x.Weight) / _targetClustersCount;
            var clusters = InitClusters(curePoints);
            while (clusters.Count > k)
            {
                //Console.WriteLine(string.Join(" ", clusters.Select(x => x.Value.Cluster.Points.Sum(y => y.Weight))));
                var closestNumber = GetClosesClusterNumber(clusters);

                var u = clusters[closestNumber];
                var v = clusters[u.Closest.Number];

                var merged = MergeClusters(u.Cluster, v.Cluster);

                clusters.Remove(u.Cluster.Number);
                clusters.Remove(v.Cluster.Number);

                var closestCluster = clusters.First().Value.Cluster; //arbitory cluster
                var minDistance = Distance(merged, closestCluster);

                foreach (var pair in clusters)
                {
                    var x = pair.Value;
                    var distanceToMerged = Distance(merged, x.Cluster);
                    if (distanceToMerged < minDistance)
                    {
                        minDistance = distanceToMerged;
                        closestCluster = x.Cluster;
                    }

                    if (x.Closest == u.Cluster || x.Closest == v.Cluster)
                    {
                        //var distanceToMerged = merged.Distance(x.Cluster);
                        if (x.Distance < distanceToMerged)
                            RecalculateClosest(clusters, x.Cluster.Number);
                        else
                        {
                            x.Closest = merged;
                            x.Distance = distanceToMerged;
                        }
                    }
                    else if (distanceToMerged < x.Distance)
                    {
                        x.Closest = merged;
                        x.Distance = distanceToMerged;
                    }


                }

                clusters.Add(merged.Number, new CalculatedCluster { Cluster = merged, Closest = closestCluster, Distance = minDistance });
            }

            return clusters.Select(x => x.Value.Cluster);
        }

        /// <summary>
        /// Расчет растояния между двумя кластерами
        /// </summary>
        /// <param name="cluster1">Калстер№1</param>
        /// <param name="cluster2">Кластер№2</param>
        /// <returns>Расстояние</returns>
        public double Distance(CureCluster cluster1, CureCluster cluster2)
        {
            var repPoints = cluster2.RepPoints.ToList();

            var min = _distanceCalculator.GetDistance(cluster1.RepPoints.First(), repPoints.First());

            foreach (var repPoint1 in cluster1.RepPoints)
            {
                foreach (var repPoint2 in repPoints)
                {
                    var distance = _distanceCalculator.GetDistance(repPoint1, repPoint2);
                    if (distance < min)
                        min = distance;
                }
            }
            var maxWeights = Math.Max(cluster1.Points.Sum(x => x.Weight), cluster2.Points.Sum(x => x.Weight));
            return maxWeights > _averageCount ? (min * maxWeights * _targetClustersCount / _averageCount) : min;
        }


        /// <summary>
        /// Слияние кластеров
        /// </summary>
        /// <param name="cluster1">Кластер№1</param>
        /// <param name="cluster2">Кластер№2</param>
        /// <returns>Итоговый кластер</returns>
        private CureCluster MergeClusters(CureCluster cluster1, CureCluster cluster2)
        {
            var totalPoints = cluster1.Points.Union(cluster2.Points).ToList();

            var meanPoint = new double[totalPoints.First().Vector.Length];

            for (var i = 0; i < meanPoint.Length; ++i)
                meanPoint[i] = totalPoints.Sum(x => x.Vector[i]) / totalPoints.Count * 1.0;

            var ordered = totalPoints
                .Select(x => new
                {
                    Point = x,
                    Distance = _distanceCalculator.GetDistance(meanPoint, x.Vector)
                })
                .OrderBy(x => x.Distance).ToList();

            var rep = (_c > ordered.Count ? ordered : ordered.Take(_c))
                .Select(x => x.Point.Vector.Select((v, i) => v + _a * (meanPoint[i] - v)).ToArray())
                .ToList();

            return new CureCluster(totalPoints, rep);
        }

        /// <summary>
        /// Получение ближайшего кластера
        /// </summary>
        /// <param name="clusters">Кластеры</param>
        /// <returns>Номер ближайшего</returns>
        private static Guid GetClosesClusterNumber(Dictionary<Guid, CalculatedCluster> clusters)
        {
            var closest = clusters.First().Key;
            var minDistance = clusters.First().Value.Distance;

            foreach (var pair in clusters)
            {
                if (minDistance > pair.Value.Distance)
                {
                    closest = pair.Key;
                    minDistance = pair.Value.Distance;
                }
            }

            return closest;
        }

        /// <summary>
        /// Перерасчет метрки кластеров для определенного кластера с номером
        /// </summary>
        /// <param name="clusters">Кластеры</param>
        /// <param name="number">Номер кластера</param>
        private void RecalculateClosest(Dictionary<Guid, CalculatedCluster> clusters, Guid number)
        {
            var minDistance = Double.MaxValue;
            CureCluster closest = null;
            foreach (var pair in clusters)
            {
                if (pair.Key == number)
                    continue;
                var distance = Distance(pair.Value.Cluster, clusters[number].Cluster);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = pair.Value.Cluster;
                }
            }
            clusters[number].Closest = closest;
            clusters[number].Distance = minDistance;
        }

        /// <summary>
        /// Кластер с посчитанными метриками
        /// </summary>
        private class CalculatedCluster
        {
            /// <summary>
            /// Кластер
            /// </summary>
            public CureCluster Cluster { get; set; }

            /// <summary>
            /// Ближайший кластер к данному
            /// </summary>
            public CureCluster Closest { get; set; }

            /// <summary>
            /// Расстояние до ближайшего кластера
            /// </summary>
            public double Distance { get; set; }
        }


        /// <summary>
        /// Инициализация начальных кластеров
        /// </summary>
        /// <param name="points">Исходные точки</param>
        /// <returns>Кластера с дополнительной информацией <seealso cref="CalculatedCluster"/></returns>
        private Dictionary<Guid, CalculatedCluster> InitClusters(IEnumerable<CurePoint> points)
        {
            var clusters = points.Select(x => new CureCluster(x)).ToList();
            var dict = new Dictionary<Guid, CalculatedCluster>();

            foreach (var cluster1 in clusters)
            {
                var minDistance = Double.MaxValue;
                CureCluster closest = null;
                foreach (var cluster2 in clusters)
                {
                    if (cluster1 == cluster2)
                        continue;
                    var distance = Distance(cluster1, cluster2);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closest = cluster2;
                    }
                }
                dict.Add(cluster1.Number, new CalculatedCluster { Closest = closest, Cluster = cluster1, Distance = minDistance });
            }

            return dict;
        }
    }
}
