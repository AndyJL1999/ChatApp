using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Interfaces
{
    public interface IChatData
    {
        Task UpsertChat(string id, string name);
        Task InsertUserChat(string id, string userId, string chatId);
        Task InsertMessage(string id, string userId, string? groupId, string? chatId, string content,
            DateTime? sentAt, DateTime? deliveredAt, DateTime? seenAt);
    }
}
