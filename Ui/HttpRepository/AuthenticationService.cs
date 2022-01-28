using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Ui.AuthProviders;
using Ui.Models.Auth.Request;
using Ui.Models.Auth.Response;
using Ui.Response;

namespace Ui.HttpRepository
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly JsonSerializerOptions _options;
        public AuthenticationService(HttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<ApiResponse<string>> RegisterUser(SignUpRequest request)
        {
            var content = JsonSerializer.Serialize(request);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var registrationResult = await _client.PostAsync("api/auth/register", bodyContent);
            var registrationContent = await registrationResult.Content.ReadAsStringAsync();
            if (!registrationResult.IsSuccessStatusCode)
            {
                
            }
            var result = JsonSerializer.Deserialize<ApiResponse<string>>(registrationContent, _options);
            return result;
        }

        public async Task<ApiResponse<AuthResponse>> Login(LoginRequest request)
        {
            var content = JsonSerializer.Serialize(request);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var authResult = await _client.PostAsync("api/Auth/login", bodyContent);
            var authContent = await authResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<AuthResponse>>(authContent, _options);
            if (!authResult.IsSuccessStatusCode)
                return result;
            await _localStorage.SetItemAsync("authToken", result?.Data?.Token);
            if (request.UserName != null)
                ((AuthStateProvider) _authStateProvider).NotifyUserAuthentication(request.UserName);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Data.Token);
            return result;
        }

        public async Task<string> GetAccessToken()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user.Identity is { IsAuthenticated: true })
            {
                var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
                var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
                var timeUtc = DateTime.UtcNow;
                var diff = expTime - timeUtc;
                if (diff.TotalMinutes > 0)
                {
                    return true;
                }
            }
            await Logout();
            return false;
        }

        public async Task<string> GetCurrentUserName()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user.Identity is {IsAuthenticated: true})
            {
                return user.FindFirst(c => c.Type.Equals(ClaimTypes.Name))?.Value;
            }
            await Logout();
            return string.Empty;
        }

        public async Task<string> GetCurrentUserEmail()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user.Identity is { IsAuthenticated: true })
            {
                return user.FindFirst(c => c.Type.Equals(ClaimTypes.Email))?.Value;
            }
            await Logout();
            return string.Empty;
        }

        public async Task<string> GetCurrentUserId()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user.Identity is { IsAuthenticated: true })
            {
                return user.FindFirst(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            }
            await Logout();
            return string.Empty;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
