﻿using ChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Interfaces
{
    public interface IMessageData
    {
        Task<IEnumerable<Message>> GetAllFromChannel(string channelId);
        Task InsertMessage(string id, string userId, string? groupId, string? chatId, string content,
            DateTime? sentAt, DateTime? deliveredAt, DateTime? seenAt);
        Task DeleteMessage(string id);
    }
}
