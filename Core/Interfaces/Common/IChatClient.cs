using Core.Dto;

namespace Core.Interfaces.Common
{
    public interface IChatClient
    {
        Task ReceiveMessage(ConversationToReturnDto message);
        Task ReceiveChatNotification(string senderName);
    }
}
