using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Clustering.Clope
{
    /// <summary>
    /// Алгоритм кластеризации CLOPE
    /// </summary>
    /// <remarks>
    /// https://basegroup.ru/community/articles/clope
    /// </remarks>
    public class ClopeAlgorithm
    {
        private readonly double _r;

        /// <summary>
        /// Создание экземпляра класса <see cref="ClopeAlgorithm"/>
        /// </summary>
        /// <param name="r">коэффициентом отталкивания (repulsion)
        /// Чем больше r, тем ниже уровень сходства и тем больше кластеров будет сгенерировано
        /// </param>
        public ClopeAlgorithm(double r = 2.0)
        {
            if (r < 1)
                throw new ArgumentException("Argument r is less than 1.0");
            _r = r;
        }

        /// <summary>
        /// Кластеризация
        /// </summary>
        /// <param name="transactions">Транзакции</param>
        /// <returns>Список кластеров</returns>
        public IEnumerable<ClopeCluster> GetClusters(IEnumerable<Transaction> transactions)
        {
            var transactionList = transactions.ToList();
            var clusters = InitiateClusters(transactionList).ToList();
            var transactionClusters = clusters
                .SelectMany(x => x.TransactionKeys.Select(y => new { Cluster = x.Number, Transaction = y }))
                .ToDictionary(x => x.Transaction, y => y.Cluster);
            while (true)
            {
                var moved = false;
                foreach (var transaction in transactionList)
                {
                    var currClusterNumber = transactionClusters[transaction.Id];
                    var currCluster = clusters.First(x => x.Number == currClusterNumber);
                    currCluster.RemoveTransaction(transaction);
                    if (currCluster.IsEmpty)
                        clusters.Remove(currCluster);
                    var bestCluster = AddTransaction(clusters, transaction);

                    // если произошло перемещение транзакции в другой кластер
                    // если удалили транзакцию из текущего слота и он стал пустым,а саму транзакцию добавили в новый созданный кластер, то перемещение не считается
                    if (!(currCluster.IsEmpty && bestCluster.Size == 1))
                    {
                        moved = true;
                    }
                    else
                    {
                        //Console.WriteLine("Nothing");
                    }
                    transactionClusters[transaction.Id] = bestCluster.Number;


                }
                if (!moved)
                    break;
            }


            return clusters;
        }

        /// <summary>
        /// Добавление транзакции в наиболее подходящий кластер
        /// </summary>
        /// <param name="clusters"></param>
        /// <param name="transaction"></param>
        /// <returns>Кластер, куда была добавлена транзакция</returns>
        private ClopeCluster AddTransaction(IList<ClopeCluster> clusters, Transaction transaction)
        {
            var bestCluster = new ClopeCluster(transaction);
            clusters.Add(bestCluster);
            var maxProfit = GetProfitValue(clusters);
            for (var i = 0; i < clusters.Count - 1; ++i)
            {
                bestCluster.RemoveTransaction(transaction);
                clusters[i].AddTransaction(transaction);
                var currProfit = GetProfitValue(clusters);
                if (currProfit > maxProfit)
                {
                    maxProfit = currProfit;
                    bestCluster = clusters[i];
                }
                else
                {
                    clusters[i].RemoveTransaction(transaction);
                    bestCluster.AddTransaction(transaction);
                }
            }

            DeleteEmptyCluster(clusters);
            return bestCluster;
        }

        /// <summary>
        /// Удлаение пустого кластера
        /// </summary>
        /// <param name="clusters">Кластеры</param>
        private static void DeleteEmptyCluster(IList<ClopeCluster> clusters)
        {
            var emptyCluster = clusters.FirstOrDefault(x => x.IsEmpty);
            if (emptyCluster != null)
                clusters.Remove(emptyCluster);
        }

        /// <summary>
        /// Расчет эффективности разбиения на кластеры (функция стоимости)
        /// </summary>
        /// <param name="clusters">Список кластеров</param>
        /// <returns>Значение эффективности</returns>
        private double GetProfitValue(IList<ClopeCluster> clusters)
        {
            return clusters.Where(x => !x.IsEmpty).Sum(x => x.Square * x.Size / System.Math.Pow(x.Width, _r)) / clusters.Sum(x => x.Size);
        }

        /// <summary>
        /// Инициализация начальных кластеров
        /// </summary>
        /// <param name="transactions">Транзакции</param>
        /// <returns>Кластеры</returns>
        private IEnumerable<ClopeCluster> InitiateClusters(IList<Transaction> transactions)
        {
            var clusters = new List<ClopeCluster>();
            foreach (var transaction in transactions)
            {
                AddTransaction(clusters, transaction);
            }
            return clusters;
        }
    }
}
