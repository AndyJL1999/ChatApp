using ChatApp.API.DTOs;
using ChatApp.API.Extensions;
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
        public async Task<ActionResult<UserDTO>> SignIn(LoginDTO login)
        {
            var response = await _accountRepo.SignIn(login.Email, login.Password);

            if (response.Success)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Message);
            
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            var response = await _accountRepo.Register(register.Name, register.Email, register.Password, register.PhoneNumber);

            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Message);
            }
            
        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await _accountRepo.SignOut();

                return Ok("Successfully signed out");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
