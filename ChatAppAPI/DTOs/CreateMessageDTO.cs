namespace ChatApp.API.DTOs
{
    public class CreateMessageDTO
    {
        public string ChannelId { get; set; }
        public string ChannelType { get; set; }
        public string Content { get; set; }
    }
}
