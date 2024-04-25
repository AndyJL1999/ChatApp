using ChatApp.UI_Library.API;
using ChatApp.UI_Library.API.Interfaces;
using ChatApp.UI_Library.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ChatApp.UI_Library.API
{
    internal class ApiHelper : IApiHelper
    {
        private readonly IConfiguration _config;
        private HttpClient _apiClient;

        public ApiHelper(IConfiguration config)
        {
            _config = config;

            InitializeClient();
        }


        public HttpClient ApiClient { get { return _apiClient; } }

        private void InitializeClient()
        {
            string api = _config.GetSection("AppSettings:ApiUrl").Value;

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.Timeout = TimeSpan.FromSeconds(30);
        }      

    }
}
