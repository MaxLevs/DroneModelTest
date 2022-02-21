using DroneModelTest.Models;
using DroneModelTest.Models.Drones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
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
            var reportBuilder = PrepareTextSimulationReport(result);
            Console.WriteLine(reportBuilder);
        }

        /// <summary>
        /// Создает текстовый отчет
        /// </summary>
        /// <param name="result">Результаты симуляции</param>
        /// <returns>Билдер текста с отчетом</returns>
        public StringBuilder PrepareTextSimulationReport(SimulationResult result)
        {
            StringBuilder reportBuilder = new();

            reportBuilder.AppendLine($"Console report for Simulation:\t[{result.Guid}]");
            reportBuilder.AppendLine($"Number of iterations:\t\t{result.Iterations}");
            reportBuilder.AppendLine($"Stopping excuse:\t\t{result.StoppingExcuse}");
            reportBuilder.Append("Has crushed drones:\t\t");

            if (result.PerIterationResults?.Any(iterationResult => iterationResult.DroneSnapshots?.Any(drone => drone.Status == DroneStatus.Crushed) ?? false) ?? false)
            {
                reportBuilder.AppendLine("YES");
            }

            else
            {
                reportBuilder.AppendLine("NO");
            }

            reportBuilder.AppendLine();

            foreach (var iterationResult in result.PerIterationResults ?? Enumerable.Empty<SimulationResultItem>())
            {
                reportBuilder.AppendLine($"Iteration: {iterationResult.Iteration}");

                AppendDronesInformationTableToStringBuiiler(reportBuilder, iterationResult.DroneSnapshots);

                reportBuilder.AppendLine();
            }

            reportBuilder.AppendLine();

            reportBuilder.AppendLine("===============");
            reportBuilder.AppendLine("| Crush cases |");
            reportBuilder.AppendLine("===============");
            reportBuilder.AppendLine();

            foreach (var crushCase in result.CrushPoints ?? Enumerable.Empty<CrushSnapshot>())
            {
                reportBuilder.AppendLine($"Position:\t\t{crushCase.Positon}");
                reportBuilder.AppendLine($"Actual distance:\t{crushCase.ActualDistance}");
                reportBuilder.AppendLine($"Critical distance:\t{crushCase.CriticalDistance}");
                AppendDronesInformationTableToStringBuiiler(reportBuilder, crushCase.GetEnumerable());
                reportBuilder.AppendLine();
            }

            return reportBuilder;
        }

        private void AppendDronesInformationTableToStringBuiiler(StringBuilder reportBuilder, IEnumerable<DroneSnapshot>? droneSnapshots)
        {
            PrintDataExtensions.SetOutputStringBuilder(reportBuilder);
            GetDronesInformationAsTable(droneSnapshots).Print();
        }

        private void PrintDroneInformation(StringBuilder reportBuilder, DroneSnapshot droneSnapshot)
        {
            reportBuilder.Append($"Drone[{droneSnapshot.Guid.ToString().Substring(0, 8)}] ");
            reportBuilder.Append($"Status={droneSnapshot.Status} ");
            reportBuilder.Append($"Radius={droneSnapshot.Radius,4:F2} ");
            reportBuilder.Append($"Position=(x: {droneSnapshot.Position.X,4:F2}, y: {droneSnapshot.Position.Y,4:F2}, z: {droneSnapshot.Position.Z,4:F2}) ");
            reportBuilder.Append($"Velocity=(x: {droneSnapshot.Velocity.X,4:F2}, y: {droneSnapshot.Velocity.Y,4:F2}, z: {droneSnapshot.Velocity.Z,4:F2}) ");
            reportBuilder.Append($"Acceleration=(x: {droneSnapshot.Acceleration.X,4:F2}, y: {droneSnapshot.Acceleration.Y,4:F2}, z: {droneSnapshot.Acceleration.Z,4:F2}) ");
        }

        private DataTable GetDronesInformationAsTable(IEnumerable<DroneSnapshot>? droneSnapshots)
        {
            DataTable table = new();
            table.TableName = "Drones";

            DataColumn idColumn           = new("#",            typeof(int))
            {
                AllowDBNull = false,
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1
            };
            DataColumn guidColumn         = new("Guid",         typeof(string));
            DataColumn statusColumn       = new("Status",       typeof(string));
            DataColumn radiusColumn       = new("Radius",       typeof(float));
            DataColumn positionColumn     = new("Position",     typeof(Vector3));
            DataColumn velocityColumn     = new("Velocity",     typeof(Vector3));
            DataColumn accelerationColumn = new("Acceleration", typeof(Vector3));

            table.Columns.Add(idColumn);
            table.Columns.Add(guidColumn);
            table.Columns.Add(statusColumn);
            table.Columns.Add(radiusColumn);
            table.Columns.Add(positionColumn);
            table.Columns.Add(velocityColumn);
            table.Columns.Add(accelerationColumn);

            if (droneSnapshots == null)
            {
                return table;
            }

            foreach(var drone in droneSnapshots)
            {
                DataRow row = table.NewRow();

                row.SetField(guidColumn, drone.Guid.ToString().Substring(0, 8));
                row.SetField(statusColumn, drone.Status.ToString());
                row.SetField(radiusColumn, drone.Radius);
                row.SetField(positionColumn, drone.Position);
                row.SetField(velocityColumn, drone.Velocity);
                row.SetField(accelerationColumn, drone.Acceleration);

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
