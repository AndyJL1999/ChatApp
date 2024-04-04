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

        public ApiHelper(IConfiguration config)
        {
            _config = config;

            InitializeClient();
        }


        public HttpClient ApiClient { get; private set; }

        private void InitializeClient()
        {
            string api = _config.GetSection("AppSettings:ApiUrl").Value;

            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(api);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.Timeout = TimeSpan.FromSeconds(30);
        }      

    }
}
