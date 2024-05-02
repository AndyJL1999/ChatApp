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
    public class GroupHelper : IGroupHelper
    {
        private readonly IApiHelper _apiHelper;

        public GroupHelper(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<NewGroupModel> UpsertGroup(string groupName, List<string> phoneNumbers)
        {
            var content = JsonContent.Create(new
            {
                GroupName = groupName,
                PhoneNumbers = phoneNumbers
            });

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_apiHelper.ApiClient.BaseAddress + "Group/CreateGroup", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<NewGroupModel>();
                }

                return null;
            }
        }

        public async Task<string> JoinGroup(string userId, string groupId)
        {
            var content = JsonContent.Create(new
            {
                UserId = userId,
                GroupId = groupId
            });

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_apiHelper.ApiClient.BaseAddress + "Group/JoinGroup", content))
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
