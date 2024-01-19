using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string? GroupId { get; set; }
        public string? ChatId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? SeenAt { get; set; }
    }
}
