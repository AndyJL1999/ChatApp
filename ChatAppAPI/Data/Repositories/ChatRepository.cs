using ChatApp.API.DTOs;
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

        public async Task<ServiceResponse<NewChatDTO>> CreateChat(string userId, string currentUsersEmail, string number)
        {
            // Get the user you wish to chat with by phone number
            UserDTO recipient = _userRepo.GetUserByPhone(number);
            // Generate chat id
            string newChatId = Guid.NewGuid().ToString();

            if(recipient != null)
            {
                // Check if this chat pair already exists
                bool chatExists = await _chatData.DoesChatExist(userId, recipient.Id);

                if (chatExists)
                {
                    return new ServiceResponse<NewChatDTO>
                    {
                        Message = "This chat already exists!",
                        Success = false
                    };
                }

                // Create chat 
                await _chatData.UpsertChat(newChatId, $"{currentUsersEmail}/{recipient.Email}");

                // Create UserChat relationship for both chatters with the same chat id
                await InsertUserChat(userId, newChatId);
                await InsertUserChat(recipient.Id, newChatId);

                return new ServiceResponse<NewChatDTO>
                {
                    Data = new NewChatDTO // return new chat info for client
                    {
                        ChatId = newChatId,
                        RecipientId = recipient.Id,
                        RecipientName = recipient.Name,
                    },
                    Message = "Chat created!",
                    Success = true
                };
            }

            return new ServiceResponse<NewChatDTO>
            {
                Message = "NO user found with that number",
                Success = false
            };

        }

        public async Task InsertUserChat(string userId, string chatId)
        {
            // Generate id for UserChat relationship 
            string id = Guid.NewGuid().ToString();

            await _chatData.InsertUserChat(id, userId, chatId);
        }
    }
}
