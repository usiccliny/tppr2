using GithubApp_Lab1.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GithubApp_Lab1
{
    internal class RequestManager
    {
        private string appName = "My Local App";

        public async Task<UserInfo> GetUserInfoAsync(string _accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", appName);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await client.GetStringAsync("https://api.github.com/user");
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(response);
                return userInfo;
            }
        }

        public async Task<List<Repository>> GetUserRepositoriesAsync(string username, string _accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", appName);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await client.GetStringAsync($"https://api.github.com/users/{username}/repos");
                var repositories = JsonConvert.DeserializeObject<List<Repository>>(response);
                return repositories;
            }
        }

        public async Task<bool> CreateUserRepositoryAsync(string _accessToken, string reposName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", appName);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var newRepo = new
                {
                    name = reposName
                };

                var json = JsonConvert.SerializeObject(newRepo);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api.github.com/user/repos", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ошибка создания репозитория: {response.StatusCode} - {error}");
                    return false;
                }
            }
        }

        public async Task<bool> DeleteUserRepositoryAsync(string _accessToken, string username, string repoName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", appName);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await client.DeleteAsync($"https://api.github.com/repos/{username}/{repoName}");

                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> RenameUserRepositoryAsync(string _accessToken, string username, string currentRepoName, string newRepoName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", appName);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var renamePayload = new
                {
                    name = newRepoName
                };

                var json = JsonConvert.SerializeObject(renamePayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PatchAsync($"https://api.github.com/repos/{username}/{currentRepoName}", content);

                return response.IsSuccessStatusCode;
            }
        }
    }
}
