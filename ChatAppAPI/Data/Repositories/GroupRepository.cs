using ChatApp.API.DTOs;
using ChatApp.API.Interfaces;
using ChatApp.API.Models;
using ChatApp.DataAccess.Data;
using ChatApp.DataAccess.Interfaces;

namespace ChatApp.API.Data.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IGroupData _groupData;
        private readonly IUserRepository _userRepo;

        public GroupRepository(IGroupData groupData, IUserRepository userRepo)
        {
            _groupData = groupData;
            _userRepo = userRepo;
        }

        public async Task<ServiceResponse<NewGroupDTO>> CreateGroup(string userId, string groupName, List<string> numbers)
        {
            // Generate group id
            string newGroupId = Guid.NewGuid().ToString();

            // Create Group
            await _groupData.UpsertGroup(newGroupId, groupName);
            // Insert current user into group
            await InsertUserGroup(userId, newGroupId);

            if(numbers.Count() > 0) // Check for any numbers to be included into group
            {
                foreach(string num in numbers)
                {
                    string userForGroupId = _userRepo.GetUserByPhone(num).Id;

                    if (userForGroupId != userId) // Make sure current user isn't being added again
                    {
                        // Create UserGroup relationship for both chatters with the same group id
                        await InsertUserGroup(userForGroupId, newGroupId);
                    }
                }
            }

            return new ServiceResponse<NewGroupDTO>
            {
                Data = new NewGroupDTO
                {
                    GroupId = newGroupId,
                    GroupName = groupName
                },
                Message = "Group created!",
                Success = true
            };
        }

        public async Task<bool> InsertUserGroup(string userId, string groupId)
        {
            // Generate id for UserChat relationship 
            string id = Guid.NewGuid().ToString();

            if (_userRepo.DoesUserExist(userId) && await DoesGroupExist(groupId))
            {
                await _groupData.InsertUserGroup(id, userId, groupId);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DoesGroupExist(string groupId)
        {
            var group = await _groupData.GetGroupById(groupId);

            if(group != null)
            {
                return true;
            }

            return false;
        }
    }
}
