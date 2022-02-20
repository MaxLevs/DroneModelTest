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

        public Guid Guid { get; } = Guid.NewGuid();

        public IEnumerable<DroneProperties> InitialProperties { get; }
        public List<Drone> ListOfSimulatedDrones { get; }
        public int IterationCount { get; private set; }
        private List<SimulationResultItem> PerIterationResults { get; init; }

        public SimulationService(IEnumerable<DroneProperties> dronesProperties)
        {
            _snapshotService = new SnapshotProducerService();

            IterationCount = 0;
            PerIterationResults = new List<SimulationResultItem>();

            InitialProperties = dronesProperties;
            ListOfSimulatedDrones = new List<Drone>();
            ListOfSimulatedDrones.AddRange(InitialProperties.Select(properties => new Drone(properties.Guid,
                                                                                            properties.Radius, 
                                                                                            properties.StartPosition,
                                                                                            properties.TrajectoryProperties ?? Enumerable.Empty<DroneTrajectoryPartProperties>(),
                                                                                            ListOfSimulatedDrones))
                                                            .ToList());

            SaveIterationResult();
        }

        /// <summary>
        /// Начать симуляцию
        /// </summary>
        /// <returns>Результаты симуляции</returns>
        public SimulationResult StartSimulation()
        {
            while(ListOfSimulatedDrones.Any(drone => drone.Status == DroneStatus.Normal))
            {
                IterationCount++;
                Update();
            }

            return new SimulationResult
            {
                Guid = Guid,
                PerIterationResults = PerIterationResults
            };
        }

        /// <summary>
        /// Провести шаг симуляции
        /// </summary>
        private void Update()
        {
            foreach (var drone in ListOfSimulatedDrones)
            {
                drone.Update();
            }

            SaveIterationResult();
        }

        /// <summary>
        /// Сохранить текущие состояния дронов в результаты симуляции
        /// </summary>
        private void SaveIterationResult()
        {
            PerIterationResults.Add(new SimulationResultItem
            {
                Iteration = IterationCount,
                DroneSnapshots = _snapshotService.ProduceSnapshots(ListOfSimulatedDrones),
            });
        }
    }
}
