using System.Security.Cryptography;
using System.Text;

namespace GithubApp_Lab1
{
    public class OAuthManager
    {
        private const string ClientId = "Ov23lipgmuySYTFnGGsi";
        private const string ClientSecret = "011e1557e0f6e7a35afb1f57586d7058d1dbccc3";
        private const string RedirectUri = "http://localhost:8080/callback";

        public string _accessToken;
        private DateTime? _tokenExpirationDate;

        private Converter _converter;

        public OAuthManager()
        {
            _converter = new Converter();
            LoadAccessToken();
        }

        public string GetAuthorizationUrl()
        {
            return $"https://github.com/login/oauth/authorize?client_id={ClientId}&redirect_uri={RedirectUri}&scope=repo,delete_repo";
        }

        public async Task<string> ExchangeCodeForTokenAsync(string code)
        {
            using (HttpClient client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "client_id", ClientId },
                    { "client_secret", ClientSecret },
                    { "code", code }
                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("https://github.com/login/oauth/access_token", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Ошибка при получении токена: {response.StatusCode} - {responseString}");
                }

                var token = responseString.Split('&')[0].Split('=')[1];
                _tokenExpirationDate = DateTime.Now.AddMinutes(60);
                SaveAccessToken(token);
                _accessToken = token;
                return token;
            }
        }

        public string GetAccessToken()
        {
            if (_accessToken == null || !_tokenExpirationDate.HasValue || DateTime.Now >= _tokenExpirationDate.Value)
            {
                throw new InvalidOperationException("Токен недействителен или истек. Необходимо выполнить повторную аутентификацию.");
            }
            return _accessToken;
        }

        private void LoadAccessToken()
        {
            if (File.Exists("accessToken.txt"))
            {
                string encryptedToken = File.ReadAllText("accessToken.txt");
                _accessToken = _converter.DecryptString(encryptedToken);

                _tokenExpirationDate = DateTime.Now.AddMinutes(60);
            }
        }

        private void SaveAccessToken(string token)
        {
            string encryptedToken = _converter.EncryptString(token);
            File.WriteAllText("accessToken.txt", encryptedToken);
        }
    }
}