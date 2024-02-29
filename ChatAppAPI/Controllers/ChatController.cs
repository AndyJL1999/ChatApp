using ChatApp.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
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
    }
}
