using ChatApp.API.DTOs;
using ChatApp.API.Extensions;
using ChatApp.API.Interfaces;
using ChatApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepo;

        public MessageController(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
        }

        [HttpGet("GetAllMessagesFromChannel/{channelId}")]
        public async Task<IActionResult> GetAllMessagesFromChannel(string channelId)
        {
            var result = await _messageRepo.GetAllFromChannel(channelId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("CreateMessage")]
        public async Task<IActionResult> CreateMessage(CreateMessageDTO messageDTO)
        {
            ServiceResponse<MessageDTO> result;

            if (messageDTO.ChannelType == ChannelType.Chat.ToString())
            {
                result = await _messageRepo.InsertMessage(messageDTO.UserId, messageDTO.ChannelId, ChannelType.Chat, messageDTO.Content);
            }
            else
            {
                result = await _messageRepo.InsertMessage(messageDTO.UserId, messageDTO.ChannelId, ChannelType.Group, messageDTO.Content);
            }

            return Ok(result.Data);
        }
    }
}
