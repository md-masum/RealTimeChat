using Core.Dto;

namespace Core.Interfaces.Services
{
    public interface IChatService
    {
        Task<List<UserToReturnDto>> GetAllUser();
        Task<List<ConversationToReturnDto>> SaveMessage(ChatMessageRequestDto message);
        Task<List<ConversationToReturnDto>> UpdateMessage(ChatMessageRequestDto message);
        Task<List<ConversationToReturnDto>> DeleteMessage(string id);
        Task<List<ConversationToReturnDto>> GetConversation(string contactId);
        Task<List<ConversationToReturnDto>> GetAllConversation(string contactId);
        Task<List<ConversationToReturnDto>> GetAllConversation(string contactId, int batchNumber);
    }
}
