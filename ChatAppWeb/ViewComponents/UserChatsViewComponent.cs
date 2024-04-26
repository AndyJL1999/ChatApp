using ChatApp.UI_Library.API.Interfaces;
using ChatApp.UI_Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppWeb.ViewComponents
{
    public class UserChatsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(ChannelModel channel)
        {
            return View(channel);
        }
    }
}
