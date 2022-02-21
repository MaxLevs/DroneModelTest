using DroneModelTest.Models.Drones.Trajectories;
using System.Collections;
using System.Numerics;

namespace DroneModelTest.Services
{
    public class MovementService
    {
        #region Private fields

        private readonly List<ITrajectoryPart> _trajectory = new();
        private int _currentSegmentId = -1;

        #endregion


        #region Phisics properties

        public Vector3 Position { get; private set; }
        public Vector3 Velocity { get; private set; }
        public Vector3 Acceleration { get; private set; }

        #endregion


        #region Calculable properties

        public bool IsInitialized => _trajectory.Count >= 0;

        public bool IsEnded
        {
            get
            {
                if (!_trajectory.Any())
                {
                    return true;
                }

                if (_currentSegmentId == -1)
                {
                    return false;
                }

                return _currentSegmentId + 1 == _trajectory.Count
                    && _trajectory[_currentSegmentId].IsEnded(Position);
            }
        }

        #endregion


        public MovementService(Vector3 startPosition)
        {
            Position = startPosition;
        }

        public MovementService(Vector3 startPosition, IEnumerable<ITrajectoryPart> trajectoryParts)
            : this(startPosition)
        {
            _trajectory.AddRange(trajectoryParts);
        }


        /// <summary>
        /// Prepare module to start moving
        /// </summary>
        /// <exception cref="InvalidOperationException">Module is already initialized</exception>
        public void Initialize()
        {
            if (_currentSegmentId >= 0)
            {
                throw new InvalidOperationException($"{nameof(MovementService)} is already initialized");
            }

            PeekNextSegment();
        }

        /// <summary>
        /// Переместиться по маршруту на один шаг
        /// </summary>
        /// <returns>True if move is successed, otherwise - false</returns>
        /// <exception cref="InvalidOperationException">Module is already initialized</exception>
        public bool Move()
        {
            if (_currentSegmentId < 0)
            {
                throw new InvalidOperationException($"{nameof(MovementService)} is not initialized");
            }

            if (!_trajectory.Any())
            {
                return false;
            }

            if (_trajectory[_currentSegmentId].IsEnded(Position))
            {
                var isMoveAvailable = PeekNextSegment();

                if (!isMoveAvailable)
                {
                    return false;
                }
            }

            // Векторные уравнения ускоренного движения с dt = 1
            Velocity += Acceleration;
            Position += Velocity;

            return true;
        }

        /// <summary>
        /// Переходит к движению по следующему отрезку маршрута.
        /// Переход также влияет на Скорость и Ускорение
        /// </summary>
        /// <returns>True если переход возможен и успешен, иначе - false</returns>
        public bool PeekNextSegment()
        {
            ITrajectoryPart currentSegment;

            // Последовательно выбираем участки маршрута и выбираем первый подходящий для прохожения
            do
            {
                // Проверяем доступность следующего по счету участка маршрута
                if (_trajectory.Count <= _currentSegmentId + 1)
                {
                    ResetVelocityAndAcceleration();
                    return false;
                }

                // Получаем следующий участок маршрута
                _currentSegmentId++;
                currentSegment = _trajectory[_currentSegmentId];

                // Устанавливаем начальную точку отрезка маршрута (без этой операции вычисления дальше не пойдут)
                currentSegment.StartPoint = Position;

                // Если участок маршрута уже считается завершенным, то берем следующий
                if (currentSegment.IsEnded(Position))
                {
                    ResetVelocityAndAcceleration();
                    continue;
                }

                // Получаем начальные скорость и ускорение
                Velocity = currentSegment.InitialVelocity;
                Acceleration = currentSegment.InitialAcceleration;

                return true;
            }

            while (true);
        }

        public void ResetVelocityAndAcceleration()
        {
            Velocity = Vector3.Zero;
            Acceleration = Vector3.Zero;
        }


        public static Vector3 VelocityToVector(float velocityModule, Vector3 cource) => cource * velocityModule;
        public static Vector3 AccelerationToVector(float accelerationModule, Vector3 cource) => cource * accelerationModule;
    }
}
