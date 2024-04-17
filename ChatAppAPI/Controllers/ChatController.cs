using ChatApp.API.Extensions;
using ChatApp.API.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        public async Task<IActionResult> CreateChat(string number)
        {
            var currentUserId = User.GetUserId();
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Name); 

            var result = await _chatRepo.CreateChat(currentUserId, currentUserEmail, number);

            if (result.Success)
            {
                // Create UserChat relationship for both chatters with the same chat id
                await _chatRepo.InsertUserChat(currentUserId, result.Data.NewChatId);
                await _chatRepo.InsertUserChat(result.Data.RecipientId, result.Data.NewChatId);

                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
