using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Ui.AuthProviders;
using Ui.Models;
using Ui.Models.Auth.Response;
using Ui.Response;

namespace Ui.HttpRepository
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
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
        public async Task<ApiResponse<List<ChatMessage>>> SaveMessageAsync(SaveOrUpdateMessage message)
        {
            var content = JsonSerializer.Serialize(message);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var authResult = await _httpClient.PostAsync("api/chat", bodyContent);
            var authContent = await authResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<List<ChatMessage>>>(authContent, _options);
            if (!authResult.IsSuccessStatusCode)
            {
                
            }

            return result;
        }
    }
}
