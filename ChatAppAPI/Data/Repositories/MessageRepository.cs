using ChatApp.API.DTOs;
using ChatApp.API.Interfaces;
using ChatApp.API.Models;
using ChatApp.DataAccess.Interfaces;
using ChatApp.DataAccess.Models;

namespace ChatApp.API.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMessageData _messageData;
        private readonly IUserRepository _userRepo;

        public MessageRepository(IMessageData messageData, IUserRepository userRepo)
        {
            _messageData = messageData;
            _userRepo = userRepo;
        }

        public async Task<ServiceResponse<IEnumerable<MessageDTO>>> GetAllFromChannel(string channelId)
        {
            if (string.IsNullOrEmpty(channelId) == false)
            {
                var result = await _messageData.GetAllFromChannel(channelId);
                List<MessageDTO> messages = new List<MessageDTO>();

                result
                    .ToList()
                    .ForEach(m => messages.Add(new MessageDTO
                    {
                        Id = m.Id,
                        UserName = m.UserName,
                        UserId = m.UserId,
                        Content = m.Content,
                        SentAt = m.SentAt
                    }));

                return new ServiceResponse<IEnumerable<MessageDTO>>
                {
                    Data = messages,
                    Message = "Messages aquired!",
                    Success = true
                };
            }

            return new ServiceResponse<IEnumerable<MessageDTO>>
            {
                Message = "Failed to acquire messages",
                Success = false
            };
        }

        public async Task<ServiceResponse<MessageDTO>> InsertMessage(string userId, string channelId, ChannelType channelType, string content)
        {
            string id = Guid.NewGuid().ToString();
            var user = await _userRepo.GetUserByIdAsync(userId);


            if(string.IsNullOrEmpty(content) == false)
            {
                if (channelType == ChannelType.Chat)
                {
                    await _messageData.InsertMessage(id, userId, null, channelId, content, DateTime.UtcNow, null, null);
                }
                else
                {
                    await _messageData.InsertMessage(id, userId, channelId, null, content, DateTime.UtcNow, null, null);
                }

                return new ServiceResponse<MessageDTO>
                {
                    Data = new MessageDTO
                    {
                        Id = id,
                        UserId = userId,
                        UserName = user.Name,
                        Content = content,
                        SentAt = DateTime.UtcNow
                    },
                    Message = "Message created!",
                    Success = true
                };
            }

            return new ServiceResponse<MessageDTO>
            {
                Message = "No message content found",
                Success = false
            };
            
        }
    }
}
