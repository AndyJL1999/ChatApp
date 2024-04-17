using ChatApp.API.DTOs;
using ChatApp.API.Extensions;
using ChatApp.API.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("GetAllChannels")]
        public async Task<IActionResult> GetAllChannels()
        {
            string id = User.GetUserId();

            var result = await _userRepo.GetAllUserChannels(id);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpGet("GetRecipientFromChat/{chatId}")]
        public async Task<IActionResult> GetRecipientFromChat(string chatId)
        {
            string id = User.GetUserId();

            var result = await _userRepo.GetRecipientFromChat(id, chatId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}
