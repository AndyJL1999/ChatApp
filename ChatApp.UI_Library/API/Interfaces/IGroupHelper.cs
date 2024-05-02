using ChatApp.UI_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library.API.Interfaces
{
    public interface IGroupHelper
    {
        Task<NewGroupModel> UpsertGroup(string groupName, List<string> phoneNumbers);
        Task<string> JoinGroup(string userId, string groupId);
    }
}
