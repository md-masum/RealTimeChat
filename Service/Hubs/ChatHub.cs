using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Service.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string username, string message)
        {
            // var users = new string[] { message.ToUserId, message.FromUserId };
            //await Clients.Users(users).SendAsync("ReceiveMessage", message);
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }
    }
}
