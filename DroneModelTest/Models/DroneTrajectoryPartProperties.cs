using System.Numerics;

namespace DroneModelTest.Models
{
    /// <summary>
    /// Представляет конфигурацию дрона
    /// </summary>
    public class DroneProperties
    {
        /// <summary>
        /// Уникальный идентификатор дрона. 
        /// Требуется для сопоставления входных данных с результатами
        /// </summary>
        public Guid Guid { get; set; }
        public float Radius { get; set; }
        public Vector3 StartPosition { get; set; }
        public IEnumerable<DroneTrajectoryPartProperties>? TrajectoryProperties { get; set; }
    }

    /// <summary>
    /// Представляет конфигурацию части маршрута
    /// </summary>
    public class DroneTrajectoryPartProperties
    {
        public Vector3 EndPoint { get; set; }
        public float InitialVelocityModule { get; set; }
        public float InitialAccelerationModule { get; set; }
    }
}
