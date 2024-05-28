using ChatApp.UI_Library.Models;
using ChatApp.UI_Library.ViewModels;

namespace ChatAppWeb.Models
{
    public class OnLeaveGroupParams
    {
        public GroupVM GroupVM { get; set; }
        public List<ChannelModel> Channels { get; set; }
    }
}
