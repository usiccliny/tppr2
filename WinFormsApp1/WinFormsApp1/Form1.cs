using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Process pythonProcess;

        private int expectedRows = 0;
        private int receivedRows = 0;

        private Client Client;

        public Form1()
        {
            InteractionWithInterface interaction = new InteractionWithInterface();

            InitializeComponent();

            Client = new Client();

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
                WorkingDirectory = exeDirectory
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
                MessageBox.Show($"Ошибка при поиске Python: {ex.Message}");
            }

            return null;
        }

        private void btnReadFromQueue_Click(object sender, EventArgs e)//раббит
        {
            var sslOptions = new SslOption
            {
                Enabled = true,
                ServerName = "localhost",
                CertPath = "C:/Certs/ServerCertOnly.pem",
                CertPassphrase = "key123456"
            };
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5671,
                UserName = "guest",
                Password = "guest",
                Ssl = sslOptions
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "test_queue1",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: true,
                                         arguments: null);

                    while (true)
                    {
                        var result = channel.BasicGet(queue: "test_queue1", autoAck: false);
                        if (result == null)
                        {
                            MessageBox.Show("Очередь пуста. Чтение завершено.");
                            break;
                        }

                        try
                        {
                            var body = result.Body.ToArray();
                            var json = Encoding.UTF8.GetString(body);
                            Project project = JsonConvert.DeserializeObject<Project>(json);

                            PostgreSQL postgre = new PostgreSQL();
                            postgre.Migrate(project);

                            channel.BasicAck(deliveryTag: result.DeliveryTag, multiple: false);

                            MessageBox.Show($"Получен проект: {project.ProjectName}");
                        }
                        catch (JsonException ex)
                        {
                            MessageBox.Show($"Ошибка десериализации: {ex.Message}");
                            channel.BasicNack(deliveryTag: result.DeliveryTag, multiple: false, requeue: false);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при обработке сообщения: {ex.Message}");
                            channel.BasicNack(deliveryTag: result.DeliveryTag, multiple: false, requeue: true);
                        }
                    }
                }
            }
            MessageBox.Show("Данные успешно получены!");
        }

        private void button1_Click(object sender, EventArgs e)//сокеты
        {
            TcpListener server = null;
            try
            {
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Any;
                server = new TcpListener(localAddr, port);
                server.Start();

                MessageBox.Show("Сервер запущен. Ожидание подключения...");

                using (TcpClient client = server.AcceptTcpClient())
                {
                    MessageBox.Show("Клиент подключен.");

                    using (SslStream sslStream = new SslStream(client.GetStream(), false))
                    {
                        X509Certificate2 serverCertificate = new X509Certificate2("C:\\Certs\\ServerCert.pfx", "key123456");

                        sslStream.AuthenticateAsServer(serverCertificate, clientCertificateRequired: false, SslProtocols.Tls12, checkCertificateRevocation: true);

                        using (StreamReader reader = new StreamReader(sslStream, Encoding.UTF8))
                        {
                            string line;
                            MessageBox.Show("Получение данных от клиента:");
                            PostgreSQL postgre = new PostgreSQL();
                            while ((line = reader.ReadLine()) != null)
                            {
                                try
                                {
                                    Project project = JsonConvert.DeserializeObject<Project>(line);
                                    postgre.Migrate(project);
                                }
                                catch (JsonException ex)
                                {
                                    MessageBox.Show($"Ошибка десериализации: {ex.Message}");
                                }
                            }
                        }

                        MessageBox.Show("Соединение закрыто.");
                    }
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Ошибка сокета: {0}", ex.Message);
            }
            finally
            {
                server.Stop();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PostgreSQL postgre = new PostgreSQL();
            postgre.TruncateTables();
        }

        private async void button3_Click(object sender, EventArgs e)
        { 
            startgrpc();
        }

        private void startgrpc()
        {
            var certPath = Path.Combine(AppContext.BaseDirectory, "Certificates", "server.pfx");
            var certPassword = "key123";

            try
            {
                var serverCert = new X509Certificate2(certPath, certPassword);

                var host = Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.ConfigureKestrel(options =>
                        {
                            options.ListenAnyIP(50051, listenOptions =>
                            {
                                listenOptions.UseHttps(serverCert);
                            });
                        });

                        webBuilder.ConfigureServices(services =>
                        {
                            services.AddGrpc();
                        });

                        webBuilder.Configure(app =>
                        {
                            app.UseRouting();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapGrpcService<MyProjectService>();
                            });
                        });
                    })
                    .Build();
                host.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start server: {ex.Message}");
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            Client.StartSocket();
        }

    }

}
