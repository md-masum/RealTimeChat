using System.Security.Claims;
using Core.Interfaces.Common;

namespace Api.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserName = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
            Email = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            Phone = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.MobilePhone);
        }

        public string? UserId { get; }
        public string? UserName { get; }
        public string? Email { get; }
        public string? Phone { get; }
    }
}
