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

        public MessageRepository(IMessageData messageData)
        {
            _messageData = messageData;
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
                Data = null,
                Message = "Failed to acquire messages",
                Success = false
            };
        }

        public async Task<string> InsertMessage(string userId, string channelId, ChannelType channelType, string content)
        {
            string id = Guid.NewGuid().ToString();

            if(channelType == ChannelType.Chat)
            {
                await _messageData.InsertMessage(id, userId, null, channelId, content, DateTime.UtcNow, null, null);
            }
            else
            {
                await _messageData.InsertMessage(id, userId, channelId, null, content, DateTime.UtcNow, null, null);
            }

            return "Message created!";
        }
    }
}
