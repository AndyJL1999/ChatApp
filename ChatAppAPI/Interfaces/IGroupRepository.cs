using ChatApp.API.DTOs;
using ChatApp.API.Models;

namespace ChatApp.API.Interfaces
{
    public interface IGroupRepository
    {
        Task<ServiceResponse<NewGroupDTO>> CreateGroup(string userId, string groupName, List<string> numbers);
        Task<bool> InsertUserGroup(string userId, string groupId);
    }
}
