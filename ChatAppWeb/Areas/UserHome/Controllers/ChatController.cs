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
        private readonly ICacheService _cacheService;

        [BindProperty]
        public CreateChatVM CreateChatVM { get; set; }
        public ChatVM ChatVM { get; set; }

        public ChatController(IChatHelper chatHelper, IUserHelper userHelper, 
            IMessageHelper messageHelper, IAuthHelper authHelper, ICacheService cacheService)
        {
            _chatHelper = chatHelper;
            _userHelper = userHelper;
            _messageHelper = messageHelper;
            _authHelper = authHelper;
            _cacheService = cacheService;
        }

        public async Task<IActionResult> Index(string channelId, string channelName)
        {
            string userToken = HttpContext.Session.GetString("access_token");
            string userId = HttpContext.Session.GetString("user_id");

            ChatVM chatVM = _cacheService.Get<ChatVM>(userId, channelId);

            if(chatVM == null)
            {
                if (string.IsNullOrEmpty(userToken) == false)
                    await _authHelper.GetUserInfo(userToken);

                var recipient = await _userHelper.GetRecipientFromChat(channelId);
                var chatMessages = await _messageHelper.GetAllMessagesFromChannel(channelId, 100, 0);

                chatVM = new ChatVM
                {
                    CurrentUserId = userId,
                    Channel = new ChannelModel { Id = channelId, Name = channelName, Type = "Chat" },
                    Recipient = recipient,
                    Messages = chatMessages.ToList()
                };
            }

            ChatVM = chatVM;// Populate ChatVM for Chat/Index

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
        public async Task<IActionResult> LoadMoreMessages([FromBody] LoadMessagesParams parameters)
        {
            var newMessages = await _messageHelper.GetAllMessagesFromChannel(parameters.ChannelId, 100, parameters.Messages.Count());

            return Json(new { success = true, newMessageList = newMessages.Reverse() });
        }

        [HttpPost]
        public void OnLeave([FromBody] OnLeaveChatParams parameters)
        {
            // When leaving a page collect all messages and store in cache
            if (HttpContext.Session.IsAvailable)
            {
                parameters.ChatVM.Messages.Reverse();
                _cacheService.Set(parameters.ChatVM.CurrentUserId, parameters.ChatVM.Channel.Id, parameters.ChatVM);
                _cacheService.Set(parameters.ChatVM.CurrentUserId, "SessionChannelList", parameters.Channels);
            }
        }

    }
}
