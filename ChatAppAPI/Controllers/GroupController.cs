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
    public class GroupController : Controller
    {
        private readonly IGroupRepository _groupRepo;

        public GroupController(IGroupRepository groupRepo)
        {
            _groupRepo = groupRepo;
        }

        [HttpPost("CreateGroup")]
        public async Task<IActionResult> CreateGroup(string groupName, List<string> numbers)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _groupRepo.CreateGroup(currentUserId, groupName, numbers);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
