using ChatApp.API.DTOs;
using ChatApp.API.Models;
using ChatApp.DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.API.Interfaces
{
    public interface IAccountRepository
    {
        Task<ServiceResponse<UserDTO>> SignIn(string email, string password);
        Task<ServiceResponse<UserDTO>> Register(string name, string email, string password, string phoneNumber);
        Task SignOut();
    }
}
