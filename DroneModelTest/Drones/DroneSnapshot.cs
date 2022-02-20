using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest
{
    public class DroneSnapshot
    {
        public Guid Guid { get; init; }
        public DroneStatus Status { get; init; }
        public float Radius { get; init; }
        public Vector3 Position { get; init; }
        public Vector3 Velocity { get; init; }
        public Vector3 Acceleration { get; init; }
    }
}
