using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest.Models.Drones
{
    public class DroneSnapshot : IEquatable<DroneSnapshot>
    {
        public Guid Guid { get; init; }
        public DroneStatus Status { get; init; }
        public float Radius { get; init; }
        public Vector3 Position { get; init; }
        public Vector3 Velocity { get; init; }
        public Vector3 Acceleration { get; init; }

        public bool Equals(DroneSnapshot? other)
        {
            return DroneSnapshotComparer.Instance.Equals(this, other);
        }

        public override bool Equals(object? obj)
        {
            if (obj is DroneSnapshot droneSnapshot)
            {
                return Equals(droneSnapshot);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return DroneSnapshotComparer.Instance.GetHashCode(this);
        }
    }

    public class DroneSnapshotComparer : IEqualityComparer<DroneSnapshot>
    {
        public bool Equals(DroneSnapshot? x, DroneSnapshot? y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            return x != null
                && y != null
                && x.Guid == y.Guid
                && x.Status == y.Status
                && x.Radius == y.Radius
                && x.Position == y.Position
                && x.Velocity == y.Velocity
                && x.Acceleration == y.Acceleration;
        }

        public int GetHashCode([DisallowNull] DroneSnapshot obj)
        {
            HashCode hashCode = new();

            hashCode.Add(obj.Guid);
            hashCode.Add(obj.Status);
            hashCode.Add(obj.Radius);
            hashCode.Add(obj.Position);
            hashCode.Add(obj.Velocity);
            hashCode.Add(obj.Acceleration);

            return hashCode.ToHashCode();
        }

        private DroneSnapshotComparer()
        {

        }

        public static DroneSnapshotComparer Instance { get; } = new DroneSnapshotComparer();
    }
}
