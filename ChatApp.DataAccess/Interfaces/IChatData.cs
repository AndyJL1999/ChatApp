using ChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Interfaces
{
    public interface IChatData
    {
        Task<string> GetRecipientFromChat(string currentUserId, string chatId);
        Task<IEnumerable<Chat>> GetAllChatsForUser(string userId);
        Task UpsertChat(string id, string name);
        Task InsertUserChat(string id, string userId, string chatId);
    }
}
