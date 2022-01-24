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
        public async Task SendMessage(string sender, string receiver, string message)
        {
            await Clients.User(receiver).SendAsync("ReceiveMessage", sender, receiver, message);
        }
        // public async Task SendMessageAsync(ChatMessage message, string userName)
        // {
        //     await Clients.All.SendAsync("ReceiveMessage", message, userName);
        // }
        public async Task ChatNotificationAsync(string senderName, string receiverUserId, string senderUserId)
        {
            await Clients.User(receiverUserId).SendAsync("ReceiveChatNotification", senderName, receiverUserId, senderUserId);
        }
    }
}
