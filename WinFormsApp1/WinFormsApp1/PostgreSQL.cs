using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class PostgreSQL
    {
        private const string PostgresConnectionString = "Host=localhost;Port=5433;Username=postgres;Password=11299133;Database=postgres";

        InteractionWithInterface interaction = new InteractionWithInterface();

        public void ExportDataToJson(DataGridView dataGrid, string outputPath)
        {
            List<string> selectedColumns = interaction.GetSelectedColumns(dataGrid);

            if (selectedColumns.Count == 0)
            {
                MessageBox.Show("Нет выбранных столбцов.");
                return;
            }

            string query = @"
    WITH executor AS (
        SELECT t.assigned_to AS executor_id,
               e.first_name || ' ' || e.last_name || ', email: ' || e.email AS executor_name
          FROM task t
          JOIN employee e ON t.assigned_to = e.employee_id 
    ),
    link_project AS (
        SELECT t.project_id,
               p.project_name || ' status: ' || p.status AS task_link_project
          FROM task t 
          JOIN project p ON t.project_id = p.project_id
    ),
    leader AS (
        SELECT t.team_id, 
               'leader: ' || e.first_name || ' ' || e.last_name || ', email: ' || e.email AS team_leader
          FROM team t
          JOIN employee e ON t.lead_employee_id = e.employee_id 
    )
    SELECT e.first_name,
           e.last_name,
           e.email,
           p.project_name,
           p.start_date AS project_start_date,
           p.end_date AS project_end_date,
           p.status AS project_status,
           t.task_name,
           ex.executor_name,
           lp.task_link_project,
           t.status AS task_status,
           t.due_date AS task_due_date,
           t2.team_name,
           l.team_leader
      FROM employee e
      LEFT JOIN task t ON t.assigned_to = e.employee_id 
      LEFT JOIN project p ON p.project_id = t.project_id 
      LEFT JOIN executor ex ON ex.executor_id = t.assigned_to
      LEFT JOIN link_project lp ON lp.project_id = t.project_id
      LEFT JOIN project_team pt ON pt.project_id = p.project_id
      LEFT JOIN team t2 ON t2.team_id = pt.team_id
      LEFT JOIN leader l ON l.team_id = t2.team_id";

            string columns = string.Join(", ", selectedColumns);
            string finalQuery = $"SELECT {columns} FROM ({query}) AS subquery;";

            using (var connection = new NpgsqlConnection(PostgresConnectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(finalQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    var dataList = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        foreach (string column in selectedColumns)
                        {
                            row[column] = reader[column];
                        }
                        dataList.Add(row);
                    }

                    string jsonOutput = JsonConvert.SerializeObject(dataList, Formatting.Indented);

                    File.WriteAllText(outputPath, jsonOutput);
                }
            }

            MessageBox.Show($"Данные успешно экспортированы в {outputPath}");
        }
    }
}
