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

        public ChatHub(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
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

            await Clients.All.SendAsync("ReceiveMessage", messageReceived.Data, messageReceived.Data.SentAt.ToString("hh:mm tt"));
        }
    }
}
