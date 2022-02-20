using DroneModelTest.Models;
using DroneModelTest.Models.Drones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneModelTest.Services
{
    /// <summary>
    /// Сервис предоставления отчетов о проведенных симуляцих в разных форматах
    /// </summary>
    public class ReportService
    {
        /// <summary>
        /// Подготавливает отчет о симуляции и выводит его на консоль
        /// </summary>
        /// <param name="result">Результаты симуляции</param>
        public void ReportToConsole(SimulationResult result)
        {
            StringBuilder reportBuilder = new();

            reportBuilder.AppendLine($"Console report for Simulation [{result.Guid}]");
            reportBuilder.AppendLine($"Number of iterations {result.Iterations}");
            reportBuilder.Append("Has crushed drones: ");

            if (result.PerIterationResults?.Any(iterationResult => iterationResult.DroneSnapshots?.Any(drone => drone.Status == DroneStatus.Crushed) ?? false) ?? false)
            {
                reportBuilder.AppendLine("YES");
            }

            else
            {
                reportBuilder.AppendLine("NO");
            }

            reportBuilder.AppendLine("Iterations present below...");
            reportBuilder.AppendLine();

            foreach (var iterationResult in result.PerIterationResults ?? Enumerable.Empty<SimulationResultItem>())
            {
                reportBuilder.AppendLine($"Iteration: {iterationResult.Iteration}");

                foreach (var droneSnapshot in iterationResult.DroneSnapshots ?? Enumerable.Empty<DroneSnapshot>())
                {
                    reportBuilder.Append($"Drone[{droneSnapshot.Guid}] ");
                    reportBuilder.Append($"Status={droneSnapshot.Status} ");
                    reportBuilder.Append($"Position=(x: {droneSnapshot.Position.X,4:F2}, y: {droneSnapshot.Position.Y,4:F2}, z: {droneSnapshot.Position.Z,4:F2}) ");
                    reportBuilder.Append($"Velocity=(x: {droneSnapshot.Velocity.X,4:F2}, y: {droneSnapshot.Velocity.Y,4:F2}, z: {droneSnapshot.Velocity.Z,4:F2}) ");
                    reportBuilder.Append($"Acceleration=(x: {droneSnapshot.Acceleration.X,4:F2}, y: {droneSnapshot.Acceleration.Y,4:F2}, z: {droneSnapshot.Acceleration.Z,4:F2}) ");
                    reportBuilder.AppendLine();
                }

                reportBuilder.AppendLine();
            }

            Console.WriteLine(reportBuilder);
        }

        void PrintDroneStatus(DroneSnapshot drone)
        {
            Console.WriteLine("Drone[{0}]: Status={1} Position={2} Vel={3} Accel={4}", drone.Guid, drone.Status, drone.Position.ToString("F2"), drone.Velocity.Length(), drone.Acceleration.Length());
        }
    }
}
