using System.Text;
using System.Text.Json;
using Ui.Models;
using Ui.Response;

namespace Ui.HttpRepository
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public UserService(HttpClient client)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public async Task<ApiResponse<UserDto>> GetCurrentUser()
        {
            var userResult = await _client.GetAsync("api/User");
            var userContent = await userResult.Content.ReadAsStringAsync();
            if (!userResult.IsSuccessStatusCode)
            {

            }
            var result = JsonSerializer.Deserialize<ApiResponse<UserDto>>(userContent, _options);
            return result;
        }

        public async Task<ApiResponse<UserDto>> UpdateUser(UserUpdateDto request)
        {
            var content = JsonSerializer.Serialize(request);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var updateResult = await _client.PutAsync("api/User", bodyContent);
            var updateContent = await updateResult.Content.ReadAsStringAsync();
            if (!updateResult.IsSuccessStatusCode)
            {

            }
            var result = JsonSerializer.Deserialize<ApiResponse<UserDto>>(updateContent, _options);
            return result;
        }

        public async Task<UserDto> GetAllUser()
        {
            throw new NotImplementedException();
        }
    }
}
