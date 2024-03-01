using ChatApp.DataAccess.DataAccess;
using ChatApp.DataAccess.Interfaces;
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

        public Task UpsertChat(string id, string name) =>
            _db.SaveData("spChat_Upsert", new { Id = id, Name = name });

        public Task InsertUserChat(string id, string userId, string chatId) =>
            _db.SaveData("spUserChat_Insert", new { Id = id, UserId = userId, ChatId = chatId });

        public Task InsertMessage(string id, string userId, string? groupId, string? chatId, string content, 
            DateTime? sentAt, DateTime? deliveredAt, DateTime? seenAt) =>
            _db.SaveData("spMessage_Insert", 
                new { Id = id, UserID = userId, GroupId = groupId, ChatId = chatId, Content = content, 
                SentAt = sentAt, DeliveredAt = deliveredAt, SeenAt = seenAt });
        
    }
}
