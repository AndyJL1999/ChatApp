﻿using ChatApp.DataAccess.DataAccess;
using ChatApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Data
{
    public class MessageData : IMessageData
    {
        private readonly ISqlDataAccess _db;

        public MessageData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task InsertMessage(string id, string userId, string? groupId, string? chatId, string content,
            DateTime? sentAt, DateTime? deliveredAt, DateTime? seenAt) =>
            _db.SaveData("spMessage_Insert",
                new
                {
                    Id = id,
                    UserID = userId,
                    GroupId = groupId,
                    ChatId = chatId,
                    Content = content,
                    SentAt = sentAt,
                    DeliveredAt = deliveredAt,
                    SeenAt = seenAt
                });

        public Task DeleteMessage(string id) =>
            _db.SaveData("spMessage_Delete", new { Id = id });
    }
}
