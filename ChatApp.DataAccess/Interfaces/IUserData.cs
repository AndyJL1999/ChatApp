using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Interfaces
{
    public interface IUserData
    {
        Task<IEnumerable<(string Id, string Name, string Type)>> GetAllChannels(string userId);
    }
}
