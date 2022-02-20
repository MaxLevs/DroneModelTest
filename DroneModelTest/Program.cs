using System.Numerics;
using System.Text;
using DroneModelTest;

List<DroneProperties> dronesProperties = new()
{
    new DroneProperties
    {
        Guid = Guid.NewGuid(),
        Radius = 15,
        StartPosition = new Vector3(x: 0, y: 0, z: 0),
        TrajectoryProperties = new List<DroneTrajectoryPartProperties>
        {
            new DroneTrajectoryPartProperties
            {
                EndPoint = new Vector3(x: 0, y: 100, z: 0),
                InitialVelocityModule = 5
            },
            new DroneTrajectoryPartProperties
            {
                EndPoint = new Vector3(x: 100, y: 100, z: 0),
                InitialVelocityModule = 5
            },
            new DroneTrajectoryPartProperties
            {
                EndPoint = new Vector3(x: 0, y: 0, z: 0),
                InitialVelocityModule = 5
            },
        }
    },

    new DroneProperties
    {
        Guid = Guid.NewGuid(),
        Radius = 30,
        StartPosition = new Vector3(x: -50, y: 100, z: 0),
        TrajectoryProperties = new List<DroneTrajectoryPartProperties>
        {
            new DroneTrajectoryPartProperties
            {
                EndPoint = new Vector3(x: 100, y: 100, z: 0),
                InitialVelocityModule = 5
            },
            new DroneTrajectoryPartProperties
            {
                EndPoint = new Vector3(x: 0, y: 0, z: 0),
                InitialVelocityModule = 10
            },
        }
    },
};

SimulationService simulation = new(dronesProperties);

void PrintDroneStatus(Drone drone)
{
    Console.WriteLine("Drone[{0}]: Position={1} Vel={2} Accel={3}", drone.Guid, drone.Position.ToString("F2"), drone.Velocity.Length(), drone.Acceleration.Length());
}

