using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest
{
    public class SimulationIterationResult
    {
        public int Iteration { get; init; }
        public IEnumerable<DroneSnapshot> DroneSnapshots { get; init; }
    }
}
