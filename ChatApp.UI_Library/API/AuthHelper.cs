using ChatApp.UI_Library.API.Interfaces;
using ChatApp.UI_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library.API
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IApiHelper _apiHelper;
        private readonly IAuthenticatedUser _authUser;

        public AuthHelper(IApiHelper apiHelper, IAuthenticatedUser authUser)
        {
            _apiHelper = apiHelper;
            _authUser = authUser;
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
                        return user;
                    }
                }

                return null;
            }
        }

        public async Task GetUserInfo(string token)
        {
            _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
            _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
            _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiHelper.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_apiHelper.ApiClient.BaseAddress + "User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<AuthenticatedUser>();

                    _authUser.Name = user.Name;
                    _authUser.Email = user.Email;
                    _authUser.PhoneNumber = user.PhoneNumber;
                    _authUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
                
            }

        }

        public async Task SignOut()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_apiHelper.ApiClient.BaseAddress + "Account/LogOut", null))
            {
                if (response.IsSuccessStatusCode)
                {
                    _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }

        }
    }
}
