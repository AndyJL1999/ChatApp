using ChatApp.API.DTOs;
using ChatApp.API.Interfaces;
using ChatApp.API.Models;
using ChatApp.DataAccess.Interfaces;
using ChatApp.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly IUserData _userData;
        private readonly IChatData _chatData;
        private readonly IMessageData _messageData;

        public UserRepository(DatabaseContext context, IUserData userData, IChatData chatData, IMessageData messageData)
        {
            _context = context;
            _userData = userData;
            _chatData = chatData;
            _messageData = messageData;
        }

        public async Task<ServiceResponse<IEnumerable<ChannelDTO>>> GetAllUserChannels(string userId)
        {
            if(string.IsNullOrEmpty(userId) == false)
            {
                var data = await _userData.GetAllChannels(userId);
                List<ChannelDTO> channels = new List<ChannelDTO>();

                foreach (var channel in data)
                {
                    string lastMessageFromChannel = await _messageData.GetLastMessage(channel.Id);

                    if(channel.Type == "Chat")
                    {
                        string recipientId = await _chatData.GetRecipientFromChat(userId, channel.Id);
                        string recipientName = (await GetUserByIdAsync(recipientId)).Name;
                        int unreadMessagesCount = await _messageData.GetUnreadMessagesCountForChat(channel.Id, userId);

                        channels.Add(new ChannelDTO(channel.Id, recipientName, ChannelType.Chat, lastMessageFromChannel, unreadMessagesCount));
                    }
                    else
                    {
                        channels.Add(new ChannelDTO(channel.Id, channel.Name, ChannelType.Group, lastMessageFromChannel, 0));
                    }
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
                Message = "User NOT Found",
                Success = false
            };
        }

        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
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

        public bool DoesUserExist(string userId)
        {
            var user = _context.AppUsers.FirstOrDefault(u => u.Id == userId);

            if(user != null)
            {
                return true;
            }

            return false;
        }
    }
}
