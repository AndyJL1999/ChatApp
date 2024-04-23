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
        private readonly IUserRepository _userRepo;

        public ChatController(IChatRepository chatRepo, IUserRepository userRepo)
        {
            _chatRepo = chatRepo;
            _userRepo = userRepo;
        }

        [HttpPost("CreateChat")]
        public async Task<IActionResult> CreateChat(string number)
        {
            string currentUserId = User.GetUserId();
            string currentUserEmail = User.FindFirstValue(ClaimTypes.Name);

            var user = _userRepo.GetUserByPhone(number);

            if (user.Id == currentUserId)
            {
                return BadRequest("Can't start chat with yourself");
            }

            var result = await _chatRepo.CreateChat(currentUserId, currentUserEmail, number);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}
