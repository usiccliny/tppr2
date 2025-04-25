using Npgsql;
using System.Data.SQLite;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WinFormsApp1
{
    internal class SQLlite
    {
        readonly string connectionString = "Data Source=mydatabase.db;Version=3;";
        private const string PostgresConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=11299133;Database=postgres";

        public void CreateTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS project (
                    first_name TEXT,
                    last_name TEXT,
                    email TEXT,
                    project_name TEXT,
                    project_start_date TEXT,
                    project_end_date TEXT,
                    project_status TEXT,
                    task_name TEXT,
                    executor_name TEXT,
                    task_link_project TEXT,
                    task_status TEXT,
                    task_due_date TEXT,
                    team_name TEXT,
                    team_leader TEXT
                )";

                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Project> ReadDataFromSQLite()
        {
            var projects = new List<Project>();

            using (var sqliteConnection = new SQLiteConnection(connectionString))
            {
                sqliteConnection.Open();
                string selectQuery = "SELECT * FROM project";

                using (var sqliteCommand = new SQLiteCommand(selectQuery, sqliteConnection))
                using (var reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var project = new Project
                        {
                            FirstName = reader["first_name"]?.ToString(),
                            LastName = reader["last_name"]?.ToString(),
                            Email = reader["email"]?.ToString(),
                            ProjectName = reader["project_name"]?.ToString(),
                            ProjectStartDate = DateTime.Parse(reader["project_start_date"]?.ToString()),
                            ProjectEndDate = DateTime.Parse(reader["project_end_date"]?.ToString()),
                            ProjectStatus = reader["project_status"]?.ToString(),
                            TaskName = reader["task_name"]?.ToString(),
                            ExecutorName = reader["executor_name"]?.ToString(),
                            TaskStatus = reader["task_status"]?.ToString(),
                            TaskDueDate = DateTime.Parse(reader["task_due_date"]?.ToString()),
                            TeamName = reader["team_name"]?.ToString()
                        };

                        projects.Add(project);
                    }
                }
            }

            return projects;
        }

        public void SaveDataToPostgreSQL(Project project)
        {
            using (var postgresConnection = new NpgsqlConnection(PostgresConnectionString))
            {
                postgresConnection.Open();

                var employeeIds = new Dictionary<string, int>();
                var projectIds = new Dictionary<string, int>();
                var teamIds = new Dictionary<string, int>();

                // Вставка сотрудников
                if (!employeeIds.ContainsKey(project.Email))
                {
                    using (var employeeCommand = new NpgsqlCommand(@"
                INSERT INTO employee (first_name, last_name, email) 
                VALUES (@first_name, @last_name, @email) 
                ON CONFLICT (email) DO NOTHING RETURNING employee_id", postgresConnection))
                    {
                        employeeCommand.Parameters.AddWithValue("@first_name", project.FirstName);
                        employeeCommand.Parameters.AddWithValue("@last_name", project.LastName);
                        employeeCommand.Parameters.AddWithValue("@email", project.Email);

                        var employeeId = employeeCommand.ExecuteScalar();
                        if (employeeId != null)
                        {
                            employeeIds[project.Email] = (int)employeeId;
                        }
                    }
                }

                // Вставка проектов
                if (!projectIds.ContainsKey(project.ProjectName))
                {
                    using (var projectCommand = new NpgsqlCommand(@"
                INSERT INTO project (project_name, start_date, end_date, status) 
                VALUES (@project_name, @start_date, @end_date, @status) 
                ON CONFLICT (project_name, status) DO NOTHING RETURNING project_id", postgresConnection))
                    {
                        projectCommand.Parameters.AddWithValue("@project_name", project.ProjectName);
                        projectCommand.Parameters.AddWithValue("@start_date", project.ProjectStartDate);
                        projectCommand.Parameters.AddWithValue("@end_date", project.ProjectEndDate);
                        projectCommand.Parameters.AddWithValue("@status", project.ProjectStatus);

                        var projectid = projectCommand.ExecuteScalar();
                        if (projectid != null)
                        {
                            projectIds[project.ProjectName] = (int)projectid;
                        }
                    }
                }

                // Вставка задач
                int assignedToId = employeeIds.TryGetValue(project.Email, out int empId) ? empId : -1;

                if (!projectIds.TryGetValue(project.ProjectName, out int projectId))
                {
                    Console.WriteLine($"Проект \"{project.ProjectName}\" не найден. Пропускаем задачу: \"{project.TaskName}\"");
                    return;
                }

                using (var taskCommand = new NpgsqlCommand(@"
            INSERT INTO task (task_name, assigned_to, project_id, status, due_date) 
            VALUES (@task_name, @assigned_to, @project_id, @status, @due_date)
            ON CONFLICT (task_name, assigned_to, project_id, status) DO NOTHING", postgresConnection))
                {
                    taskCommand.Parameters.AddWithValue("@task_name", project.TaskName);
                    taskCommand.Parameters.AddWithValue("@assigned_to", assignedToId);
                    taskCommand.Parameters.AddWithValue("@project_id", projectId);
                    taskCommand.Parameters.AddWithValue("@status", project.TaskStatus);
                    taskCommand.Parameters.AddWithValue("@due_date", project.TaskDueDate);
                    taskCommand.ExecuteNonQuery();
                }

                // Вставка команд
                string teamLeaderEmail = project.Email; // Предполагаем, что лидер команды - это тот же сотрудник
                if (!teamIds.ContainsKey(project.TeamName) && !string.IsNullOrEmpty(teamLeaderEmail))
                {
                    if (!employeeIds.TryGetValue(teamLeaderEmail, out int leadId))
                    {
                        return;
                    }

                    using (var teamCommand = new NpgsqlCommand(@"
                INSERT INTO team (team_name, lead_employee_id) 
                VALUES (@team_name, @lead_employee_id) 
                ON CONFLICT (team_name, lead_employee_id) DO NOTHING RETURNING team_id", postgresConnection))
                    {
                        teamCommand.Parameters.AddWithValue("@team_name", project.TeamName);
                        teamCommand.Parameters.AddWithValue("@lead_employee_id", leadId);

                        var teamId = teamCommand.ExecuteScalar();
                        if (teamId != null)
                        {
                            teamIds[project.TeamName] = (int)teamId;
                        }
                    }
                }

                // Вставка связей между проектами и командами
                if (projectIds.ContainsKey(project.ProjectName) && teamIds.ContainsKey(project.TeamName))
                {
                    using (var projectTeamCommand = new NpgsqlCommand(@"
                INSERT INTO project_team (project_id, team_id) 
                VALUES (@project_id, @team_id) 
                ON CONFLICT (project_id, team_id) DO NOTHING", postgresConnection))
                    {
                        projectTeamCommand.Parameters.AddWithValue("@project_id", projectIds[project.ProjectName]);
                        projectTeamCommand.Parameters.AddWithValue("@team_id", teamIds[project.TeamName]);
                        projectTeamCommand.ExecuteNonQuery();
                    }
                }
            }
        }
        

    }
}