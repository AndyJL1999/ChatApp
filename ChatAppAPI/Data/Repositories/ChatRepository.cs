using ChatApp.API.Interfaces;
using ChatApp.DataAccess.Interfaces;

namespace ChatApp.API.Data.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IChatData _chatData;

        public ChatRepository(IChatData chatData)
        {
            _chatData = chatData;
        }
        
        public async Task UpsertChat(string? id, string name)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
            }

            await _chatData.UpsertChat(id, name);
        }
    }
}
