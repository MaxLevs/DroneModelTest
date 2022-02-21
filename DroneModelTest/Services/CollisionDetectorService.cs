using DroneModelTest.Models.Drones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest.Services
{
    public class CollisionDetectorService
    {
        private IEnumerable<Drone> ListOfDrones { get; init; }

        public CollisionDetectorService(IEnumerable<Drone> drones)
        {
            ListOfDrones = drones;
        }

        /// <summary>
        /// Получает все коллизии для текущего набора дронов
        /// </summary>
        /// <returns></returns>
        public ILookup<Drone, Drone> GetAllCollisions()
        {
            return ListOfDrones.SelectMany(drone => GetAllCollidedWith(drone).Select(collision => KeyValuePair.Create(drone, collision)))
                               .ToLookup(collisions => collisions.Key, collisions => collisions.Value);
        }

        /// <summary>
        /// Получает все дроны, с которыми переданный дрон имеет коллизии
        /// </summary>
        /// <param name="drone">Некий дрон</param>
        /// <returns>Дроны, с которыми переданный дрон имеет коллизии</returns>
        public IEnumerable<Drone> GetAllCollidedWith(Drone drone)
        {
            var dangers = GetAllDangersFor(drone);
            return dangers.Where(anotherDrone => HasCollision(drone, anotherDrone))
                          .ToHashSet(DroneEqualityComparer.Instance);
        }

        /// <summary>
        /// Получает все дроны, с которыми заданный дрон может иметь коллизии
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        public IEnumerable<Drone> GetAllDangersFor(Drone drone)
        {
            return ListOfDrones.Except(new List<Drone> { drone })
                               .ToHashSet(DroneEqualityComparer.Instance);
        }

        /// <summary>
        /// Проверяет коллизию между двумя дронами
        /// </summary>
        /// <param name="drone1">Первый дрон</param>
        /// <param name="drone2">Второй дрон</param>
        /// <returns>True если коллизия есть, и fasle если её нет</returns>
        private bool HasCollision(Drone drone1, Drone drone2)
        {
            if (DroneEqualityComparer.Instance.Equals(drone1, drone2))
            {
                throw new ArgumentException("Параметры должны быть двумя разными дронами");
            }

            var actualDistance = Vector3.Distance(drone1.Position, drone2.Position);
            var criticalDistance = drone1.Radius + drone2.Radius;

            return actualDistance < criticalDistance
                || Math.Abs(criticalDistance - actualDistance) < 0.01;
        }
    }
}
