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
    public class MessageHelper : IMessageHelper
    {
        private readonly IApiHelper _apiHelper;

        public MessageHelper(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IEnumerable<MessageModel>> GetAllMessagesFromChannel(string channelId)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_apiHelper.ApiClient.BaseAddress + $"Message/GetAllMessagesFromChannel/{channelId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<IEnumerable<MessageModel>>();

                    return result;
                }

                return null;
            }
        }

        public async Task<string> CreateMessage(string channelId, string channelType, string content)
        {
            var data = JsonContent.Create(new 
            {
                ChannelId = channelId,
                ChannelType = channelType,
                Content = content
            });

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_apiHelper.ApiClient.BaseAddress + "Message/CreateMessage", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;
            }
        }
    }
}
