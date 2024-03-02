using ChatApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepo;

        public ChatController(IChatRepository chatRepo)
        {
            _chatRepo = chatRepo;
        }

        [HttpPost("CreateChat")]
        public async Task<IActionResult> CreateChat(string? id, string name)
        {
            try
            {
                await _chatRepo.UpsertChat(id, name);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost("CreateUserChat")]
        public async Task<IActionResult> CreateUserChat(string userId, string chatId)
        {
            try
            {
                await _chatRepo.InsertUserChat(userId, chatId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost("CreateMessage")]
        public async Task<IActionResult> CreateMessage(string content)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                await _chatRepo.InsertMessage(userId, content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
