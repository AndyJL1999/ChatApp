using ChatApp.UI_Library.Models;
using Maui_UI_Fiction_Library.API;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ChatAppWeb.Areas.UserAccount.Controllers
{
    [Area("UserAccount")]
    public class AuthController : Controller
    {
        private readonly IApiHelper _apiHelper;

        public AuthController(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel input)
        {

            if (ModelState.IsValid)
            {
                var user = await _apiHelper.Authenticate(input.Email, input.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Name)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);

                    return RedirectToAction("Index", "Home", new { area = "UserHome" });
                }

                ModelState.AddModelError(string.Empty, "Wrong Email or Password");
                return View(input);
            }
            else
            {
                return View(input);
            }
           
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel input)
        {
            if (ModelState.IsValid)
            {
                var user = await _apiHelper.Register(input.Name, input.Email, input.Password, input.PhoneNumber);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Name)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);

                    return RedirectToAction("Index", "Home", new { area = "UserHome" });
                }

                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View(input);
            }
            else
            {
                return View(input);
            }
        }

        [Authorize]
        public async Task LogOut()
        {
            HttpContext.Session.Clear();

            await _apiHelper.SignOut();
            await HttpContext.SignOutAsync();
        }
    }
}
