using ChatApp.UI_Library.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Maui_UI_Fiction_Library.API
{
    public class ApiHelper : IApiHelper
    {
        private HttpClient _apiClient;
        private readonly IConfiguration _config;

        public ApiHelper(IConfiguration config)
        {
            _config = config;

            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = _config.GetSection("AppSettings:ApiUrl").Value;


            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<AuthenticatedUser> Authenticate(string email, string password)
        {
            var data = JsonContent.Create(new
            {
                Email = email,
                Password = password
            });

            using (HttpResponseMessage response = await _apiClient.PostAsync(_apiClient.BaseAddress + "Account/SignIn", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<AuthenticatedUser>();

                    if (user != null)
                    {
                        _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.Token}");

                        return user;
                    }
                }

                return null;
            }
        }

        public async Task<AuthenticatedUser> Register(string name, string email, string password, string phoneNumber)
        {
            var data = JsonContent.Create(new
            {
                Name = name,
                Password = password,
                Email = email,
                PhoneNumber = phoneNumber
            });

            using (HttpResponseMessage response = await _apiClient.PostAsync(_apiClient.BaseAddress + "Account/Register", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<AuthenticatedUser>();

                    if(user != null)
                    {
                        _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.Token}");

                        return user;
                    }
                }

                return null;
            }
        }

        public async Task SignOut()
        {
            using (HttpResponseMessage response = await _apiClient.PostAsync(_apiClient.BaseAddress + "Account/LogOut", null))
            {
                if (response.IsSuccessStatusCode)
                {
                    _apiClient.DefaultRequestHeaders.Remove("Authorization");
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }

        }

    }
}
