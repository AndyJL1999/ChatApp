using ChatApp.UI_Library.API.Interfaces;
using ChatApp.UI_Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppWeb.Areas.UserHome.Controllers
{
    [Area("UserHome")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatHelper _chatHelper;

        public ChatController(IChatHelper chatHelper)
        {
            _chatHelper = chatHelper;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChatVM chatVM)
        {
            string result = await _chatHelper.UpsertChat(chatVM.PhoneNumber);

            if (string.IsNullOrEmpty(result))
            {
                return View(chatVM);
            }

            return RedirectToAction("Index", "Home", new { area = "UserHome" });
        }
    }
}
