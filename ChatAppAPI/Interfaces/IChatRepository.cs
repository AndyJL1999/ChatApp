namespace ChatApp.API.Interfaces
{
    public interface IChatRepository
    {
        Task UpsertChat(string? id, string name);
    }
}
