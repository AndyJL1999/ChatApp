using ChatApp.API.Models;

namespace ChatApp.API.Interfaces
{
    public interface IChatRepository
    {
        Task<ServiceResponse<dynamic>> CreateChat(string userId, string currentUsersName, string number);
        Task InsertMessage(string userId, string content);
        Task InsertUserChat(string userId, string chatId);
    }
}
