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
    public class AuthHelper : IAuthHelper
    {
        private readonly IApiHelper _apiHelper;

        public AuthHelper(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<AuthenticatedUser> Authenticate(string email, string password)
        {
            var data = JsonContent.Create(new
            {
                Email = email,
                Password = password
            });

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_apiHelper.ApiClient.BaseAddress + "Account/SignIn", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<AuthenticatedUser>();

                    if (user != null)
                    {
                        _apiHelper.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.Token}");

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

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_apiHelper.ApiClient.BaseAddress + "Account/Register", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<AuthenticatedUser>();

                    if (user != null)
                    {
                        _apiHelper.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {user.Token}");

                        return user;
                    }
                }

                return null;
            }
        }

        public async Task SignOut()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_apiHelper.ApiClient.BaseAddress + "Account/LogOut", null))
            {
                if (response.IsSuccessStatusCode)
                {
                    _apiHelper.ApiClient.DefaultRequestHeaders.Remove("Authorization");
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }

        }
    }
}
