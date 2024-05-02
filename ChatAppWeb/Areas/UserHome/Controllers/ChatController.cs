using ChatApp.UI_Library.API.Interfaces;
using ChatApp.UI_Library.Models;
using ChatApp.UI_Library.ViewModels;
using ChatAppWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppWeb.Areas.UserHome.Controllers
{
    [Area("UserHome")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatHelper _chatHelper;
        private readonly IUserHelper _userHelper;
        private readonly IMessageHelper _messageHelper;
        private readonly IAuthHelper _authHelper;

        [BindProperty]
        public CreateChatVM CreateChatVM { get; set; }
        public ChatVM ChatVM { get; set; }

        public ChatController(IChatHelper chatHelper, IUserHelper userHelper, 
            IMessageHelper messageHelper, IAuthHelper authHelper)
        {
            _chatHelper = chatHelper;
            _userHelper = userHelper;
            _messageHelper = messageHelper;
            _authHelper = authHelper;
        }

        public async Task<IActionResult> Index(string channelId, string channelName)
        {
            string userToken = HttpContext.Session.GetString("access_token");
            IEnumerable<MessageModel> chatMessages;

            if (HttpContext.Session.Keys.Contains(channelId) == false) // Call APi if messages aren't in session
            {
                if (string.IsNullOrEmpty(userToken) == false)
                    await _authHelper.GetUserInfo(userToken);

                chatMessages = await _messageHelper.GetAllMessagesFromChannel(channelId, 100, 0);
            }
            else
            {
                // Get messages from session
                chatMessages = HttpContext.Session.Get<IEnumerable<MessageModel>>(channelId).Reverse();
            }

            ChatVM = new ChatVM // Populate ChatVM for Chat/Index
            {
                CurrentUserId = await _userHelper.GetCurrentUserId(),
                Channel = new ChannelModel { Id = channelId, Name = channelName, Type = "Chat" },
                Recipient = await _userHelper.GetRecipientFromChat(channelId),
                Messages = chatMessages.ToList()
            };

            return View(ChatVM);
        }

        public IActionResult Create()
        {
            CreateChatVM = new CreateChatVM
            {
                PhoneNumber = string.Empty
            };

            return View(CreateChatVM);
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePOST()
        {
            if (ModelState.IsValid)
            {
                NewChatModel result = await _chatHelper.UpsertChat(CreateChatVM.PhoneNumber);

                // If chat upsert is successful redirect to chat view => Chat/Index
                if (result != null)
                    return RedirectToAction("Index", new { result.ChatId, result.RecipientName });
                else
                    return BadRequest();
            }

            return View(CreateChatVM);
        }

        [HttpPost]
        public async Task<IActionResult> LoadMoreMessages([FromBody] ChannelParams parameters)
        {
            var newMessages = await _messageHelper.GetAllMessagesFromChannel(parameters.ChannelId, 100, parameters.Messages.Count());

            return Json(new { success = true, newMessageList = newMessages.Reverse() });
        }

        [HttpPost]
        public ActionResult OnLeave([FromBody] ChannelParams parameters)
        {
            // When leaving a page collect all messages and store in session
            if (HttpContext.Session.IsAvailable)
            {
                HttpContext.Session.Set(parameters.ChannelId, parameters.Messages);
            }

            return Json(new { success = true });
        }

    }
}
