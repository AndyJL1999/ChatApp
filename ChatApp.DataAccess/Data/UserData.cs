using ChatApp.DataAccess.DataAccess;
using ChatApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Data
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<(string Id, string Name, string Type)>> GetAllChannels(string userId) =>
            _db.LoadData<(string Id, string Name, string Type), dynamic>("spProcedure_GetAllChannelsForUser", new { UserId = userId });
    }
}
