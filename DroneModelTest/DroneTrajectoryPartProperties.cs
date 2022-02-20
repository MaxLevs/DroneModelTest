using System.Numerics;

namespace DroneModelTest
{
    /// <summary>
    /// Представляет конфигурацию части маршрута
    /// </summary>
    public class DroneTrajectoryPartProperties
    {
        public Vector3 EndPoint { get; set; }
        public float InitialVelocityModule { get; set; }
        public float InitialAccelerationModule { get; set; }
    }

    /// <summary>
    /// Представляет конфигурацию дрона
    /// </summary>
    public class DroneProperties
    {
        public float Radius { get; set; }
        public Vector3 StartPosition { get; set; }
        public IEnumerable<DroneTrajectoryPartProperties>? TrajectoryProperties { get; set; }
    }
}
