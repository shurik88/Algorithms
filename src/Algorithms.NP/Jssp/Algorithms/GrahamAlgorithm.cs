using Algorithms.Structures.Heaps;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Algorithms.NP.Jssp.Algorithms
{
    /// <summary>
    ///     Жадный алгоритм Грэма.
    /// </summary>
    public class GrahamAlgorithm : IJsspAlgorithm
    {
        /// <inheritdoc/>
        public AlgorithmOutput FindSolution(AlgorithmInput input)
        {
            var machines = input.Machines.ToList();
            var jobs = input.Jobs.OrderByDescending(x => x.Duration).ToList();

            var machinesHeap = new Heap<int, MachineJobs>(HeapType.Min, machines.Count);
            foreach (var machine in machines)
                machinesHeap.Insert(new Structures.ComparableElement<int, MachineJobs> { Key = 0, Value = new MachineJobs(machine) });


            foreach(var job in jobs)
            {
                var leastLoaded = machinesHeap.Extract();
                leastLoaded.Value.AddJob(job);
                leastLoaded.Key = leastLoaded.Value.Makespan;
                machinesHeap.Insert(leastLoaded);
            }

            return new AlgorithmOutput
            {
                Makespan = machinesHeap.Values.Max(x => x.Makespan),
                Assignments = machinesHeap.Values.Select(x => new { Assignments = x.Jobs.Select(y => new Assignment { Job = y, Machine = x.Machine }) }).SelectMany(x => x.Assignments)
            };
        }

        private class MachineJobs
        {
            public MachineJobs(Machine machine)
            {
                Machine = machine;
                Jobs = new Collection<Job>();
            }
            public Machine Machine { get; private set; }

            public ICollection<Job> Jobs { get; private set; }

            public int Makespan { get; private set; }

            public void AddJob(Job job)
            {
                Jobs.Add(job);
                Makespan += job.Duration;
            }
        }
    }
}
