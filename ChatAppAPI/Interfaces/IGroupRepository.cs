using ChatApp.API.Models;

namespace ChatApp.API.Interfaces
{
    public interface IGroupRepository
    {
        Task<ServiceResponse<dynamic>> CreateGroup(string userId, string groupName, List<string> numbers);
        Task InsertUserGroup(string userId, string groupId);
    }
}
