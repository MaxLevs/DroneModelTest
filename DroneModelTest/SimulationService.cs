using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest
{
    /// <summary>
    /// Сервис, занимающийся проведением симуляции
    /// </summary>
    public class SimulationService
    {
        private readonly SnapshotProducerService _snapshotService;

        public IEnumerable<DroneProperties> InitialProperties { get; }
        public List<Drone> ListOfSimulatedDrones { get; }
        public List<SimulationIterationResult> IterationResults { get; }

        public int IterationCount { get; private set; }

        public SimulationService(IEnumerable<DroneProperties> dronesProperties)
        {
            _snapshotService = new SnapshotProducerService();
            IterationCount = 0;

            InitialProperties = dronesProperties;
            ListOfSimulatedDrones = new List<Drone>();
            ListOfSimulatedDrones.AddRange(InitialProperties.Select(properties => new Drone(properties.Guid,
                                                                                            properties.Radius, 
                                                                                            properties.StartPosition,
                                                                                            properties.TrajectoryProperties ?? Enumerable.Empty<DroneTrajectoryPartProperties>(),
                                                                                            ListOfSimulatedDrones))
                                                            .ToList());

            IterationResults = new List<SimulationIterationResult>();

            SaveIterationResult();
        }

        public IEnumerable<SimulationIterationResult> StartSimulation()
        {
            while(ListOfSimulatedDrones.Any(drone => drone.Status == DroneStatus.Normal))
            {
                IterationCount++;
                Update();
            }

            return IterationResults;
        }

        private void Update()
        {
            foreach (var drone in ListOfSimulatedDrones)
            {
                drone.Update();
            }

            SaveIterationResult();
        }

        private void SaveIterationResult()
        {
            IterationResults.Add(new SimulationIterationResult
            {
                Iteration = IterationCount,
                DroneSnapshots = _snapshotService.ProduceSnapshots(ListOfSimulatedDrones),
            });
        }
    }
}
