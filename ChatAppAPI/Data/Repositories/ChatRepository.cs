using ChatApp.API.Interfaces;
using ChatApp.API.Models;
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
        
        public async Task<ServiceResponse<dynamic>> CreateChat(string userId, string currentUsersEmail, string number)
        {
            // Get the user you wish to chat with by phone number
            var userForChat = _userRepo.GetUserByPhone(number);
            // Generate chat id
            var newChatId = Guid.NewGuid().ToString();

            if(userForChat != null)
            {
                // Create chat 
                await _chatData.UpsertChat(newChatId, $"{currentUsersEmail}/{userForChat.Email}");

                return new ServiceResponse<dynamic>
                {
                    Data = new
                    {
                        UserForChatId = userForChat.Id,
                        NewChatId = newChatId
                    },
                    Message = "Chat created!",
                    Success = true
                };
            }

            return new ServiceResponse<dynamic>
            {
                Message = "NO user found with that number",
                Success = false
            };

        }

        public async Task InsertMessage(string userId, string content)
        {
            string id = Guid.NewGuid().ToString();

            await _chatData.InsertMessage(id, userId, null, null, content, DateTime.UtcNow, null, null);
        }

        public async Task InsertUserChat(string userId, string chatId)
        {
            // Generate id for UserChat relationship 
            string id = Guid.NewGuid().ToString();

            await _chatData.InsertUserChat(id, userId, chatId);
        }
    }
}
