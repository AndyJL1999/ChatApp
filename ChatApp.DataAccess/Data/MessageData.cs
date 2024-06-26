﻿using ChatApp.DataAccess.DataAccess;
using ChatApp.DataAccess.Interfaces;
using ChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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

        public Task<IEnumerable<(string Id, string UserName, string UserId, string Content, DateTime SentAt)>> GetAllFromChannel(string channelId, int limit, int offset) =>_db
                .LoadData<(string Id, string UserName, string UserId, string Content, DateTime SentAt), dynamic>
                ("spMessage_GetAllFromChannel", new { ChannelId = channelId, Limit = limit, Offset = offset });

        public async Task<string> GetLastMessage(string channelId)
        {
            var result = await _db.LoadData<string, dynamic>("spMessage_GetLastMessageSent", new { ChannelId = channelId });

            return result.FirstOrDefault();
        }

        public async Task<int> GetUnreadMessagesCountForChat(string chatId, string userId)
        {
            var result = await _db.LoadData<int, dynamic>("spMessage_GetUnreadChatMessagesCount", new { ChatId = chatId, UserId = userId });

            return result.FirstOrDefault();
        }

        public Task InsertMessage(string id, string userId, string? groupId, string? chatId, string content,
            DateTime? sentAt, DateTime? deliveredAt, DateTime? seenAt) =>
            _db.SaveData("spMessage_Insert",
                new
                {
                    Id = id,
                    UserId = userId,
                    GroupId = groupId,
                    ChatId = chatId,
                    Content = content,
                    SentAt = sentAt,
                    DeliveredAt = deliveredAt,
                    SeenAt = seenAt
                });

        public Task DeleteMessage(string id) =>
            _db.SaveData("spMessage_Delete", new { Id = id });

        public Task InsertConnection(string connectionId, string channelId, string username) =>
            _db.SaveData("spConnections_Insert", new { ConnectionId = connectionId, ChannelId = channelId, Username = username });
        public Task DeleteConnection(string connectionId) =>
            _db.SaveData("spConnections_Delete", new { ConnectionId = connectionId });
        public Task<IEnumerable<Connection>> GetConnectionsByChannel(string channelId) =>
            _db.LoadData<Connection, dynamic>("spConnections_GetConnectionsByChannel", new { ChannelId = channelId });
    }
}
