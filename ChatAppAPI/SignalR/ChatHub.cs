using ChatApp.API.DTOs;
using ChatApp.API.Interfaces;
using ChatApp.API.Models;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace ChatApp.API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IUserRepository _userRepo;

        public ChatHub(IMessageRepository messageRepo, IUserRepository userRepo)
        {
            _messageRepo = messageRepo;
            _userRepo = userRepo;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var channelId = httpContext.Request.Query["channelId"];

            await Groups.AddToGroupAsync(Context.ConnectionId, channelId);

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(CreateMessageDTO message)
        {
            ServiceResponse<MessageDTO> messageReceived = new ServiceResponse<MessageDTO>();

            if (message.ChannelType == "Chat")
            {
                messageReceived = await _messageRepo.InsertMessage(message.UserId, message.ChannelId, ChannelType.Chat, message.Content);
            }
            else
            {
                messageReceived = await _messageRepo.InsertMessage(message.UserId, message.ChannelId, ChannelType.Group, message.Content);
            }

            if(messageReceived.Data == null)
            {
                throw new HubException($"Message could not be created: {messageReceived.Message}");
            }

            await Clients.Group(message.ChannelId).SendAsync("ReceiveMessage", messageReceived.Data, messageReceived.Data.SentAt.ToString("hh:mm tt"));
        }
    }
}
