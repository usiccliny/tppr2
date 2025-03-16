using Npgsql;
using WinFormsApp1.Model;
using System.Data.SQLite;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WinFormsApp1
{
    internal class SQLlite
    {
        readonly string connectionString = "Data Source=mydatabase.db;Version=3;";
        private const string PostgresConnectionString = "Host=localhost;Port=5433;Username=postgres;Password=11299133;Database=postgres";

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

        public void InsertData(string json)
        {
            if (!File.Exists(json))
            {
                MessageBox.Show("Файл JSON не найден.");
                return;
            }

            string jsonContent = File.ReadAllText(json);
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                MessageBox.Show("Файл JSON пуст.");
                return;
            }

            List<Project> records;
            try
            {
                records = JsonConvert.DeserializeObject<List<Project>>(jsonContent);
            }
            catch (JsonReaderException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return;
            }


            using (var sqliteConnection = new SQLiteConnection(connectionString))
            {
                sqliteConnection.Open();

                foreach (var record in records)
                {
                    string insertQuery = @"
                        INSERT INTO project (
                            first_name, last_name, email, project_name, project_start_date, 
                            project_end_date, project_status, task_name, executor_name, 
                            task_link_project, task_status, task_due_date, team_name, team_leader
                        ) VALUES (
                            @first_name, @last_name, @email, @project_name, @project_start_date, 
                            @project_end_date, @project_status, @task_name, @executor_name, 
                            @task_link_project, @task_status, @task_due_date, @team_name, @team_leader
                        )";

                    using (var insertCommand = new SQLiteCommand(insertQuery, sqliteConnection))
                    {
                        // Заполнение параметров
                        insertCommand.Parameters.AddWithValue("@first_name", record.first_name);
                        insertCommand.Parameters.AddWithValue("@last_name", record.last_name);
                        insertCommand.Parameters.AddWithValue("@email", record.email);
                        insertCommand.Parameters.AddWithValue("@project_name", record.project_name);
                        insertCommand.Parameters.AddWithValue("@project_start_date", record.project_start_date);
                        insertCommand.Parameters.AddWithValue("@project_end_date", record.project_end_date);
                        insertCommand.Parameters.AddWithValue("@project_status", record.project_status);
                        insertCommand.Parameters.AddWithValue("@task_name", record.task_name);
                        insertCommand.Parameters.AddWithValue("@executor_name", record.executor_name);
                        insertCommand.Parameters.AddWithValue("@task_link_project", record.task_link_project);
                        insertCommand.Parameters.AddWithValue("@task_status", record.task_status);
                        insertCommand.Parameters.AddWithValue("@task_due_date", record.task_due_date);
                        insertCommand.Parameters.AddWithValue("@team_name", record.team_name);
                        insertCommand.Parameters.AddWithValue("@team_leader", record.team_leader);

                        // Выполнение запроса
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public void TransferData()
        {
            using (var sqliteConnection = new SQLiteConnection(connectionString))
            {
                sqliteConnection.Open();
                string selectQuery = "SELECT * FROM project";

                using (var sqliteCommand = new SQLiteCommand(selectQuery, sqliteConnection))
                using (var reader = sqliteCommand.ExecuteReader())
                {
                    var employeeIds = new Dictionary<string, int>();
                    var projectIds = new Dictionary<string, int>();
                    var teamIds = new Dictionary<string, int>();

                    var employeesToInsert = new List<(string FirstName, string LastName, string Email)>();
                    var projectsToInsert = new List<(string ProjectName, DateTime StartDate, DateTime EndDate, string Status)>();
                    var tasksToInsert = new List<(string TaskName, string AssignedToEmail, string ProjectName, string Status, DateTime DueDate)>();
                    var teamsToInsert = new List<(string TeamName, string TeamLeaderEmail)>();

                    // Считываем данные из SQLite
                    while (reader.Read())
                    {
                        string firstName = reader["first_name"]?.ToString();
                        string lastName = reader["last_name"]?.ToString();
                        string email = reader["email"]?.ToString();
                        string projectName = reader["project_name"]?.ToString();
                        DateTime projectStartDate = DateTime.Parse(reader["project_start_date"]?.ToString());
                        DateTime projectEndDate = DateTime.Parse(reader["project_end_date"]?.ToString());
                        string projectStatus = reader["project_status"]?.ToString();
                        string taskName = reader["task_name"]?.ToString();
                        string taskStatus = reader["task_status"]?.ToString();
                        DateTime taskDueDate = DateTime.Parse(reader["task_due_date"]?.ToString());
                        string teamName = reader["team_name"]?.ToString();
                        string teamLeader = reader["team_leader"]?.ToString();
                        string teamLeaderEmail = teamLeader.Split(new[] { ", email: " }, StringSplitOptions.None)[1].Trim();

                        employeesToInsert.Add((firstName, lastName, email));
                        projectsToInsert.Add((projectName, projectStartDate, projectEndDate, projectStatus));
                        tasksToInsert.Add((taskName, email, projectName, taskStatus, taskDueDate));
                        teamsToInsert.Add((teamName, teamLeaderEmail));
                    }

                    using (var postgresConnection = new NpgsqlConnection(PostgresConnectionString))
                    {
                        postgresConnection.Open();

                        // 1. Вставка сотрудников
                        foreach (var (FirstName, LastName, Email) in employeesToInsert)
                        {
                            using (var employeeCommand = new NpgsqlCommand(@"
                    INSERT INTO employee (first_name, last_name, email) 
                    VALUES (@first_name, @last_name, @email) 
                    ON CONFLICT (email) DO NOTHING RETURNING employee_id", postgresConnection))
                            {
                                employeeCommand.Parameters.AddWithValue("@first_name", FirstName);
                                employeeCommand.Parameters.AddWithValue("@last_name", LastName);
                                employeeCommand.Parameters.AddWithValue("@email", Email);

                                var employeeId = employeeCommand.ExecuteScalar();
                                if (employeeId != null)
                                {
                                    employeeIds[Email] = (int)employeeId;
                                }
                            }
                        }

                        // 2. Вставка проектов
                        foreach (var (ProjectName, StartDate, EndDate, Status) in projectsToInsert)
                        {
                            using (var projectCommand = new NpgsqlCommand(@"
                    INSERT INTO project (project_name, start_date, end_date, status) 
                    VALUES (@project_name, @start_date, @end_date, @status) 
                    ON CONFLICT (project_name, status) DO NOTHING RETURNING project_id", postgresConnection))
                            {
                                projectCommand.Parameters.AddWithValue("@project_name", ProjectName);
                                projectCommand.Parameters.AddWithValue("@start_date", StartDate);
                                projectCommand.Parameters.AddWithValue("@end_date", EndDate);
                                projectCommand.Parameters.AddWithValue("@status", Status);

                                var projectId = projectCommand.ExecuteScalar();
                                if (projectId != null)
                                {
                                    projectIds[ProjectName] = (int)projectId;
                                }
                            }
                        }

                        // 3. Вставка задач
                        foreach (var (TaskName, AssignedToEmail, ProjectName, Status, DueDate) in tasksToInsert)
                        {
                            int assignedToId = employeeIds.TryGetValue(AssignedToEmail, out int empId) ? empId : -1;

                            // Проверка, существует ли проект
                            if (!projectIds.TryGetValue(ProjectName, out int projectId))
                            {
                                Console.WriteLine($"Проект \"{ProjectName}\" не найден. Пропускаем задачу: \"{TaskName}\"");
                                continue; // Пропускаем задачу, если проект не существует
                            }

                            using (var taskCommand = new NpgsqlCommand(@"
    INSERT INTO task (task_name, assigned_to, project_id, status, due_date) 
    VALUES (@task_name, @assigned_to, @project_id, @status, @due_date)
    ON CONFLICT (task_name, assigned_to, project_id, status) DO NOTHING", postgresConnection))
                            {
                                taskCommand.Parameters.AddWithValue("@task_name", TaskName);
                                taskCommand.Parameters.AddWithValue("@assigned_to", assignedToId);
                                taskCommand.Parameters.AddWithValue("@project_id", projectId);
                                taskCommand.Parameters.AddWithValue("@status", Status);
                                taskCommand.Parameters.AddWithValue("@due_date", DueDate);
                                taskCommand.ExecuteNonQuery();
                            }
                        }

                        // 4. Вставка команд
                        foreach (var (TeamName, LeaderEmail) in teamsToInsert)
                        {
                            // Проверяем, существует ли лидер в employee
                            if (!employeeIds.TryGetValue(LeaderEmail, out int leadId))
                            {
                                // Если лидера нет, можем либо пропустить добавление команды,
                                // либо добавить специальное значение (например, NULL) или выдать предупреждение.
                                Console.WriteLine($"Лидер команды {TeamName} не найден в базе сотрудников: {LeaderEmail}");
                                continue;
                            }

                            using (var teamCommand = new NpgsqlCommand(@"
                    INSERT INTO team (team_name, lead_employee_id) 
                    VALUES (@team_name, @lead_employee_id) 
                    ON CONFLICT (team_name, lead_employee_id) DO NOTHING RETURNING team_id", postgresConnection))
                            {
                                teamCommand.Parameters.AddWithValue("@team_name", TeamName);
                                teamCommand.Parameters.AddWithValue("@lead_employee_id", leadId);

                                var teamId = teamCommand.ExecuteScalar();
                                if (teamId != null)
                                {
                                    teamIds[TeamName] = (int)teamId;
                                }
                            }
                        }

                        // 5. Вставка в таблицу project_team
                        foreach (var projectEntry in projectIds)
                        {
                            foreach (var teamEntry in teamIds)
                            {
                                using (var projectTeamCommand = new NpgsqlCommand(@"
                        INSERT INTO project_team (project_id, team_id) 
                        VALUES (@project_id, @team_id) 
                        ON CONFLICT (project_id, team_id) DO NOTHING", postgresConnection))
                                {
                                    projectTeamCommand.Parameters.AddWithValue("@project_id", projectEntry.Value);
                                    projectTeamCommand.Parameters.AddWithValue("@team_id", teamEntry.Value);
                                    projectTeamCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}