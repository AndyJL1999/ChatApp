using ChatApp.UI_Library.Models;
using ChatApp.UI_Library.ViewModels;
using Maui_UI_Fiction_Library.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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
                await _apiHelper.Authenticate(input.Email, input.Password);

                return RedirectToAction("Index", "Home", new { area = "UserHome" });
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
                await _apiHelper.Register(input.Name, input.Email, input.Password, input.PhoneNumber);

                return RedirectToAction("Index", "Home", new { area = "UserHome" });
            }
            else
            {
                return View(input);
            }
        }
    }
}
