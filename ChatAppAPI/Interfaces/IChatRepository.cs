using ChatApp.API.DTOs;
using ChatApp.API.Models;

namespace ChatApp.API.Interfaces
{
    public interface IChatRepository
    {
        Task<ServiceResponse<dynamic>> CreateChat(string userId, string currentUsersName, string number);
        Task InsertUserChat(string userId, string chatId);
    }
}
