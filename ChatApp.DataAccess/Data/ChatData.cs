using ChatApp.DataAccess.DataAccess;
using ChatApp.DataAccess.Interfaces;
using ChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Data
{
    public class ChatData : IChatData
    {
        private readonly ISqlDataAccess _db;

        public ChatData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<string> GetRecipientFromChat(string currentUserId, string chatId)
        {
            var result = await _db.LoadData<string, dynamic>(
                "spUserChat_GetRecipientFromChat", new { CurrentUserId = currentUserId, ChatId = chatId });

            return result.FirstOrDefault();
        }

        public Task<IEnumerable<Chat>> GetAllChatsForUser(string userId) =>
            _db.LoadData<Chat, dynamic>("spChat_GetAllChatsForUser", new { UserId = userId });

        public Task UpsertChat(string id, string name) =>
            _db.SaveData("spChat_Upsert", new { Id = id, Name = name });

        public Task InsertUserChat(string id, string userId, string chatId) =>
            _db.SaveData("spUserChat_Insert", new { Id = id, UserId = userId, ChatId = chatId });
        
    }
}
