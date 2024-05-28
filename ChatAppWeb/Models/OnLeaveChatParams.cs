using ChatApp.UI_Library.Models;
using ChatApp.UI_Library.ViewModels;

namespace ChatAppWeb.Models
{
    public class OnLeaveChatParams
    {
        public ChatVM ChatVM { get; set; }
        public List<ChannelModel> Channels { get; set; }
    }
}
