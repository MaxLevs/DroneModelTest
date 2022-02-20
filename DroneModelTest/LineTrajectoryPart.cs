using System.Numerics;

namespace DroneModelTest
{
    public class LineTrajectoryPart : ITrajectoryPart
    {
        #region Trajectorie's properties

        public Vector3? StartPoint { get; set; }
        public Vector3 EndPoint { get; set; }

        public float InitialVelocityModule { get; set; }
        public float InitialAccelerationModule { get; set; }

        #endregion

        #region Calculable properties and Methods

        public Vector3 Path => EndPoint - (StartPoint ?? throw new InvalidOperationException($"{nameof(StartPoint)} must be initialized before calculation started"));
        public Vector3 Course => Vector3.Normalize(Path);
        public Vector3 InitialVelocity => MovementService.VelocityToVector(InitialVelocityModule, Course);
        public Vector3 InitialAcceleration => MovementService.AccelerationToVector(InitialAccelerationModule, Course);

        public bool IsEnded(Vector3 position)
        {
            if (position == EndPoint)
            {
                return true;
            }

            var actualEndPointDirection = Vector3.Normalize(EndPoint - position);
            return Math.Abs(Vector3.Dot(Course, actualEndPointDirection) + 1) < 0.1;
        }

        #endregion
    }
}
