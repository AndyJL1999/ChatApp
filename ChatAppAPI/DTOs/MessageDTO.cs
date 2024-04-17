namespace ChatApp.API.DTOs
{
    public class MessageDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

    }
}
