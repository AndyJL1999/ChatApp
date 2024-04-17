using ChatApp.API.DTOs;
using ChatApp.API.Models;

namespace ChatApp.API.Interfaces
{
    public interface IMessageRepository
    {
        Task<ServiceResponse<IEnumerable<MessageDTO>>> GetAllFromChannel(string channelId);
        Task<string> InsertMessage(string userId, string channelId, ChannelType channelType, string content);
    }
}
