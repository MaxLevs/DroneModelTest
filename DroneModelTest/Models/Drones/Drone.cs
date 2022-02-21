using DroneModelTest.Models;
using DroneModelTest.Models.Drones.Trajectories;
using DroneModelTest.Services;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DroneModelTest.Models.Drones
{
    /// <summary>
    /// Представляет дрона, проходящего некий маршрут
    /// </summary>
    public class Drone
    {
        /// <summary>
        /// Уникальный ID дрона
        /// </summary>
        public Guid Guid { get; init; }

        /// <summary>
        /// В каком состоянии находится дрон
        /// </summary>
        public DroneStatus Status { get; private set; } = DroneStatus.NotInitialized;

        /// <summary>
        /// Радиус, определяющий область, в которой находится дрон
        /// </summary>
        public float Radius { get; init; }


        public Vector3 Position => MovementService.Position;
        public Vector3 Velocity => MovementService.Velocity;
        public Vector3 Acceleration => MovementService.Acceleration;

        /// <summary>
        /// Сервис, занимающийся перемещением дрона по заданному маршруту
        /// </summary>
        MovementService MovementService { get; set; }

        public Drone(Guid guid,
                     float radius,
                     Vector3 startPosition,
                     IEnumerable<DroneTrajectoryPartProperties> trajectoryProperties,
                     IEnumerable<Drone> dronesInCurrentSimulation)
        {
            Guid = guid;
            Radius = radius;

            var trajectoryParts = trajectoryProperties.Select(prop => new LineTrajectoryPart
            {
                EndPoint = prop.EndPoint,
                InitialVelocityModule = prop.InitialVelocityModule,
                InitialAccelerationModule = prop.InitialAccelerationModule
            });

            MovementService = new MovementService(startPosition, trajectoryParts);
        }

        /// <summary>
        /// Prepare drone for moving
        /// </summary>
        /// <exception cref="InvalidOperationException">Drone is already initialized</exception>
        public void Initialize()
        {
            if (Status != DroneStatus.NotInitialized)
            {
                throw new InvalidOperationException($"{nameof(Drone)}[{Guid}] is already initialized");
            }

            MovementService.Initialize();

            if (MovementService.IsEnded)
            {
                Status = DroneStatus.Finished;
            }

            else
            {
                Status = DroneStatus.Normal;
            }
        }

        /// <summary>
        /// Обновить состояние элемента моделирования
        /// </summary>
        /// <exception cref="InvalidOperationException">Drone is not initialized</exception>
        public void Update()
        {
            if (Status == DroneStatus.NotInitialized)
            {
                throw new InvalidOperationException($"{nameof(Drone)}[{Guid}] is not initialized");
            }

            if (Status != DroneStatus.Normal)
            {
                return;
            }

            var isDroneMoved = MovementService.Move();

            if (!isDroneMoved || MovementService.IsEnded)
            {
                Status = DroneStatus.Finished;
                MovementService.ResetVelocityAndAcceleration();
            }
        }

        public void SetCrushed()
        {
            Status = DroneStatus.Crushed;
            MovementService.ResetVelocityAndAcceleration();
        }
    }

    /// <summary>
    /// Представляет статус состояния дрона в каждый момент времени
    /// </summary>
    public enum DroneStatus
    {
        /// <summary>
        /// Дрон не инициализирован
        /// </summary>
        NotInitialized,

        /// <summary>
        /// Обычное состояние дрона
        /// </summary>
        Normal,

        /// <summary>
        /// Прохождение по маршруту завершено
        /// </summary>
        Finished,

        /// <summary>
        /// Дрон сломался
        /// </summary>
        Crushed
    }

    /// <summary>
    /// Сравнивает двух дронов
    /// </summary>
    public class DroneEqualityComparer : IEqualityComparer<Drone>
    {
        public bool Equals(Drone? x, Drone? y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            return x != null
                && y != null
                && x.Guid == y.Guid;
        }

        public int GetHashCode([DisallowNull] Drone obj)
        {
            return obj.Guid.GetHashCode();
        }

        private DroneEqualityComparer()
        {

        }

        private static DroneEqualityComparer? _instance;
        public static DroneEqualityComparer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DroneEqualityComparer();
                }

                return _instance;
            }
        }
    }
}
