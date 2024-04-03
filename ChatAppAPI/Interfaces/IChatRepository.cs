namespace ChatApp.API.Interfaces
{
    public interface IChatRepository
    {
        Task CreateChat(string userId, string currentUsersName, string number);
        Task InsertMessage(string userId, string content);
    }
}
