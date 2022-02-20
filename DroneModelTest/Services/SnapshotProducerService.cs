using DroneModelTest.Models.Drones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest.Services
{
    public class SnapshotProducerService
    {
        public DroneSnapshot CreateSnapshot(Drone drone)
        {
            return new DroneSnapshot
            {
                Guid = drone.Guid,
                Status = drone.Status,
                Radius = drone.Radius,
                Position = drone.Position,
                Velocity = drone.Velocity,
                Acceleration = drone.Acceleration,
            };
        }

        public IEnumerable<DroneSnapshot> ProduceSnapshots(IEnumerable<Drone> drones)
        {
            return drones.Select(CreateSnapshot).ToList();
        }
    }
}
