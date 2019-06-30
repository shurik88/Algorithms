using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Clustering.Cure
{
    /// <summary>
    /// Кластер CURE
    /// </summary>
    public class CureCluster
    {
        /// <summary>
        /// Создание экземпляра класса <see cref="CureCluster"/>
        /// </summary>
        /// <param name="point">Точка на основе которой будет создан кластер</param>
        public CureCluster(CurePoint point) : this(new List<CurePoint> { point }, new List<double[]> { point.Vector }) { }

        /// <summary>
        /// Создание экземпляра класса <see cref="CureCluster"/>
        /// </summary>
        /// <param name="points">Точк, входящие в кластер</param>
        /// <param name="representativePoints"></param>
        public CureCluster(IEnumerable<CurePoint> points, IEnumerable<double[]> representativePoints)
        {
            Number = Guid.NewGuid();
            RepPoints = representativePoints.ToList();
            Points = points;
        }

        /// <summary>
        /// Номер кластера
        /// </summary>
        public Guid Number { get; private set; }

        /// <summary>
        /// Точки,входящие в кластер
        /// </summary>
        public IEnumerable<CurePoint> Points { get; private set; }

        /// <summary>
        /// Точки, описывающие кластер(его границы)
        /// </summary>
        public IEnumerable<double[]> RepPoints { get; private set; }
    }
}
