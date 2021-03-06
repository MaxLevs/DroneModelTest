using DroneModelTest.Models;
using DroneModelTest.Models.Drones;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest.Services
{
    /// <summary>
    /// Сервис, занимающийся проведением симуляции
    /// </summary>
    public class SimulationService
    {
        private readonly SnapshotProducerService _snapshotService;
        private readonly CollisionDetectorService _collisionDetectorService;

        public Guid Guid { get; } = Guid.NewGuid();

        public int IterationCount { get; private set; }
        public int MaxIterationsLimit { get; init; }
        public IEnumerable<DroneProperties> InitialProperties { get; }
        public List<Drone> ListOfSimulatedDrones { get; }
        private List<SimulationResultItem> PerIterationResults { get; init; }
        private HashSet<CrushSnapshot> CrushSnapshots { get; }

        public SimulationService(IEnumerable<DroneProperties> dronesProperties, int maxIterationsLimit = 10000)
        {
            _snapshotService = new SnapshotProducerService();

            IterationCount = 0;
            MaxIterationsLimit = maxIterationsLimit;
            PerIterationResults = new List<SimulationResultItem>();
            CrushSnapshots = new HashSet<CrushSnapshot>(CrushSnapshotComparer.Instance);

            InitialProperties = dronesProperties;
            ListOfSimulatedDrones = new List<Drone>();
            ListOfSimulatedDrones.AddRange(InitialProperties.Select(properties => new Drone(properties.Guid,
                                                                                            properties.Radius,
                                                                                            properties.StartPosition,
                                                                                            properties.TrajectoryProperties ?? Enumerable.Empty<DroneTrajectoryPartProperties>(),
                                                                                            ListOfSimulatedDrones))
                                                            .ToList());

            InitializeDrones();

            _collisionDetectorService = new CollisionDetectorService(ListOfSimulatedDrones);

            SaveIterationResult(); // Сохраняем начальные состояния в результаты
        }

        /// <summary>
        /// Начать симуляцию
        /// </summary>
        /// <returns>Результаты симуляции</returns>
        public SimulationResult StartSimulation()
        {
            if (ListOfSimulatedDrones.Any(drone => drone.Status == DroneStatus.NotInitialized))
            {
                throw new InvalidOperationException($"{nameof(SimulationService)} is not initialized");
            }

            while (IterationCount < MaxIterationsLimit
                && ListOfSimulatedDrones.Any(drone => drone.Status == DroneStatus.Normal))
            {
                IterationCount++;
                Update();
            }

            if (IterationCount == MaxIterationsLimit)
            {
                return new SimulationResult
                {
                    Guid = Guid,
                    StoppingExcuse = SimulationStoppingExcuse.OutOfMaxIterations,
                    PerIterationResults = PerIterationResults,
                    CrushPoints = CrushSnapshots
                };
            }

            return new SimulationResult
            {
                Guid = Guid,
                StoppingExcuse = SimulationStoppingExcuse.Finished,
                PerIterationResults = PerIterationResults,
                CrushPoints = CrushSnapshots
            };
        }

        /// <summary>
        /// Инициализировать днонов, участвующих в моделировании
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private void InitializeDrones()
        {
            if (ListOfSimulatedDrones.All(drone => drone.Status != DroneStatus.NotInitialized))
            {
                throw new InvalidOperationException($"{nameof(SimulationService)} is already initialized");
            }

            foreach (var drone in ListOfSimulatedDrones)
            {
                drone.Initialize();
            }
        }

        /// <summary>
        /// Провести шаг симуляции
        /// </summary>
        private void Update()
        {
            CheckCollisions();

            foreach (var drone in ListOfSimulatedDrones)
            {
                drone.Update();
            }

            CheckCollisions();

            SaveIterationResult();
        }

        private void CheckCollisions()
        {
            var collisions = _collisionDetectorService.GetAllCollisions();

            if (!collisions.Any())
            {
                return;
            }

            foreach (var collision in collisions)
            {
                collision.Key.SetCrushed();
            }

            var crushSnapshots = collisions.SelectMany(collisionGroup => collisionGroup.Select(collision => new HashSet<Drone>(DroneComparer.Instance) { collisionGroup.Key, collision }))
                                           .Distinct()
                                           .Select(twoCollisions => _snapshotService.ProduceCrushSnapshot(twoCollisions.First(), twoCollisions.Last()));

            foreach(var crushSnapshot in crushSnapshots)
            {
                CrushSnapshots.Add(crushSnapshot);
            }
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

    /// <summary>
    /// Причина завершения симуляции
    /// </summary>
    public enum SimulationStoppingExcuse
    {
        /// <summary>
        /// Прервано пользователем
        /// </summary>
        Interrupted,

        /// <summary>
        /// Симуляция завершена естественно
        /// </summary>
        Finished,

        /// <summary>
        /// Превышен лимит итераций
        /// </summary>
        OutOfMaxIterations,
    }
}
