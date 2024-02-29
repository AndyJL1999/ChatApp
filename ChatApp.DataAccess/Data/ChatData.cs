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
        
    }
}
