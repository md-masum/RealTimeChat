using System.Security.Claims;
using Core.Dto.Auth.Request;
using Core.Dto.Auth.Response;
using Core.Entity;
using Core.Response;

namespace Core.Interfaces.Common
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<string>> RegisterAsync(SignUpRequest request);
        Task ForgotPassword(ForgotPasswordRequest model);
        Task<ApiResponse<string>> ResetPassword(ResetPasswordRequest model);
        Task<ApiResponse<string>> ChangePassword(ChangePwdRequest model);
        Task<bool> CheckUserByEmail(string email);
        Task<bool> CheckUserByPhone(string phoneNumber);
        Task<IEnumerable<Claim>> GetUserClaims(ApplicationUser user);
        Task<ApplicationUser> GetLoggedInUser();
        Task<bool> ChangeUserEmail(string userId, string email);
        Task<bool> ChangeUserPhone(string userId, string phone);
        Task<bool> ChangeUserName(string userId, string name);
    }
}
