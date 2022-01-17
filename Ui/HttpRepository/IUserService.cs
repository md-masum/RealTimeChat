using Ui.Models;
using Ui.Response;

namespace Ui.HttpRepository
{
    public interface IUserService
    {
        Task<ApiResponse<UserDto>> GetCurrentUser();
        Task<ApiResponse<UserDto>> UpdateUser(UserUpdateDto request);
        Task<UserDto> GetAllUser();
    }
}
