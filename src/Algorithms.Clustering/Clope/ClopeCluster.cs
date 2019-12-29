using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Algorithms.Clustering.Clope
{
    /// <summary>
    /// Кластер алгоритма CLOPE
    /// </summary>
    public class ClopeCluster
    {
        /// <summary>
        /// Частота вхождения признаков в кластер
        /// </summary>
        private readonly Dictionary<long, int> _occ = new Dictionary<long, int>();

        /// <summary>
        /// Создание экземпляра класса <see cref="ClopeAlgorithm"/>
        /// </summary>
        /// <param name="transaction">Исходная транзакция</param>
        public ClopeCluster(Transaction transaction)
        {
            Number = Guid.NewGuid();
            AddTransaction(transaction);
        }

        /// <summary>
        /// Номер кластера
        /// </summary>
        public Guid Number { get; }

        /// <summary>
        /// Ширина кластера (количество уникальных признаков)
        /// </summary>
        public int Width => _occ.Count;

        /// <summary>
        /// Размер кластера (количество транзакций в кластере)
        /// </summary>
        public int Size => TransactionKeys.Count;

        /// <summary>
        /// Площадь кластера
        /// </summary>
        public int Square { get; private set; }

        /// <summary>
        /// Высота кластера
        /// </summary>
        public double Height => Square / Width;

        /// <summary>
        /// Признаки кластера
        /// </summary>
        public IEnumerable<long> D => _occ.Keys;

        /// <summary>
        /// Идентификаторы транзакций входящих в кластер
        /// </summary>
        public ICollection<long> TransactionKeys { get; } = new Collection<long>();

        /// <summary>
        /// Является ли кластер пустым
        /// </summary>
        public bool IsEmpty => Size == 0;

        /// <summary>
        /// Добавление транзакции в кластер
        /// </summary>
        /// <param name="transaction">Транзакция</param>
        public void AddTransaction(Transaction transaction)
        {
            if (TransactionKeys.Contains(transaction.Id))
                throw new InvalidOperationException("Transaction already added");

            var objects = transaction.Objects.ToList();
            foreach (var o in objects)
            {
                if (!_occ.ContainsKey(o))
                    _occ.Add(o, 1);
                else
                {
                    _occ[o] += 1;
                }
            }
            Square += objects.Count;
            TransactionKeys.Add(transaction.Id);
        }

        /// <summary>
        /// Удаление транзакции из кластера
        /// </summary>
        /// <param name="transaction">Транзакция</param>
        public void RemoveTransaction(Transaction transaction)
        {
            if (!TransactionKeys.Contains(transaction.Id))
                throw new ArgumentException("Transaction not exists");

            var objects = transaction.Objects.ToList();

            foreach (var o in objects)
            {
                _occ[o]--;
                if (_occ[o] == 0)
                    _occ.Remove(o);
            }

            Square -= objects.Count;
            TransactionKeys.Remove(transaction.Id);
        }
    }
}
