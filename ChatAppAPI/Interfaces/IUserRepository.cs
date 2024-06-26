﻿using ChatApp.API.DTOs;
using ChatApp.API.Models;
using ChatApp.DataAccess.Models;

namespace ChatApp.API.Interfaces
{
    public interface IUserRepository
    {
        Task<ServiceResponse<IEnumerable<ChannelDTO>>> GetAllUserChannels(string userId);
        Task<ServiceResponse<UserDTO>> GetRecipientFromChat(string currentUserId, string chatId);
        Task<UserDTO> GetUserByIdAsync(string id);
        UserDTO GetUserByPhone(string number);
        bool DoesUserExist(string userId);
    }
}
