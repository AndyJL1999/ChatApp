namespace ChatApp.API.Interfaces
{
    public interface IChatRepository
    {
        Task UpsertChat(string? id, string name);
        Task InsertUserChat(string userId, string chatId);
        Task InsertMessage(string userId, string content);
    }
}
