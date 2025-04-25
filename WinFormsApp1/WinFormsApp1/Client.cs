using Grpc.Net.Client;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WinFormsApp1.Protos;

namespace WinFormsApp1
{
    internal class Client
    {
        string deviceName = "DESKTOP-B0RNFQA";
        int port = 13000;

        public static string GetIPv4AddressByNetworkName(string networkName = "Беспроводная сеть")
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
            {
                if ((networkInterface.Name.Equals(networkName, StringComparison.OrdinalIgnoreCase) ||
                     networkInterface.Description.Equals(networkName, StringComparison.OrdinalIgnoreCase)) &&
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    var ipProperties = networkInterface.GetIPProperties();

                    var ipv4Address = ipProperties.UnicastAddresses
                        .Where(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)
                        .Select(addr => addr.Address.ToString())
                        .FirstOrDefault();

                    if (!string.IsNullOrEmpty(ipv4Address))
                    {
                        return ipv4Address;
                    }
                }
            }

            throw new Exception($"Не удалось найти IPv4-адрес для сети с именем '{networkName}'.");
        }

        public void StartSocket()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(deviceName);

            IPAddress deviceIp = null;
            foreach (var address in hostEntry.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    deviceIp = address;
                    break;
                }
            }
            MessageBox.Show($"Используем IP-адрес: {deviceIp}");

            using (TcpClient client = new TcpClient(deviceIp.ToString(), port))
            {
                MessageBox.Show("Подключение к серверу...");

                using (SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null))
                {
                    X509Certificate2 caCertificate = new X509Certificate2("C:\\Certs\\RootCA.cer");

                    sslStream.AuthenticateAsClient("localhost");

                    SQLlite sQLlite = new SQLlite();
                    var projects = sQLlite.ReadDataFromSQLite();

                    using (StreamWriter writer = new StreamWriter(sslStream, Encoding.UTF8) { AutoFlush = true })
                    {
                        MessageBox.Show("Отправка данных...");
                        foreach (var project in projects)
                        {
                            string json = JsonConvert.SerializeObject(project);
                            writer.WriteLine(json);
                            MessageBox.Show($"Отправлен проект: {project.ProjectName}");
                        }
                    }
                    MessageBox.Show("Данные отправлены.");
                }
            }
        }

        public void StartRabbit()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(deviceName);
            IPAddress deviceIp = null;
            foreach (var address in hostEntry.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    deviceIp = address;
                    break;
                }
            }
            MessageBox.Show($"Используем IP-адрес: {deviceIp}");
            string rabbitmqHost = deviceIp.ToString();

            var sslOptions = new SslOption
            {
                Enabled = true,
                ServerName = "localhost"
            };
            var factory = new ConnectionFactory()
            {
                HostName = rabbitmqHost,
                Port = 5671,
                UserName = "user1",
                Password = "user1",
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

                    SQLlite sQLlite = new SQLlite();
                    var projects = sQLlite.ReadDataFromSQLite();

                    foreach (var project in projects)
                    {
                        string json = JsonConvert.SerializeObject(project);
                        byte[] data = Encoding.UTF8.GetBytes(json);

                        channel.BasicPublish(
                            exchange: "",
                            routingKey: "test_queue1",
                            basicProperties: null,
                            body: data
                        );
                    }
                    MessageBox.Show("Успешно отправлено!");
                }
            }
        }

        public async Task SendProjectsAsync(List<Project> projects)
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(deviceName);
            IPAddress deviceIp = null;
            foreach (var address in hostEntry.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    deviceIp = address;
                    break;
                }
            }
            MessageBox.Show($"Используем IP-адрес: {deviceIp}");


            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using var channel = GrpcChannel.ForAddress($"https://{deviceIp}:50051", new GrpcChannelOptions
            {
                HttpHandler = handler
            });

            var client = new ProjectService.ProjectServiceClient(channel);

            try
            {
                using var call = client.CreateProject();

                foreach (var project in projects)
                {
                    var projectRequest = new ProjectRequest
                    {
                        FirstName = project.FirstName,
                        LastName = project.LastName,
                        Email = project.Email,
                        ProjectName = project.ProjectName,
                        ProjectStartDate = new DateTimeOffset(project.ProjectStartDate).ToUnixTimeSeconds(),
                        ProjectEndDate = new DateTimeOffset(project.ProjectEndDate).ToUnixTimeSeconds(),
                        ProjectStatus = project.ProjectStatus,
                        TaskName = project.TaskName,
                        ExecutorName = project.ExecutorName,
                        TaskStatus = project.TaskStatus,
                        TaskDueDate = new DateTimeOffset(project.TaskDueDate).ToUnixTimeSeconds(),
                        TeamName = project.TeamName
                    };
                    await call.RequestStream.WriteAsync(projectRequest);
                    MessageBox.Show($"Отправился проект: {project.ProjectName}");
                }
                await call.RequestStream.CompleteAsync();
                var response = await call.ResponseAsync;
                Console.WriteLine($"Ответ сервера:: {response.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine($"Ошибка проверки сертификата: {sslPolicyErrors}");
            return false;
        }
    }
}

