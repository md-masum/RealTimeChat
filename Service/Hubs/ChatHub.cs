using Core.Entity;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Service.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ICurrentUserService _cureUserService;

        public ChatHub(ICurrentUserService cureUserService)
        {
            _cureUserService = cureUserService;
        }
        public async Task SendMessage(string sender, string recever, string message)
        {
            await Clients.User(recever).SendAsync("ReceiveMessage", sender, recever, message);
        }
        // public async Task SendMessageAsync(ChatMessage message, string userName)
        // {
        //     await Clients.All.SendAsync("ReceiveMessage", message, userName);
        // }
        public async Task ChatNotificationAsync(string message, string receiverUserId, string senderUserId)
        {
            await Clients.All.SendAsync("ReceiveChatNotification", message, receiverUserId, senderUserId);
        }
    }
}
