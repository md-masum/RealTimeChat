using Core.Dto;

namespace Core.Interfaces.Common
{
    public interface IChatClient
    {
        Task ReceiveMessage(ConversationToReturnDto message);
        Task ReceiveChatNotification(string senderName);
        Task RtcClientProtocol(string data, string sender, string receiver);
    }
}
