using ChatApp.API.Interfaces;
using ChatApp.DataAccess.Models;
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

        [HttpGet]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            var result = await _accountRepo.SignIn(email, password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
            
        }

        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            var result = await _accountRepo.Register(name, email, password);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }

    }
}
