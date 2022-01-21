using Core.Dto;
using Core.Entity;

namespace Core.Interfaces.Services
{
    public interface IChatService
    {
        Task<List<UserToReturnDto>> GetAllUser();
        Task<bool> SaveMessage(ChatMessageRequestDto message);
        Task<List<ChatMessage>> GetConversation(string contactId);
    }
}
