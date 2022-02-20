using System.Numerics;

namespace DroneModelTest
{
    /// <summary>
    /// Представляет конфигурацию части маршрута
    /// </summary>
    public class DroneTrajectoryPartProperty
    {
        public Vector3 EndPoint { get; set; }
        public float InitialVelocityModule { get; set; }
        public float InitialAccelerationModule { get; set; }
    }
}
