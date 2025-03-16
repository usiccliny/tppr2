using GithubApp_Lab1.model;
using Newtonsoft.Json;
using System.Diagnostics;

namespace GithubApp_Lab1
{
    public partial class Form1 : Form
    {
        private HttpServer _httpServer;
        private OAuthManager _oauthManager;
        private RequestManager _requestManager;

        private string accessToken;

        public Form1()
        {
            InitializeComponent();
            _oauthManager = new OAuthManager();
            _httpServer = new HttpServer(this);
            _requestManager = new RequestManager();
            Task.Run(() => _httpServer.StartServer());

            authButton.Click += btnLogin_Click;

            try
            {
                accessToken = _oauthManager.GetAccessToken();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblStatus.Visible = false;
            lblStatusUpd.Visible = false;

            string authorizationUrl = _oauthManager.GetAuthorizationUrl();

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = authorizationUrl
            };

            Process.Start(startInfo);
        }

        public async Task HandleRedirect(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Код авторизации не получен.");
                return;
            }

            try
            {
                accessToken = await _oauthManager.ExchangeCodeForTokenAsync(code);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async Task LoadUserRepositories()
        {
            try
            {
                UserInfo userInfo = await _requestManager.GetUserInfoAsync(accessToken);

                var repositories = await _requestManager.GetUserRepositoriesAsync(userInfo.Login, accessToken);

                listRepositories.Items.Clear();

                foreach (var repo in repositories)
                {
                    listRepositories.Items.Add($"{repo.FullName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении репозиториев: {ex.Message}");
            }
        }

        private async void btnGetRepos_Click(object sender, EventArgs e)
        {
            lblStatus.Visible = false;
            lblStatusUpd.Visible = false;

            await LoadUserRepositories();
        }

        private async void btnCreateRepos_Click(object sender, EventArgs e)
        {
            bool result = await _requestManager.CreateUserRepositoryAsync(accessToken, txtRepoName.Text);

            if (result)
            {
                lblStatus.Text = "Репозиторий успешно создан!";
                lblStatus.BackColor = Color.Green;
                await LoadUserRepositories();
            }
            else
            {
                lblStatus.Text = "Ошибка при создании репозитория.";
                lblStatus.BackColor = Color.Red;
            }

            lblStatus.Visible = true;
            lblStatusUpd.Visible = false;
        }

        private async void btnDeleteRepo_Click(object sender, EventArgs e)
        {
            if (listRepositories.SelectedItem != null)
            {
                lblStatus.Visible = false;
                lblStatusUpd.Visible = false;

                string selectedItem = listRepositories.SelectedItem.ToString();
                var parts = selectedItem.Split('/');

                string username = parts[0];
                string repoName = parts[1];

                bool result = await _requestManager.DeleteUserRepositoryAsync(accessToken, username, repoName);

                if (result)
                {
                    await LoadUserRepositories();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении репозитория.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите репозиторий для удаления.");
            }
        }

        private async void btnUpdateRepo_Click(object sender, EventArgs e)
        {
            if (listRepositories.SelectedItem != null)
            {
                lblStatus.Visible = false;

                string selectedItem = listRepositories.SelectedItem.ToString();
                var parts = selectedItem.Split('/');

                string currentUsername = parts[0];
                string currentRepoName = parts[1];
                string newRepoName = txtUpdate.Text.Trim();

                if (string.IsNullOrEmpty(newRepoName))
                {
                    MessageBox.Show("Пожалуйста, введите новое имя репозитория.");
                    return;
                }

                bool result = await _requestManager.RenameUserRepositoryAsync(accessToken, currentUsername, currentRepoName, newRepoName);

                if (result)
                {
                    LoadUserRepositories();
                    lblStatusUpd.Text = "Репозиторий успешно переименован!";
                    lblStatusUpd.BackColor = Color.Green;

                }
                else
                {
                    lblStatusUpd.Text = "Ошибка при переименовании репозитория";
                    lblStatusUpd.BackColor = Color.Red;
                }
                lblStatusUpd.Visible = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите репозиторий для переименования.");
            }
        }
    }
}