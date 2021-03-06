using Ui.Models.Auth.Request;
using Ui.Models.Auth.Response;
using Ui.Response;

namespace Ui.HttpRepository
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<string>> RegisterUser(SignUpRequest request);
        Task<ApiResponse<AuthResponse>> Login(LoginRequest request);
        Task<string> GetAccessToken();
        Task Logout();
        Task<bool> IsUserAuthenticated();
        Task<string> GetCurrentUserName();
        Task<string> GetCurrentUserEmail();
        Task<string> GetCurrentUserId();
    }
}
