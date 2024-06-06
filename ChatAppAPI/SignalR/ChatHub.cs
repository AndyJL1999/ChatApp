using ChatApp.API.DTOs;
using ChatApp.API.Extensions;
using ChatApp.API.Interfaces;
using ChatApp.API.Models;
using ChatApp.DataAccess.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IUserRepository _userRepo;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _tracker;

        public ChatHub(IMessageRepository messageRepo, IUserRepository userRepo, IHubContext<PresenceHub> presenceHub, PresenceTracker tracker)
        {
            _messageRepo = messageRepo;
            _userRepo = userRepo;
            _presenceHub = presenceHub;
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var channelId = httpContext.Request.Query["channelId"];

            await Groups.AddToGroupAsync(Context.ConnectionId, channelId);
            await AddToConnectionGroup(channelId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _messageRepo.RemoveConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDTO message)
        {
            ServiceResponse<MessageDTO> messageReceived = new ServiceResponse<MessageDTO>();

            var connections = await _messageRepo.GetChannelConnections(message.ChannelId);
            var recipient = await _userRepo.GetRecipientFromChat(message.UserId, message.ChannelId);

            if(connections.Any(c => c.Username == recipient.Data.Email))
            {
                // recipient has read new message
            }
            else
            {
                var recipientConnections = await _tracker.GetConnectionsForUser(recipient.Data.Email);
                if(recipientConnections != null)
                {
                    // send notification to recipient
                    var currentUser = await _userRepo.GetUserByIdAsync(message.UserId);
                    await _presenceHub.Clients.Clients(recipientConnections).SendAsync("NewMessageRecieved", message.Content, message.ChannelId, currentUser.Name);
                }
            }

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

        private async Task AddToConnectionGroup(string channelId)
        {
            var connection = new Connection
            {
                ConnectionId = Context.ConnectionId, 
                ChannelId = channelId, 
                Username = Context.User.GetUsername()
            };

            await _messageRepo.InsertConnection(connection);
        }

    }
}
