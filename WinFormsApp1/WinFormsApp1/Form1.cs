using System.Diagnostics;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Process pythonProcess;

        public Form1()
        {
            InteractionWithInterface interaction = new InteractionWithInterface();

            InitializeComponent();

            interaction.SetDataGrid(this.dataGridView1);
        }

        private void RunPythonScriptButton_Click(object sender, EventArgs e)
        {
            PostgreSQL postgreSQL = new PostgreSQL();

            string exeDirectory = Application.StartupPath;
            string scriptPath = Path.Combine(exeDirectory, "script.py");
            string outJson = Path.Combine(exeDirectory, "output.json");

            postgreSQL.ExportDataToJson(dataGridView1, outJson);

            if (!File.Exists(scriptPath))
            {
                MessageBox.Show($"Файл {scriptPath} не найден.");
                return;
            }

            string pythonPath = FindPythonPath();
            if (string.IsNullOrEmpty(pythonPath))
            {
                MessageBox.Show("Python не найден.");
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/K \"{pythonPath} \"{scriptPath}\"\"", // /K оставляет консоль открытой
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = exeDirectory // Устанавливаем рабочую директорию
            };

            Process.Start(startInfo);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pythonProcess != null && !pythonProcess.HasExited)
            {
                pythonProcess.Kill();
                pythonProcess.Dispose();
            }
        }

        public static string FindPythonPath()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "where";
                process.StartInfo.Arguments = "python";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                string[] paths = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (paths.Length > 0)
                {
                    return paths[0].Trim();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске Python: {ex.Message}");
            }

            return null;
        }
    }
}
