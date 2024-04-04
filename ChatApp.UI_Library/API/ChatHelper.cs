using ChatApp.UI_Library.API.Interfaces;
using ChatApp.UI_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library.API
{
    public class ChatHelper : IChatHelper
    {
        private readonly IApiHelper _apiHelper;

        public ChatHelper(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<string> UpsertChat(string number)
        {
            var content = JsonContent.Create(number);

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_apiHelper.ApiClient.BaseAddress + "Chat/CreateChat", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return "Chat created successfully!";
                }

                return "Something went wrong.";
            }
        }
    }
}
