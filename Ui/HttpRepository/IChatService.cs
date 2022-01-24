using Ui.Models;
using Ui.Response;

namespace Ui.HttpRepository
{
    public interface IChatService
    {
        Task<ApiResponse<List<UserDto>>> GetUsersAsync();
        Task<ApiResponse<List<ChatMessage>>> SaveMessageAsync(SaveOrUpdateMessage message);
        Task<ApiResponse<List<ChatMessage>>> GetConversationAsync(string contactId);
    }
}
