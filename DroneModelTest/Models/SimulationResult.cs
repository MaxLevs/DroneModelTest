using DroneModelTest.Models.Drones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest.Models
{
    public class SimulationResult
    {
        public Guid Guid { get; init; }
        public int Iterations => PerIterationResults?.Count ?? 0;
        public IReadOnlyCollection<SimulationResultItem>? PerIterationResults { get; init; }
    }

    public class SimulationResultItem
    {
        public int Iteration { get; init; }
        public IEnumerable<DroneSnapshot>? DroneSnapshots { get; init; }
    }
}
