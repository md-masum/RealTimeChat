using System.Net.Http.Json;
using Ui.Models;
using Ui.Response;

namespace Ui.HttpRepository
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ApiResponse<List<ChatMessage>>> GetConversationAsync(string contactId)
        {
            var data =  (await _httpClient.GetFromJsonAsync<ApiResponse<List<ChatMessage>>>($"api/chat/{contactId}"))!;
            return data;
        }
        public async Task<ApiResponse<List<UserDto>>> GetUsersAsync()
        {
            var data = await _httpClient.GetFromJsonAsync<ApiResponse<List<UserDto>>>("api/chat/users");
            return data!;
        }
        public async Task<ApiResponse<bool>> SaveMessageAsync(ChatMessage message)
        {
            await _httpClient.PostAsJsonAsync("api/chat", message);
            return new ApiResponse<bool>(true);
        }
    }
}
