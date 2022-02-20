using System.Numerics;

namespace DroneModelTest
{
    public class Drone
    {
        public Guid Guid { get; init; } = Guid.NewGuid();

        public float Radius { get; init; }

        public Vector3 Position => MovementService.Position;
        public Vector3 Velocity => MovementService.Velocity;
        public Vector3 Acceleration => MovementService.Acceleration;

        MovementService MovementService { get; set; }

        public Drone(float radius, Vector3 startPosition, IEnumerable<DroneTrajectoryPartProperty> trajectoryProperties)
        {
            Radius = radius;

            var trajectoryParts = trajectoryProperties.Select((prop, i) => new LineTrajectoryPart
            {
                EndPoint = prop.EndPoint,
                InitialVelocityModule = prop.InitialVelocityModule,
                InitialAccelerationModule = prop.InitialAccelerationModule
            });

            MovementService = new MovementService(startPosition, trajectoryParts);
        }

        public bool Update()
        {
            return MovementService.Move();
        }
    }
}
