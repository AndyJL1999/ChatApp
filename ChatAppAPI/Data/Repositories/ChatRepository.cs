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

        public async Task InsertUserChat(string userId, string chatId)
        {
            string id = Guid.NewGuid().ToString();

            await _chatData.InsertUserChat(id, userId, chatId);
        }

        public async Task InsertMessage(string userId, string content)
        {
            string id = Guid.NewGuid().ToString();

            await _chatData.InsertMessage(id, userId, null, null, content, null, null, null);
        }
    }
}
