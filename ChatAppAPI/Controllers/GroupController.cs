using ChatApp.API.DTOs;
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
        public async Task<IActionResult> CreateGroup(CreateGroupDTO createGroup)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _groupRepo.CreateGroup(currentUserId, createGroup.GroupName, createGroup.PhoneNumbers);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest();
        }

        [HttpPost("JoinGroup")]
        public async Task<IActionResult> JoinGroup(string userId, string groupId)
        {
            var success = await _groupRepo.InsertUserGroup(userId, groupId);

            if (success)
            {
                return Ok("User has joined group!");
            }
            
            return BadRequest("Failed to join group");
        }

    }
}
