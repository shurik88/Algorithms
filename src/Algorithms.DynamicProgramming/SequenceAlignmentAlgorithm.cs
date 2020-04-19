using System;
using System.Collections.Generic;

namespace Algorithms.DynamicProgramming
{
    /// <summary>
    ///  Алгоритм выравнивания последовательностей
    /// </summary>
    /// <typeparam name="TElement">Тип элемента последовательности</typeparam>
    public class SequenceAlignmentAlgorithm<TElement>
        where TElement: IEquatable<TElement>
    {
        /// <summary>
        /// Получение выравненных последовательностей
        /// </summary>
        /// <remarks>
        ///     Если элемент пропущен, то будет указано дефолтное значение <typeparamref name="TElement"/>
        /// </remarks>
        /// <param name="first">Первая последовательностьы</param>
        /// <param name="second">Вторая последовательность</param>
        /// <param name="penalty">Штраф за несовпадение элементов</param>
        /// <param name="gapPenalty">Штраф за разрыв цепочки (пропуск элемента)</param>
        /// <returns>Выравненные последовательности, суммарное расстояние(штраф)</returns>
        public (TElement[] First, TElement[] Second, int DistanceCost) GetAlignment(TElement[] first, TElement[] second, int penalty, int gapPenalty)
        {
            var solution = new int[first.Length + 1, second.Length + 1];
            for (var i = 0; i <= first.Length; ++i)
                solution[i, 0] = i * gapPenalty;
            for (var j = 0; j <= second.Length; ++j)
                solution[0, j] = j * gapPenalty;

            for(var i = 1; i <= first.Length; ++i)
            {
                for(var j = 1; j <= second.Length; ++j)
                {
                    solution[i, j] = Math.Min(solution[i - 1, j - 1] + (first[i - 1].Equals(second[j - 1]) ? 0 : penalty), solution[i - 1, j] + gapPenalty);
                    solution[i, j] = Math.Min(solution[i, j], solution[i, j - 1] + gapPenalty);
                }
            }

            var currI = first.Length;
            var currJ = second.Length;
            var actualFirst = new List<TElement>(Math.Max(first.Length, second.Length)); //new TElement[Math.Max(first.Length, second.Length)];
            var actualSecond = new List<TElement>(Math.Max(first.Length, second.Length));
            while (currI > 0 && currJ > 0)
            {
                var currElementsEquals = first[currI - 1].Equals(second[currJ - 1]);
                if (solution[currI, currJ] == solution[currI - 1, currJ - 1] + (currElementsEquals ? 0 : penalty))
                {
                    actualFirst.Add(first[currI - 1]);
                    actualSecond.Add(second[currJ - 1]);
                    currI -= 1;
                    currJ -= 1;
                }
                else if (solution[currI, currJ] == solution[currI - 1, currJ] + gapPenalty)
                {
                    actualFirst.Add(first[currI - 1]);
                    actualSecond.Add(default);
                    currI -= 1;
                }
                else if (solution[currI, currJ] == solution[currI, currJ - 1] + gapPenalty)
                {
                    actualFirst.Add(default);
                    actualSecond.Add(second[currJ - 1]);
                    currJ -= 1;
                }
                else
                    throw new ArgumentOutOfRangeException("Unsupported case");
            }
            actualFirst.Reverse();
            actualSecond.Reverse();
            return (actualFirst.ToArray(),  actualSecond.ToArray(), solution[first.Length, second.Length]);
        }
    }
}
