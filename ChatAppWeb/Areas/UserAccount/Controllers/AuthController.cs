using ChatApp.UI_Library.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApp.UI_Library.API.Interfaces;

namespace ChatAppWeb.Areas.UserAccount.Controllers
{
    [Area("UserAccount")]
    public class AuthController : Controller
    {
        private readonly IAuthHelper _authHelper;
        private readonly IUserHelper _userHelper;

        public AuthController(IAuthHelper authHelper, IUserHelper userHelper)
        {
            _authHelper = authHelper;
            _userHelper = userHelper;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginModel input)
        {
            if (ModelState.IsValid)
            {
                var user = await _authHelper.Authenticate(input.Email, input.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);
                    await _authHelper.GetUserInfo(user.Token);

                    string userId = await _userHelper.GetCurrentUserId();

                    HttpContext.Session.SetString("user_id", userId);
                    HttpContext.Session.SetString("access_token", user.Token);

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
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterModel input)
        {
            if (ModelState.IsValid)
            {
                var user = await _authHelper.Register(input.Name, input.Email, input.Password, input.PhoneNumber);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);
                    await _authHelper.GetUserInfo(user.Token);

                    string userId = await _userHelper.GetCurrentUserId();

                    HttpContext.Session.SetString("user_id", userId);
                    HttpContext.Session.SetString("access_token", user.Token);

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
        public async Task<IActionResult> LogOut()
        {
            string? userToken = HttpContext.Session.GetString("access_token");

            if (string.IsNullOrEmpty(userToken) == false)
            {
                await _authHelper.GetUserInfo(userToken);

                HttpContext.Session.Clear();

                await _authHelper.SignOut();
            }
                
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult GetTokenForPresenceHub()
        {
            return Json(new { success = true, token = HttpContext.Session.GetString("access_token")});
        }

    }
}
