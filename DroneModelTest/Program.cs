using System.Numerics;
using System.Text;
using DroneModelTest;

var drone1 = new Drone(15, Vector3.Zero, new List<DroneTrajectoryPartProperty>
{
    new DroneTrajectoryPartProperty
    {
        EndPoint = new Vector3(x: 0, y: 100, z: 0),
        InitialVelocityModule = 5,
    },

    new DroneTrajectoryPartProperty
    {
        EndPoint = new Vector3(x: 100, y: 100, z: 0),
        InitialVelocityModule = 5,
    },

    new DroneTrajectoryPartProperty
    {
        EndPoint = Vector3.Zero,
        InitialVelocityModule = 5,
    },
});

var drone2 = new Drone(30, new Vector3(x: -50, y: 100, z: 0), new List<DroneTrajectoryPartProperty>
{
    new DroneTrajectoryPartProperty
    {
        EndPoint = new Vector3(x: 0, y: 100, z: 0),
        InitialVelocityModule = 5,
    },

    new DroneTrajectoryPartProperty
    {
        EndPoint = new Vector3(x: 100, y: 100, z: 0),
        InitialVelocityModule = 5,
    },

    new DroneTrajectoryPartProperty
    {
        EndPoint = Vector3.Zero,
        InitialVelocityModule = 5,
    },
});

void PrintDroneStatus(Drone drone, int iterationId)
{
    Console.WriteLine("{0} Drone[{1}]: Position={2} Vel={3} Accel={4}", iterationId, drone.Guid, drone.Position.ToString("F2"), drone.Velocity.Length(), drone.Acceleration.Length());
}

PrintDroneStatus(drone1, 0);
PrintDroneStatus(drone2, 0);

int iteration = 0;
while(drone1.Update() || drone2.Update())
{
    ++iteration;
    PrintDroneStatus(drone1, iteration);
    PrintDroneStatus(drone2, iteration);
}
