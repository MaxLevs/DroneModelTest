using System.Numerics;

namespace DroneModelTest
{
    /// <summary>
    /// Представляет дрона, проходящего некий маршрут
    /// </summary>
    public class Drone
    {
        /// <summary>
        /// Уникальный ID дрона
        /// </summary>
        public Guid Guid { get; init; } = Guid.NewGuid();

        /// <summary>
        /// В каком состоянии находится дрон
        /// </summary>
        public DroneStatus Status { get; private set; } = DroneStatus.Normal;

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

        public Drone(float radius,
                     Vector3 startPosition,
                     IEnumerable<DroneTrajectoryPartProperty> trajectoryProperties,
                     IEnumerable<Drone> dronesInCurrentSimulation)
        {
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
        /// Обновить состояние элемента моделирования
        /// </summary>
        public void Update()
        {
            var isDroneMoved = MovementService.Move();

            if (!isDroneMoved || MovementService.IsEnded)
            {
                Status = DroneStatus.Finished;
            }
        }
    }

    /// <summary>
    /// Представляет статус состояния дрона в каждый момент времени
    /// </summary>
    public enum DroneStatus
    {
        Normal,   // Обычное состояние
        Finished, // Прохождение по маршруту завершено
        Crushed   // Дрон столкнулся
    }
}
