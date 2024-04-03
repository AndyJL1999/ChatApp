using ChatApp.API.Interfaces;
using ChatApp.DataAccess.Interfaces;

namespace ChatApp.API.Data.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IChatData _chatData;
        private readonly IUserRepository _userRepo;

        public ChatRepository(IChatData chatData, IUserRepository userRepo)
        {
            _chatData = chatData;
            _userRepo = userRepo;
        }
        
        public async Task CreateChat(string userId, string currentUsersName, string number)
        {
            // Get the user you wish to chat with by phone number
            var userForChat = _userRepo.GetUserByPhone(number);
            // Generate chat id
            var newChatId = Guid.NewGuid().ToString();

            // Create chat 
            await _chatData.UpsertChat(newChatId, $"{currentUsersName}/{userForChat.Name}");

            // Create UserChat relationship for both chatters with the same chat id
            await InsertUserChat(userId, newChatId);
            await InsertUserChat(userForChat.Id, newChatId);
        }

        public async Task InsertMessage(string userId, string content)
        {
            string id = Guid.NewGuid().ToString();

            await _chatData.InsertMessage(id, userId, null, null, content, null, null, null);
        }

        private async Task InsertUserChat(string userId, string chatId)
        {
            // Generate id for UserChat relationship 
            string id = Guid.NewGuid().ToString();

            await _chatData.InsertUserChat(id, userId, chatId);
        }
    }
}
