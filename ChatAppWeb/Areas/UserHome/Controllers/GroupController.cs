using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppWeb.Areas.UserHome.Controllers
{
    [Area("UserHome")]
    [Authorize]
    public class GroupController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
