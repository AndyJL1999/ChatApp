using ChatApp.API.DTOs;
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

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(LoginDTO login)
        {
            var result = await _accountRepo.SignIn(login.Email, login.Password);

            if (result.Succeeded)
            {
                return Ok("You have signed in");
            }

            return BadRequest("Failed to sign in");
            
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            var result = await _accountRepo.Register(register.Name, register.Email, register.Password, register.PhoneNumber);

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
