using DroneModelTest.Models.Drones;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest.Models
{
    /// <summary>
    /// Отчет о столкновении
    /// </summary>
    public class CrushSnapshot : IEquatable<CrushSnapshot>
    {
        /// <summary>
        /// Столкнувшийся дрон 1
        /// </summary>
        public DroneSnapshot Drone1 { get; init; }

        /// <summary>
        /// Столкнувшийся дрон 2
        /// </summary>
        public DroneSnapshot Drone2 { get; init; }

        /// <summary>
        /// Позиция столкновения
        /// </summary>
        public Vector3 Positon => (Drone1.Position + Drone2.Position) / 2;

        /// <summary>
        /// Актуальное расстояние между дронами
        /// </summary>
        public float ActualDistance => Vector3.Distance(Drone1.Position, Drone2.Position);

        /// <summary>
        /// Критическое расстояние
        /// </summary>
        public float CriticalDistance => Drone1.Radius + Drone2.Radius;

        #region IEquatable Members

        public bool Equals(CrushSnapshot? other)
        {
            return CrushSnapshotComparer.Instance.Equals(this, other);
        }

        public override bool Equals(object? obj)
        {
            if (obj is CrushSnapshot snapshot)
            {
                return Equals(snapshot);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return CrushSnapshotComparer.Instance.GetHashCode(this);
        }

        #endregion


        #region IEnumerable Members

        public IEnumerable<DroneSnapshot> GetEnumerable()
        {
            return new List<DroneSnapshot> { Drone1, Drone2 };
        }

        #endregion
    }

    public class CrushSnapshotComparer : IEqualityComparer<CrushSnapshot>
    {
        public bool Equals(CrushSnapshot? x, CrushSnapshot? y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            var droneSnapshots1 = x?.GetEnumerable()?.OrderBy(drone => drone.Guid)?.ToList();
            var droneSnapshots2 = y?.GetEnumerable()?.OrderBy(drone => drone.Guid)?.ToList();

            return x != null
                && y != null
                && DroneSnapshotComparer.Instance.Equals(droneSnapshots1?.First(), droneSnapshots2?.First())
                && DroneSnapshotComparer.Instance.Equals(droneSnapshots1?.Last(), droneSnapshots2?.Last())
                && x.Positon == y.Positon;
        }

        public int GetHashCode([DisallowNull] CrushSnapshot obj)
        {
            HashCode hashCode = new();
            var droneSnapshots = obj.GetEnumerable().OrderBy(drone => drone.Guid).ToList();

            foreach (var droneSnapshot in droneSnapshots)
            {
                hashCode.Add(droneSnapshot);
            }

            hashCode.Add(obj.Positon.GetHashCode());

            return hashCode.ToHashCode();
        }

        private CrushSnapshotComparer()
        {

        }

        public static CrushSnapshotComparer Instance { get; set; } = new CrushSnapshotComparer();
    }
}
