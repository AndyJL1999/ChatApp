using ChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Interfaces
{
    public interface IMessageData
    {
        Task<IEnumerable<(string Id, string UserName, string UserId, string Content, DateTime SentAt)>> GetAllFromChannel(string channelId, int limit, int offset);
        Task<string> GetLastMessage(string channelId);
        Task<int> GetUnreadMessagesCountForChat(string chatId, string userId);
        Task InsertMessage(string id, string userId, string? groupId, string? chatId, string content,
            DateTime? sentAt, DateTime? deliveredAt, DateTime? seenAt);
        Task DeleteMessage(string id);
    }
}
