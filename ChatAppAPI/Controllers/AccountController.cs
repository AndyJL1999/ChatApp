using ChatApp.API.Interfaces;
using ChatApp.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;

        public AccountController(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [HttpGet("SignIn")]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            var result = await _accountRepo.SignIn(email, password);

            if (result.Succeeded)
            {
                return Ok("You have sign in");
            }

            return BadRequest("Failed to sign in");
            
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            var result = await _accountRepo.Register(name, email, password);

            if (result.Succeeded)
            {
                return Ok("You have been registered");
            }
            else
            {
                return BadRequest("Something went wrong");
            }
            
        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await _accountRepo.SignOut();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Successfully signed out");
        }

    }
}
