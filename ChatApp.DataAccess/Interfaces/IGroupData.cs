using ChatApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Interfaces
{
    public interface IGroupData
    {
        Task<IEnumerable<Group>> GetAllGroupsForUser(string userId);
        Task<Group> GetGroupById(string groupId);
        Task UpsertGroup(string id, string name);
        Task InsertUserGroup(string id, string userId, string groupId);
    }
}
