using System.Numerics;
using System.Text;
using DroneModelTest;

List<Drone> drones = new();

drones.Add(new Drone(15, Vector3.Zero, new List<DroneTrajectoryPartProperty>
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
}, drones));

drones.Add(new Drone(30, new Vector3(x: -50, y: 100, z: 0), new List<DroneTrajectoryPartProperty>
{
    new DroneTrajectoryPartProperty
    {
        EndPoint = new Vector3(x: 0, y: 100, z: 0),
        InitialVelocityModule = 10,
    },

    new DroneTrajectoryPartProperty
    {
        EndPoint = new Vector3(x: 100, y: 100, z: 0),
        InitialVelocityModule = 10,
    },

    new DroneTrajectoryPartProperty
    {
        EndPoint = Vector3.Zero,
        InitialVelocityModule = 10,
    },
}, drones));

void PrintDroneStatus(Drone drone)
{
    Console.WriteLine("Drone[{0}]: Position={1} Vel={2} Accel={3}", drone.Guid, drone.Position.ToString("F2"), drone.Velocity.Length(), drone.Acceleration.Length());
}

Console.WriteLine("Дано:");
foreach (var drone in drones)
{
    PrintDroneStatus(drone);
}
Console.WriteLine();

int iteration = 0;

while(drones.Select(drone => drone.Status).Any(status => status == DroneStatus.Normal))
{
    ++iteration;

    Console.WriteLine($"Итерация: {iteration}");

    foreach (var drone in drones)
    {
        drone.Update();
    }

    foreach (var drone in drones)
    {
        PrintDroneStatus(drone);
    }

    Console.WriteLine();
}
