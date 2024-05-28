using ChatApp.DataAccess.Models;
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
    public class GroupController : Controller
    {
        private readonly IMessageHelper _messageHelper;
        private readonly IUserHelper _userHelper;
        private readonly IGroupHelper _groupHelper;
        private readonly IAuthHelper _authHelper;
        private readonly ICacheService _cacheService;

        [BindProperty]
        public CreateGroupVM CreateGroupVM { get; set; }
        public GroupVM GroupVM { get; set; }

        public GroupController(IMessageHelper messageHelper, IUserHelper userHelper, 
            IGroupHelper groupHelper, IAuthHelper authHelper, ICacheService cacheService)
        {
            _messageHelper = messageHelper;
            _userHelper = userHelper;
            _groupHelper = groupHelper;
            _authHelper = authHelper;
            _cacheService = cacheService;
        }

        public async Task<IActionResult> Index(string channelId, string channelName)
        {
            string userToken = HttpContext.Session.GetString("access_token");
            string userId = HttpContext.Session.GetString("user_id");

            GroupVM groupVM = _cacheService.Get<GroupVM>(userId, channelId);

            if(groupVM == null)
            {
                if (string.IsNullOrEmpty(userToken) == false)
                    await _authHelper.GetUserInfo(userToken);

                var groupMessages = await _messageHelper.GetAllMessagesFromChannel(channelId, 100, 0);

                // Get recipient names
                var recipients = groupMessages
                    .DistinctBy(r => r.UserId)
                    .Select(r => r.UserName)
                    .ToList();

                groupVM = new GroupVM
                {
                    CurrentUserId = userId,
                    Channel = new ChannelModel { Id = channelId, Name = channelName, Type = "Chat" },
                    Recipients = recipients,
                    Messages = groupMessages.ToList()
                };
            }

            GroupVM = groupVM; // Populate GroupVM for Group/Index

            return View(GroupVM);
        }

        public IActionResult Create()
        {
            CreateGroupVM = new CreateGroupVM
            {
                GroupName = string.Empty,
                PhoneNumbers = new List<string>()
            };

            return View(CreateGroupVM);
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePOST()
        {
            if (ModelState.IsValid)
            {
                NewGroupModel result = await _groupHelper.UpsertGroup(CreateGroupVM.GroupName, CreateGroupVM.PhoneNumbers);

                // If group upsert is successful redirect to chat view => Chat/Index
                if (result != null)
                    return RedirectToAction("Index", new { result.GroupId, result.GroupName });
                else
                    return BadRequest();
            }

            return View(CreateGroupVM);
        }

        [HttpPost]
        public async Task<IActionResult> LoadMoreMessages([FromBody] LoadMessagesParams parameters)
        {
            var newMessages = await _messageHelper.GetAllMessagesFromChannel(parameters.ChannelId, 100, parameters.Messages.Count());

            return Json(new { success = true, newMessageList = newMessages.Reverse() });
        }

        [HttpPost]
        public void OnLeave([FromBody] OnLeaveGroupParams parameters)
        {
            // When leaving a page collect all messages and store in cache
            if (HttpContext.Session.IsAvailable)
            {
                parameters.GroupVM.Messages.Reverse();
                _cacheService.Set(parameters.GroupVM.CurrentUserId, parameters.GroupVM.Channel.Id, parameters.GroupVM);
                _cacheService.Set(parameters.GroupVM.CurrentUserId, "SessionChannelList", parameters.Channels);
            }
        }
    }
}
