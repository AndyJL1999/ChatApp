using ChatApp.API.DTOs;
using ChatApp.API.Models;
using ChatApp.DataAccess.Models;

namespace ChatApp.API.Interfaces
{
    public interface IUserRepository
    {
        UserDTO GetUserByPhone(string number);
    }
}
