using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace ChatApp.API.SignalR
{
    public class ChatHub : Hub
    {
        public ChatHub()
        {

        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
