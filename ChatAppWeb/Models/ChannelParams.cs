using ChatApp.UI_Library.Models;

namespace ChatAppWeb.Models
{
    public class ChannelParams
    {
        public string ChannelId { get; set; }
        public List<MessageModel> Messages { get; set; }
    }
}
