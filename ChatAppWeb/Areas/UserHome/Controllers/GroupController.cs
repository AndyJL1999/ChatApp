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

        [BindProperty]
        public CreateGroupVM CreateGroupVM { get; set; }
        public GroupVM GroupVM { get; set; }

        public GroupController(IMessageHelper messageHelper, IUserHelper userHelper, 
            IGroupHelper groupHelper, IAuthHelper authHelper)
        {
            _messageHelper = messageHelper;
            _userHelper = userHelper;
            _groupHelper = groupHelper;
            _authHelper = authHelper;
        }

        public async Task<IActionResult> Index(string channelId, string channelName)
        {
            string userToken = HttpContext.Session.GetString("access_token");
            IEnumerable<MessageModel>? groupMessages;

            if(HttpContext.Session.Keys.Contains(channelId) == false) // Call APi if message aren't in session
            {
                if (string.IsNullOrEmpty(userToken) == false)
                    await _authHelper.GetUserInfo(userToken);

                groupMessages = await _messageHelper.GetAllMessagesFromChannel(channelId);
            }
            else
            {
                // Get group messages from session
                groupMessages = HttpContext.Session.Get<IEnumerable<MessageModel>>(channelId);
            }
            // Get recipient names
            var recipients = groupMessages
                .DistinctBy(r => r.UserId)
                .Select(r => r.UserName)
                .ToList();

            GroupVM = new GroupVM // Populate GroupVM for Group/Index
            {
                CurrentUserId =  await _userHelper.GetCurrentUserId(),
                Channel = new ChannelModel { Id = channelId, Name = channelName, Type = "Group" },
                Recipients = recipients,
                Messages = groupMessages.ToList()
            };

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
