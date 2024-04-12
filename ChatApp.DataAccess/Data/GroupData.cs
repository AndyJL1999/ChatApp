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
    public class GroupData : IGroupData
    {
        private readonly ISqlDataAccess _db;

        public GroupData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<Group>> GetAllGroupsForUser(string userId) =>
            _db.LoadData<Group, dynamic>("spGroup_GetAllGroupsForUser", new { UserId = userId });

        public Task UpsertGroup(string id, string name) =>
            _db.SaveData("spGroup_Upsert", new { Id = id, Name = name });

        public Task InsertUserGroup(string id, string userId, string groupId) =>
            _db.SaveData("spUserGroup_Insert", new { Id = id, UserId = userId, GroupId = groupId });
    }
}
