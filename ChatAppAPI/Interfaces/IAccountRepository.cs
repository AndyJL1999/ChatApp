using ChatApp.API.DTOs;
using ChatApp.API.Models;
using ChatApp.DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.API.Interfaces
{
    public interface IAccountRepository
    {
        Task<ServiceResponse<AuthUserDTO>> SignIn(string email, string password);
        Task<ServiceResponse<AuthUserDTO>> Register(string name, string email, string password, string phoneNumber);
        Task SignOut();
    }
}
