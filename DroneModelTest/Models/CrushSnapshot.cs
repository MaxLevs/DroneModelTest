using DroneModelTest.Models.Drones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest.Models
{
    /// <summary>
    /// Отчет о столкновении
    /// </summary>
    public class CrushSnapshot
    {
        /// <summary>
        /// Столкнувшийся дрон 1
        /// </summary>
        DroneSnapshot Drone1 { get; init; }

        /// <summary>
        /// Столкнувшийся дрон 2
        /// </summary>
        DroneSnapshot Drone2 { get; init; }

        /// <summary>
        /// Позиция столкновения
        /// </summary>
        Vector3 Positon => (Drone1.Position + Drone2.Position) / 2;
    }
}
