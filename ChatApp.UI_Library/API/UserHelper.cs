using ChatApp.UI_Library.API.Interfaces;
using ChatApp.UI_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library.API
{
    public class UserHelper : IUserHelper
    {
        private readonly IApiHelper _apiHelper;

        public UserHelper(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<RecipientModel> GetRecipientFromChat(string chatId)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_apiHelper.ApiClient.BaseAddress + $"User/GetRecipientFromChat/{chatId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RecipientModel>();

                    return result;
                }

                return null;
            }
        }

        public async Task<IEnumerable<ChannelModel>> GetAllUserChannels()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_apiHelper.ApiClient.BaseAddress + "User/GetAllChannels"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<IEnumerable<ChannelModel>>();

                    return result;
                }

                return null;
            }
        }
    }
}
