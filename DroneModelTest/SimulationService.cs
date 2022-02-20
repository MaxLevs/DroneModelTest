using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest
{
    /// <summary>
    /// Сервис, занимающийся проведением симуляции
    /// </summary>
    public class SimulationService
    {
        IEnumerable<DroneProperties> InitialProperties { get; }
        List<Drone> ListOfSimulatedDrones;

        public SimulationService(IEnumerable<DroneProperties> dronesProperties)
        {
            InitialProperties = dronesProperties;
            ListOfSimulatedDrones = new List<Drone>();
            ListOfSimulatedDrones.AddRange(InitialProperties.Select(properties => new Drone(properties.Guid,
                                                                                            properties.Radius, 
                                                                                            properties.StartPosition,
                                                                                            properties.TrajectoryProperties ?? Enumerable.Empty<DroneTrajectoryPartProperties>(),
                                                                                            ListOfSimulatedDrones))
                                                            .ToList());
        }

        public void Update()
        {
            foreach (var drone in ListOfSimulatedDrones)
            {
                drone.Update();
            }
        }
    }
}
