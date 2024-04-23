using ChatApp.API.Models;
using ChatApp.DataAccess.Interfaces;
using ChatApp.DataAccess.Models;

namespace ChatApp.API.DTOs
{
    public class ChannelDTO
    {
        public ChannelDTO(string id, string name, ChannelType type)
        {
            Id = id;
            Name = name;
            Type = type;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ChannelType Type { get; set; }
    }
}
