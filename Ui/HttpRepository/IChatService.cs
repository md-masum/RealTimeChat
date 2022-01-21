using Ui.Models;
using Ui.Response;

namespace Ui.HttpRepository
{
    public interface IChatService
    {
        Task<ApiResponse<List<UserDto>>> GetUsersAsync();
        Task<ApiResponse<bool>> SaveMessageAsync(ChatMessage message);
        Task<ApiResponse<List<ChatMessage>>> GetConversationAsync(string contactId);
    }
}
