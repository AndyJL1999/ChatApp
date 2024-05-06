using ChatApp.API.Models;
using ChatApp.DataAccess.Interfaces;
using ChatApp.DataAccess.Models;

namespace ChatApp.API.DTOs
{
    public class ChannelDTO
    {
        public ChannelDTO(string id, string name, ChannelType type, string lastMessage)
        {
            Id = id;
            Name = name;
            Type = type;
            LastMessage = lastMessage;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ChannelType Type { get; set; }
        public string LastMessage { get; set; }
    }
}
