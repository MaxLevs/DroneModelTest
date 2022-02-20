using System.Numerics;
using System.Text;
using DroneModelTest.Models;
using DroneModelTest.Services;

ReportService reportService = new();

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

var result = simulation.StartSimulation();
reportService.ReportToConsole(result);

