using Microsoft.AspNetCore.Mvc;

namespace ChatAppWeb.ViewComponents
{
    public class UserChatsViewComponent : ViewComponent
    {
        public UserChatsViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
