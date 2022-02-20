using System.Numerics;

namespace DroneModelTest.Models.Drones.Trajectories
{
    /// <summary>
    /// Представляет некий кусок траектории
    /// </summary>
    public interface ITrajectoryPart
    {
        /// <summary>
        /// Точка начала прохождения траектории
        /// </summary>
        public Vector3? StartPoint { get; set; }

        /// <summary>
        /// Определят напраление движения
        /// Должен быть нормализованным вектором
        /// </summary>
        Vector3 Course { get; }

        /// <summary>
        /// Скорость движения
        /// </summary>
        Vector3 InitialVelocity { get; }

        /// <summary>
        /// Ускорение
        /// </summary>
        Vector3 InitialAcceleration { get; }

        /// <summary>
        /// Пройдена ли траектория
        /// </summary>
        bool IsEnded(Vector3 position);
    }
}
