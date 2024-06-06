using ChatApp.API.DTOs;
using ChatApp.API.Models;
using ChatApp.DataAccess.Models;

namespace ChatApp.API.Interfaces
{
    public interface IMessageRepository
    {
        Task<ServiceResponse<IEnumerable<MessageDTO>>> GetAllFromChannel(string channelId, int limit, int offset);
        Task<ServiceResponse<MessageDTO>> InsertMessage(string userId, string channelId, ChannelType channelType, string content);
        Task InsertConnection(Connection connection);
        Task RemoveConnection(string connnectionId);
        Task<IEnumerable<Connection>> GetChannelConnections(string channelId);
    }
}
