using ChatApp.API.DTOs;
using ChatApp.API.Interfaces;
using ChatApp.API.Models;
using ChatApp.DataAccess.Interfaces;
using ChatApp.DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly IChatData _chatData;
        private readonly IGroupData _groupData;

        public UserRepository(DatabaseContext context, IChatData chatData, IGroupData groupData)
        {
            _context = context;
            _chatData = chatData;
            _groupData = groupData;
        }

        public async Task<ServiceResponse<IEnumerable<ChannelDTO>>> GetAllUserChannels(string userId)
        {
            if(string.IsNullOrEmpty(userId) == false)
            {
                var chats = await _chatData.GetAllChatsForUser(userId);
                var groups = await _groupData.GetAllGroupsForUser(userId);

                Dictionary<string, List<IChannel>> pairs = new Dictionary<string, List<IChannel>>();

                pairs.Add("Groups", new List<IChannel>(groups));
                pairs.Add("Chats", new List<IChannel>(chats));

                List<ChannelDTO> channels = new List<ChannelDTO>();

                foreach (var pair in pairs)
                {
                    if(pair.Key == "Chats")
                        pair.Value.ForEach(c => channels.Add(new ChannelDTO(c, ChannelType.Chat)));
                    else
                        pair.Value.ForEach(c => channels.Add(new ChannelDTO(c, ChannelType.Group)));
                }

                return new ServiceResponse<IEnumerable<ChannelDTO>>
                {
                    Data = channels,
                    Message = "Channels acquired!",
                    Success = true
                };
            }

            return new ServiceResponse<IEnumerable<ChannelDTO>>
            {
                Data = null,
                Message = "Failed to acquire channels",
                Success = false
            };
        }

        public async Task<ServiceResponse<UserDTO>> GetRecipientFromChat(string currentUserId, string chatId)
        {
            string recipientId = await _chatData.GetRecipientFromChat(currentUserId, chatId);

            var user = _context.AppUsers.FirstOrDefault(u => u.Id == recipientId);

            if (user != null)
            {
                return new ServiceResponse<UserDTO>
                {
                    Data = new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    },
                    Message = "User Found",
                    Success = true
                };
            }

            return new ServiceResponse<UserDTO>
            {
                Data = null,
                Message = "User NOT Found",
                Success = false
            };
        }

        public UserDTO GetUserByPhone(string number)
        {
            var user = _context.AppUsers.FirstOrDefault(u => u.PhoneNumber == number);

            if(user != null)
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
            }

            return null;
        }
    }
}
