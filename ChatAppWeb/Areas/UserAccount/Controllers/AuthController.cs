using ChatApp.DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppWeb.Areas.UserAccount.Controllers
{
    public class AuthController : Controller
    {
        public AuthController()
        {

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserVM user)
        {
            return View("UserHome/Views/Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserVM user)
        {
            return View("UserHome/Views/Index");
        }
    }
}
