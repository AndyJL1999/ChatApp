namespace ChatApp.API.DTOs
{
    public class GetMessagesDTO
    {
        public string ChannelId { get; set; }
        public int Limit { get; set; } = 20;
        public int Offset { get; set; } = 0;
    }
}
