using ChatApp.UI_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library.API.Interfaces
{
    public interface IAuthHelper
    {
        Task<AuthenticatedUser> Authenticate(string email, string password);
        Task<AuthenticatedUser> Register(string name, string email, string password, string phoneNumber);
        Task GetUserInfo(string token);
        Task SignOut();
    }
}
